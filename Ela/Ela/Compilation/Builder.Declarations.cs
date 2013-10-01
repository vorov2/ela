using System;
using Ela.CodeModel;
using Ela.Runtime;
using System.Collections.Generic;

namespace Ela.Compilation
{
	internal sealed partial class Builder
    {
        //Compile all declarations including function bindings, name bindings and bindings
        //defined by pattern matching.
        private void CompileDeclaration(ElaEquation s, LabelMap map, Hints hints)
        {
            //Check for some errors
            ValidateBinding(s);
            var partial = (s.VariableFlags & ElaVariableFlags.PartiallyApplied) == ElaVariableFlags.PartiallyApplied;
            var fun = partial || s.IsFunction();

            if ((s.Left.Type != ElaNodeType.NameReference || ((ElaNameReference)s.Left).Uppercase) && !fun)
            {
                if ((hints & Hints.Lazy) == Hints.Lazy)                    
                    CompileLazyBindingPattern(s, map);
                else
                    CompileBindingPattern(s, map);
            }
            else
            {
                var nm = default(String);
                var sv = GetNoInitVariable(s, out nm);
                var addr = sv.Address;

                if (sv.IsEmpty())
                    addr = AddVariable(s.Left.GetName(), s, s.VariableFlags, -1);
                else
                    CurrentScope.AddFlags(nm, ElaVariableFlags.Self);

                //Compile expression and write it to a variable
                if (fun)
                {
                    //*****CURRENTLY NOT USED!
                    //Here we automatically eta expand functions defined through partial application
                    if (partial)
                        EtaExpand(s.Left.GetName(), s.Right, map, s.Arguments);
                    else if (CompileFunction(s))
                        CurrentScope.AddFlags(nm, ElaVariableFlags.Clean); //A function is clean
                }
                else
                {
                    map.BindingName = s.Left.GetName();

                    if ((hints & Hints.Lazy) == Hints.Lazy)
                        CompileLazyExpression(s.Right, map, Hints.None);
                    else
                    {
                        var ed = CompileExpression(s.Right, map, Hints.None, s);

                        if (ed.Type == DataKind.Builtin)
                            CurrentScope.AddFlagsAndData(nm, ElaVariableFlags.Builtin, ed.Data);
                    }
                }

                //Now, when done initialization, when can remove NoInit flags.
                if (!sv.IsEmpty())
                    CurrentScope.RemoveFlags(nm, ElaVariableFlags.NoInit | ElaVariableFlags.Self);

                AddLinePragma(s);
                PopVar(addr);
            }
        }

        //Compiles a binding defined by pattern matching.
        private void CompileBindingPattern(ElaEquation s, LabelMap map)
        {
            //Compile a right hand expression. Currently it is always compiled, event if right-hand
            //and both left-hand are tuples (however an optimization can be applied in such a case).
            var sys = AddVariable();
            CompileExpression(s.Right, map, Hints.None, s);
            AddLinePragma(s);
            PopVar(sys);

            //Labels needed for pattern compilation
            var next = cw.DefineLabel();
            var exit = cw.DefineLabel();

            //Here we compile a pattern a generate a 'handling logic' that raises a MatchFailed exception
            CompilePattern(sys, s.Left, next, false /*allowBang*/, false /*forceStrict*/);
            cw.Emit(Op.Br, exit);
            cw.MarkLabel(next);
            cw.Emit(Op.Failwith, (Int32)ElaRuntimeError.MatchFailed);
            cw.MarkLabel(exit);
            cw.Emit(Op.Nop);
        }

        //Validate correctness of a binding
        private void ValidateBinding(ElaEquation s)
        {
            //These errors are not critical and allow to continue compilation
            if (s.Right.Type == ElaNodeType.Builtin && s.Left.Type != ElaNodeType.NameReference)
                AddError(ElaCompilerError.InvalidBuiltinBinding, s);

            if ((s.VariableFlags & ElaVariableFlags.Private) == ElaVariableFlags.Private && CurrentScope != globalScope)
                AddError(ElaCompilerError.PrivateOnlyGlobal, s);

            //Bang patterns are only allowed in functions and constructors
            if (s.Left.Type == ElaNodeType.NameReference && ((ElaNameReference)s.Left).Bang)
            {
                AddError(ElaCompilerError.BangPatternNotValid, s, FormatNode(s.Left));
                AddHint(ElaCompilerHint.BangsOnlyFunctions, s);
            }
        }

        //Returns a variable from a local scope marked with NoInit flag
        //If such variable couldn't be found returns -1
        private ScopeVar GetNoInitVariable(ElaEquation s, out string name)
        {
            ScopeVar var;
            name = s.Left.GetName();

            if (s.IsFunction())
                name = s.GetFunctionName();
            
            if (CurrentScope.Locals.TryGetValue(name, out var))
            {
                //If it doesn't have a NoInit flag we are not good
                if ((var.Flags & ElaVariableFlags.NoInit) != ElaVariableFlags.NoInit)
                    return ScopeVar.Empty;
                else
                {
                    var.Address = 0 | var.Address << 8; //Aligning it to local scope
                    return var;
                }
            }

            return ScopeVar.Empty;
        }

        //Adds a variable with NoInit flag to the current scope
        //This method also calculates additional flags and metadata for variables.
        //If a given binding if defined by pattern matching then all variables from
        //patterns are traversed using AddPatternVariable method.
        private bool AddNoInitVariable(ElaEquation exp)
        {
            var flags = exp.VariableFlags | ElaVariableFlags.NoInit;
            
            //This binding is not defined by PM
            if (exp.IsFunction())
            {
                var name = exp.GetFunctionName();

                if (name == null || Char.IsUpper(name[0]))
                {
                    AddError(ElaCompilerError.InvalidFunctionDeclaration, exp, FormatNode(exp.Left));
                    return false;
                }

                AddVariable(name, exp.Left, flags | ElaVariableFlags.Function, exp.GetArgumentNumber());
            }
            else if (exp.Left.Type == ElaNodeType.NameReference && !((ElaNameReference)exp.Left).Uppercase)
            {
               var data = -1;

               if (exp.Right.Type == ElaNodeType.Builtin)
               {
                   //Adding required hints for a built-in
                   data = (Int32)((ElaBuiltin)exp.Right).Kind;
                   flags |= ElaVariableFlags.Builtin;
               }
               else if (exp.Right.Type == ElaNodeType.Lambda)
               {
                   var fun = (ElaLambda)exp.Right;
                   flags |= ElaVariableFlags.Function;
                   data = fun.GetParameterCount();
               }
               else if (exp.Right.IsLiteral())
                   flags |= ElaVariableFlags.ObjectLiteral;
               
               AddVariable(exp.Left.GetName(), exp, flags, data);
            }
            else
                AddPatternVariables(exp.Left);

            return true;
        }

        //Adding all variables from pattern as NoInit's. This method recursively walks 
        //all patterns. Currently we don't associate any additional metadata or flags 
        //(except of NoInit) with variables inferred in such a way.
        private void AddPatternVariables(ElaExpression pat)
        {
            switch (pat.Type)
            {
                case ElaNodeType.LazyLiteral:
                    {
                        //Not all forms of lazy patterns are supported,
                        //but it is easier to process them anyways. Errors will be caught
                        //during pattern compilation.
                        var vp = (ElaLazyLiteral)pat;
                        AddPatternVariables(vp.Expression);
                    }
                    break;
                case ElaNodeType.UnitLiteral: //Idle
                    break;
                case ElaNodeType.As:
                    {
                        var asPat = (ElaAs)pat;
                        AddVariable(asPat.Name, asPat, ElaVariableFlags.NoInit, -1);
                        AddPatternVariables(asPat.Expression);
                    }
                    break;
                case ElaNodeType.Primitive: //Idle
                    break;
                case ElaNodeType.NameReference:
                    {
                        var vexp = (ElaNameReference)pat;
                        
                        if (!vexp.Uppercase) //Uppercase is constructor
                            AddVariable(vexp.Name, vexp, ElaVariableFlags.NoInit, -1);
                    }
                    break;
                case ElaNodeType.RecordLiteral:
                    {
                        var rexp = (ElaRecordLiteral)pat;

                        foreach (var e in rexp.Fields)
                            if (e.FieldValue != null)
                                AddPatternVariables(e.FieldValue);
                    }
                    break;
                case ElaNodeType.TupleLiteral:
                    {
                        var texp = (ElaTupleLiteral)pat;

                        foreach (var e in texp.Parameters)
                            AddPatternVariables(e);
                    }
                    break;
                case ElaNodeType.Placeholder: //Idle
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        var hexp = (ElaJuxtaposition)pat;

                        foreach (var e in hexp.Parameters)
                            AddPatternVariables(e);
                    }
                    break;
                case ElaNodeType.ListLiteral: //Idle
                    {
                        var l = (ElaListLiteral)pat;

                        if (l.HasValues())
                        {
                            foreach (var e in l.Values)
                                AddPatternVariables(e);
                        }
                    }
                    break;
            }
        }

        //Compiles a pattern as lazy by a making a right hand a thunk and then delegating
        //a job to 'CompileLazyPattern' routine.
        private void CompileLazyBindingPattern(ElaEquation eq, LabelMap map)
        {
            CompileLazyExpression(eq.Right, map, Hints.None);
            var sys = AddVariable();
            PopVar(sys);
            CompileLazyPattern(sys, eq.Left, false);            
        }        
    }
}