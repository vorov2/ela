using System;
using System.Collections.Generic;
using Ela.CodeModel;
using Ela.Debug;
using Ela.Linking;

namespace Ela.Compilation
{
	internal sealed partial class Builder
	{
		private ElaCompiler comp; //Reference to compiler, to raise ModuleInclude event
                
        //Compilation options
        private bool debug; //Generate extended debug info
		private bool opt;   //Perform optimizations
        private CompilerOptions options;
		
        private CodeWriter cw; //Code emitter
		
        private CodeFrame frame; //Frame that is currently compiled
        private Scope globalScope; //Global scope for the current frame
        private Dictionary<String,Int32> stringLookup;  //String table
               
        private ExportVars exports; //Exports for current module
        
        //globalScope is not empty (e.g. new Scope()) only if we are resuming in interactive mode
		internal Builder(CodeFrame frame, ElaCompiler comp, ExportVars exportVars, Scope globalScope)
		{
			this.frame = frame;
			this.options = comp.Options;
			this.exports = exportVars;
			this.comp = comp;
			this.cw = new CodeWriter(frame.Ops, frame.OpData);
			this.globalScope = globalScope;
            			
			CurrentScope = globalScope;
			debug = options.GenerateDebugInfo;
			opt = options.Optimize;

			stringLookup = new Dictionary<String,Int32>();
            cleans.Push(true);

            ResumeIndexer();
            Success = true;			
		}

        //Entry point
		internal void CompileUnit(ElaProgram prog)
		{
            frame.Layouts.Add(new MemoryLayout(0, 0, 1)); //top level layout
			cw.StartFrame(0);

            //Handles and references should be of the same length. This might not
            //be the case, if we are resuming in interactive mode. We should "align" them,
            //so they are of the same length.
            if (refs.Count != frame.HandleMap.Count)
                for (var i = 0; i < frame.HandleMap.Count; i++)
                    refs.Add(null);

            //Determine whether this is fresh session.
            var scratch = cw.Offset == 0;
			
            //We always include prelude, but a module variable is created just once
            //(We check if we are not resuming in interactive mode (cw.Offset==0) and if yes
            //we don't create a variable 'prelude'.
            if (!String.IsNullOrEmpty(options.Prelude) && scratch)
                IncludePrelude(prog, scratch);

            //Always include arguments module
            if (scratch)
                IncludeArguments();

            //Another workaround that is needed for interactive mode. We need
            //to restore a reference list with correct indices (LogicalHandle).
            //It can be done by working through the reference map and requesting all referenced
            //modules one more time.
            if (cw.Offset != 0)
            {
                var arr = new ModuleReference[frame.References.Count];
                
                foreach (var kv in frame.References)
                    arr[kv.Value.LogicalHandle] = kv.Value;

                for (var i = 0; i < arr.Length; i++)
                {
                    var e = new ModuleEventArgs(arr[i]);
                    comp.OnModuleInclude(e);
                    refs[i] = e.Frame;
                }
            }
			            
            var map = new LabelMap();

            //Main compilation routine
            CompileProgram(prog, map);

            //Forcing evaluation of a program retval
            cw.Emit(Op.Force);
            
            //Every Ela module should end with a Stop op typeId
			cw.Emit(Op.Stop);
			cw.CompileOpList();

			frame.Layouts[0].Size = currentCounter;
			frame.Layouts[0].StackSize = cw.FinishFrame();
		}

        private ExprData CompileExpression(ElaExpression exp, LabelMap map, Hints hints, ElaExpression parent)
        {
            var exprData = ExprData.Empty;

            switch (exp.Type)
            {
                case ElaNodeType.As:
                case ElaNodeType.Equation:
                    AddError(ElaCompilerError.InvalidExpression, exp, FormatNode(exp));
                    break;
                case ElaNodeType.Context:
                    {
                        var v = (ElaContext)exp;

                        if (!v.Context.Parens && v.Context.Type == ElaNodeType.NameReference)
                        {
                            var nr = (ElaNameReference)v.Context;

                            if (nr.Uppercase)
                                EmitSpecName(null, "$$" + nr.Name, nr, ElaCompilerError.UndefinedType);
                            else
                            {
                                CompileExpression(v.Context, map, Hints.None, v);
                                cw.Emit(Op.Api, 5); //TypeCode
                            }
                        }
                        else if (!v.Context.Parens && v.Context.Type == ElaNodeType.FieldReference)
                        {
                            var fr = (ElaFieldReference)v.Context;

                            if (Char.IsUpper(fr.FieldName[0]) && fr.TargetObject.Type == ElaNodeType.NameReference)
                                EmitSpecName(fr.TargetObject.GetName(), "$$" + fr.FieldName, fr, ElaCompilerError.UndefinedType);
                            else
                            {
                                CompileExpression(v.Context, map, Hints.None, v);
                                cw.Emit(Op.Api, 5); //TypeCode
                            }
                        }
                        else
                        {
                            CompileExpression(v.Context, map, Hints.None, v);
                            cw.Emit(Op.Force);
                            cw.Emit(Op.Api, 5); //TypeCode
                        }

                        AddLinePragma(v);

                        var a = AddVariable();
                        cw.Emit(Op.Dup);
                        cw.Emit(Op.Ctx);
                        PopVar(a);
                        var newMap = map.Clone(a);

                        CompileExpression(v.Expression, newMap, hints, v);
                    }
                    break;
                case ElaNodeType.Builtin:
                    {
                        var v = (ElaBuiltin)exp;
                        CompileBuiltin(v.Kind, v, map, map.BindingName);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(exp);
                    }
                    break;
                case ElaNodeType.Generator:
                    {
                        CompileGenerator((ElaGenerator)exp, map, hints);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(exp);
                    }
                    break;
                case ElaNodeType.TypeCheck:
                    {
                        var n = (ElaTypeCheck)exp;
                        CompileTypeCheck(n, map, hints);
                    }
                    break;
                case ElaNodeType.Range:
                    {
                        var r = (ElaRange)exp;
                        CompileRange(exp, r, map, hints);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(r);
                    }
                    break;
                case ElaNodeType.LazyLiteral:
                    {
                        var v = (ElaLazyLiteral)exp;
                        CompileLazy(v, map, hints);
                    }
                    break;
                case ElaNodeType.LetBinding:
                    {
                        var v = (ElaLetBinding)exp;
                        StartScope(false, v.Line, v.Column);
                        CompileEquationSet(v.Equations, map);
                        CompileExpression(v.Expression, map, hints, v);
                        EndScope();
                    }
                    break;
                case ElaNodeType.Try:
                    {
                        var s = (ElaTry)exp;
                        CompileTryExpression(s, map, hints);
                    }
                    break;
                case ElaNodeType.Comprehension:
                    {
                        var c = (ElaComprehension)exp;
                        AddLinePragma(c);

                        if (c.Lazy)
                            CompileLazyList(c.Generator, map, hints);
                        else
                        {
                            cw.Emit(Op.Newlist);
                            CompileGenerator(c.Generator, map, Hints.None);
                            AddLinePragma(c);
                            cw.Emit(Op.Genfin);
                        }

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(c);
                    }
                    break;
                case ElaNodeType.Match:
                    {
                        var n = (ElaMatch)exp;
                        CompileMatchExpression(n, map, hints);
                    }
                    break;
                case ElaNodeType.Lambda:
                    {
                        var f = (ElaLambda)exp;
                        var pc = CompileLambda(f);
                        exprData = new ExprData(DataKind.FunParams, pc);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(exp);
                    }
                    break;
                case ElaNodeType.Condition:
                    {
                        var s = (ElaCondition)exp;
                        CompileConditionalOperator(s, map, hints);
                    }
                    break;
                case ElaNodeType.Raise:
                    {
                        var s = (ElaRaise)exp;
                        CompileExpression(s.Expression, map, Hints.None, s);
                        AddLinePragma(s);
                        cw.Emit(Op.Throw);
                    }
                    break;
                case ElaNodeType.Primitive:
                    {
                        var p = (ElaPrimitive)exp;
                        exprData = CompilePrimitive(p, map, hints);
                    }
                    break;
                case ElaNodeType.RecordLiteral:
                    {
                        var p = (ElaRecordLiteral)exp;
                        exprData = CompileRecord(p, map, hints);
                    }
                    break;
                case ElaNodeType.FieldReference:
                    {
                        var p = (ElaFieldReference)exp;

                        //Here we check if a field reference is actually an external name
                        //prefixed by a module alias. This call is not neccessary (modules
                        //are first class) but is used as optimization.
                        if (!TryOptimizeFieldReference(p))
                        {
                            CompileExpression(p.TargetObject, map, Hints.None, p);
                            cw.Emit(Op.Pushstr, AddString(p.FieldName));
                            AddLinePragma(p);
                            cw.Emit(Op.Pushfld);
                        }

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(exp);
                    }
                    break;
                case ElaNodeType.ListLiteral:
                    {
                        var p = (ElaListLiteral)exp;
                        exprData = CompileList(p, map, hints);
                    }
                    break;
                case ElaNodeType.NameReference:
                    {
                        var v = (ElaNameReference)exp;
                        AddLinePragma(v);
                        var scopeVar = GetVariable(v.Name, v.Line, v.Column);

                        //Bang patterns are only allowed in functions and constructors
                        if (v.Bang)
                        {
                            AddError(ElaCompilerError.BangPatternNotValid, exp, FormatNode(exp));
                            AddHint(ElaCompilerHint.BangsOnlyFunctions, exp);
                        }
                        
                        //This a polymorphic constant
                        if ((scopeVar.Flags & ElaVariableFlags.Polyadric) == ElaVariableFlags.Polyadric)
                        {
                            PushVar(scopeVar);
                            
                            if (map.HasContext)
                                PushVar(map.Context.Value);
                            else
                                cw.Emit(Op.Pushunit);

                            cw.Emit(Op.Disp);
                        }
                        else if ((scopeVar.Flags & ElaVariableFlags.Context) == ElaVariableFlags.Context)
                        {
                            //This is context value, not a real variable
                            cw.Emit(Op.Pushunit);
                            cw.Emit(Op.Api, (Int32)Api.CurrentContext);
                        }
                        else
                            PushVar(scopeVar);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(v);

                        //Add some hints if we know how this name is initialized
                        if ((scopeVar.VariableFlags & ElaVariableFlags.Function) == ElaVariableFlags.Function)
                            exprData = new ExprData(DataKind.FunParams, scopeVar.Data);
                        else if ((scopeVar.VariableFlags & ElaVariableFlags.ObjectLiteral) == ElaVariableFlags.ObjectLiteral)
                            exprData = new ExprData(DataKind.VarType, (Int32)ElaVariableFlags.ObjectLiteral);
                        else if ((scopeVar.VariableFlags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin)
                            exprData = new ExprData(DataKind.Builtin, scopeVar.Data);
                    }
                    break;
                case ElaNodeType.Placeholder:
                    {
                        if ((hints & Hints.Left) != Hints.Left)
                            AddError(ElaCompilerError.PlaceholderNotValid, exp);
                        else
                        {
                            AddLinePragma(exp);
                            cw.Emit(Op.Pop);
                        }
                    }
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        var v = (ElaJuxtaposition)exp;

                        CompileFunctionCall(v, map, hints);

                        if ((hints & Hints.Left) == Hints.Left)
                            AddValueNotUsed(v);
                    }
                    break;
                case ElaNodeType.UnitLiteral:
                    if ((hints & Hints.Left) != Hints.Left)
                        cw.Emit(Op.Pushunit);

                    exprData = new ExprData(DataKind.VarType, (Int32)ElaTypeCode.Unit);
                    break;
                case ElaNodeType.TupleLiteral:
                    {
                        var v = (ElaTupleLiteral)exp;
                        exprData = CompileTuple(v, map, hints);
                    }
                    break;
            }

            return exprData;
        }
		
        //Adds a string to a string table (strings are indexed).
		private int AddString(string val)
		{
			var index = 0;

            //The lookup is done to prevent redundant strings.
			if (!stringLookup.TryGetValue(val, out index))
			{
				frame.Strings.Add(val);
				index = frame.Strings.Count - 1;
				stringLookup.Add(val, index);
			}

			return index;
		}

		internal bool Success { get; private set; }
		
		internal Scope CurrentScope { get; private set; }
	}
}