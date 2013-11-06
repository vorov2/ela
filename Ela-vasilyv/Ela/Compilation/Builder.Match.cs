using System;
using Ela.CodeModel;
using Ela.Runtime;
using System.Collections.Generic;

namespace Ela.Compilation
{
    //This part contains compilation logic for pattern matching.
	internal sealed partial class Builder
	{
        //Used to compile a 'try' expression.
        private void CompileTryExpression(ElaTry n, LabelMap map, Hints hints)
        {
            var catchLab = cw.DefineLabel();
            var exitLab = cw.DefineLabel();
            
            //Generate a start of a 'try' section
            AddLinePragma(n);
            cw.Emit(Op.Start, catchLab);

            CompileExpression(n.Expression, map, Hints.None, n);

            //Leaving 'try' section
            cw.Emit(Op.Leave);
            cw.Emit(Op.Br, exitLab);
            cw.MarkLabel(catchLab);
            cw.Emit(Op.Leave);

            //Throw hint is to tell match compiler to generate a different typeId if 
            //all pattern fail - to rethrow an original error instead of generating a
            //new MatchFailed error.
            CompileSimpleMatch(n.Entries.Equations, map, hints | Hints.Throw, null);

            cw.MarkLabel(exitLab);
            cw.Emit(Op.Nop);

            if ((hints & Hints.Left) == Hints.Left)
                AddValueNotUsed(n);
        }

        //Used to compile a 'match' expression.
        private void CompileMatchExpression(ElaMatch n, LabelMap map, Hints hints)
        {
            CompileExpression(n.Expression, map, Hints.None, n);
            AddLinePragma(n);
            CompileSimpleMatch(n.Entries.Equations, map, hints, n.Expression);

            if ((hints & Hints.Left) == Hints.Left)
                AddValueNotUsed(n);
        }

        //This method contains main compilation logic for 'match' expressions and for 'try' expressions. 
        //This method only supports a single pattern per entry.
        //A 'mexp' (matched expression) parameter can be null and is used for additional validation only.
        private void CompileSimpleMatch(IEnumerable<ElaEquation> bindings, LabelMap map, Hints hints, ElaExpression mexp)
        {
            ValidateOverlapSimple(bindings, mexp);

            var failLab = cw.DefineLabel();
            var endLab = cw.DefineLabel();

            //We need to remembers the first set of system addresses because we will have
            //to push them manually for the entries other than first.
            var firstSys = -1;
            var first = true;

            //Loops through all bindings
            //For the first iteration we assume that all values are already on stack.
            //For the next iteration we manually push all values.
            foreach (var b in bindings)
            {
                //Each match entry starts a lexical scope
                StartScope(false, b.Line, b.Column);

                //For second+ entries we put a label where we would jump if a previous 
                //pattern fails
                if (!first)
                {
                    cw.MarkLabel(failLab);
                    failLab = cw.DefineLabel();
                }

                var p = b.Left;
                var sysVar = -1;

                //We apply an optimization if a we have a name reference (only for the first iteration).
                if (p.Type == ElaNodeType.NameReference && first)
                    sysVar = AddVariable(p.GetName(), p, ElaVariableFlags.Parameter, -1);
                else
                    sysVar = AddVariable(); //Create an unnamed system variable

                //This is not the first entry, so we have to push values first
                if (!first)
                    PushVar(firstSys);

                PopVar(sysVar);

                //We have to remember addresses calculated in a first iteration
                //to be able to push them once again in a second iteration.
                if (first)
                    firstSys = sysVar;

                CompilePattern(sysVar, p, failLab, false /*allowBang*/, false /*forceStrict*/);
                first = false;

                //Compile entry expression
                if (b.Right == null)
                    AddError(ElaCompilerError.InvalidMatchEntry, b.Left, FormatNode(b));
                else
                    CompileExpression(b.Right, map, Hints.None, b);

                //Close current scope
                EndScope();

                //If everything is OK skip through 'fail' clause
                cw.Emit(Op.Br, endLab);
            }

            //We fall here if all patterns have failed
            cw.MarkLabel(failLab);

            //If this hints is set than we generate a match for a 'try'
            //(exception handling) block. Instead of MatchFailed we need
            //to rethrow an original exception. Block 'try' always have
            //just a single expression to match, therefore firstSys[0] is
            //pretty safe here.
            if ((hints & Hints.Throw) == Hints.Throw)
            {
                PushVar(firstSys);
                cw.Emit(Op.Rethrow);
            }
            else
                cw.Emit(Op.Failwith, (Int32)ElaRuntimeError.MatchFailed);

            //Happy path for match
            cw.MarkLabel(endLab);
            cw.Emit(Op.Nop);
        }

        //Pattern match compilation method which is used by functions defined by pattern matching. 
        //Argument patNum contains number of patterns that should be in each.
        private void CompileFunctionMatch(int patNum, IEnumerable<ElaEquation> bindings, LabelMap map, Hints hints)
        {
            ValidateOverlapComplex(bindings);
            
            var failLab = cw.DefineLabel();
            var endLab = cw.DefineLabel();
            
            //We need to remembers the first set of system addresses because we will have
            //to push them manually for the entries other than first.
            var firstSys = new int[patNum];
            var first = true;

            //Loops through all bindings
            //For the first iteration we assume that all values are already on stack.
            //For the next iteration we manually push all values.
            foreach (var b in bindings)
            {
                //Each match entry starts a lexical scope
                StartScope(false, b.Line, b.Column);

                //This match compilation is used for functions only,
                //therefore the left-hand should always be a juxtaposition.
                var fc = (ElaJuxtaposition)b.Left;
                var len = fc.Parameters.Count;
                var pars = fc.Parameters;

                //Entries have different number of patterns, so generate an error and continue
                //compilation in order to prevent generation of 'redundant' error messages.
                if (len < patNum)
                    AddError(ElaCompilerError.PatternsTooFew, b.Left, FormatNode(b.Left), patNum, len);
                else if (len > patNum)
                    AddError(ElaCompilerError.PatternsTooMany, b.Left, FormatNode(b.Left), patNum, len);

                //For second+ entries we put a label where we would jump if a previous 
                //pattern fails
                if (!first)
                {
                    cw.MarkLabel(failLab);
                    failLab = cw.DefineLabel();
                }

                for (var i = 0; i < len; i++)
                {
                    var p = pars[i];
                    var sysVar = -1;

                    //We apply an optimization if a we have a name reference (only for the first iteration).
                    if (p.Type == ElaNodeType.NameReference && first)
                        sysVar = AddVariable(p.GetName(), p, ElaVariableFlags.Parameter, -1);
                    else
                        sysVar = AddVariable(); //Create an unnamed system variable

                    //This is not the first entry, so we have to push values first
                    if (!first && firstSys.Length > i)
                        PushVar(firstSys[i]);

                    PopVar(sysVar);

                    //We have to remember addresses calculated in a first iteration
                    //to be able to push them once again in a second iteration.
                    if (first && firstSys.Length > i)
                        firstSys[i] = sysVar;

                    
                }

                //Now compile patterns
                for (var i = 0; i < len; i++)
                    if (firstSys.Length > i && pars.Count > i)
                        CompilePattern(firstSys[i], pars[i], failLab, true /*allowBang*/, false /*forceStrict*/);
                
                first = false;

                //Compile entry expression
                if (b.Right == null)
                    AddError(ElaCompilerError.InvalidMatchEntry, b.Left, FormatNode(b));
                else
                    CompileExpression(b.Right, map, hints, b);

                //Close current scope
                EndScope();
                
                //If everything is OK skip through 'fail' clause
                cw.Emit(Op.Br, endLab);
            }

            //We fall here if all patterns have failed
            cw.MarkLabel(failLab);
            cw.Emit(Op.Failwith, (Int32)ElaRuntimeError.MatchFailed);

            //Happy path for match
            cw.MarkLabel(endLab);
            cw.Emit(Op.Nop);
        }

        //Compile a given expression as a pattern. If match fails proceed to failLab.
        private void CompilePattern(int sysVar, ElaExpression exp, Label failLab, bool allowBang, bool forceStrict)
        {
            AddLinePragma(exp);

            switch (exp.Type)
            {                    
                case ElaNodeType.LazyLiteral:
                    {
                        var n = (ElaLazyLiteral)exp;

                        //Normally this flag is set when everything is already compiled as lazy.
                        if (forceStrict)
                            CompilePattern(sysVar, n.Expression, failLab, allowBang, forceStrict);
                        else
                            CompileLazyPattern(sysVar, exp, allowBang);
                    }
                    break;
                case ElaNodeType.FieldReference:
                    {
                        //We treat this expression as a constructor with a module alias
                        var n = (ElaFieldReference)exp;
                        var fn = n.FieldName;
                        var alias = n.TargetObject.GetName();
                        PushVar(sysVar);

                        if (n.TargetObject.Type != ElaNodeType.NameReference)
                            AddError(ElaCompilerError.InvalidPattern, n, FormatNode(n));
                        else
                            EmitSpecName(alias, "$$$$" + fn, n, ElaCompilerError.UndefinedName);

                        cw.Emit(Op.Skiptag);
                        cw.Emit(Op.Br, failLab);
                    }
                    break;
                case ElaNodeType.NameReference:
                    {
                        //Irrefutable pattern, always binds expression to a name, unless it is 
                        //a constructor pattern
                        var n = (ElaNameReference)exp;

                        //Bang pattern are only allowed in constructors and functions
                        if (n.Bang && !allowBang)
                        {
                            AddError(ElaCompilerError.BangPatternNotValid, exp, FormatNode(exp));
                            AddHint(ElaCompilerHint.BangsOnlyFunctions, exp);
                        }

                        if (n.Uppercase) //This is a constructor
                        {
                            if (sysVar != -1)
                                PushVar(sysVar); 
                            
                            EmitSpecName(null, "$$$$" + n.Name, n, ElaCompilerError.UndefinedName);
            
                            //This op codes skips one offset if an expression
                            //on the top of the stack has a specified tag.
                            cw.Emit(Op.Skiptag);
                            cw.Emit(Op.Br, failLab);
                        }
                        else
                        {
                            var newV = false;
                            var addr = AddMatchVariable(n.Name, n, out newV);

                            //This is a valid situation, it means that the value is
                            //already on the top of the stack.
                            if (sysVar > -1 && newV)
                                PushVar(sysVar);

                            if (n.Bang)
                                cw.Emit(Op.Force);

                            //The binding is already done, so just idle.
                            if (newV)
                                PopVar(addr);
                        }
                    }
                    break;
                case ElaNodeType.UnitLiteral:
                    {
                        //Unit pattern is redundant, it is essentially the same as checking
                        //the type of an expression which is what we do here.
                        PushVar(sysVar);
                        cw.Emit(Op.Force);
                        cw.Emit(Op.PushI4, (Int32)ElaTypeCode.Unit);
                        cw.Emit(Op.Ctype);

                        //Types are not equal, proceed to fail.
                        cw.Emit(Op.Brfalse, failLab);
                    }
                    break;
                case ElaNodeType.Primitive:
                    {
                        var n = (ElaPrimitive)exp;

                        //Compare a given value with a primitive
                        PushVar(sysVar);
                        PushPrimitive(n.Value);
                        cw.Emit(Op.Cneq);

                        //Values not equal, proceed to fail.
                        cw.Emit(Op.Brtrue, failLab);
                    }
                    break;
                case ElaNodeType.As:
                    {
                        var n = (ElaAs)exp;
                        CompilePattern(sysVar, n.Expression, failLab, allowBang, false /*forceStrict*/);
                        var newV = false;
                        var addr = AddMatchVariable(n.Name, n, out newV);
                        PushVar(sysVar);
                        PopVar(addr);
                    }
                    break;
                case ElaNodeType.Placeholder:
                    //This is a valid situation, it means that the value is
                    //already on the top of the stack. Otherwise - nothing have to be done.
                    if (sysVar == -1)
                        cw.Emit(Op.Pop);
                    break;
                case ElaNodeType.RecordLiteral:
                    {
                        var n = (ElaRecordLiteral)exp;
                        CompileRecordPattern(sysVar, n, failLab, allowBang);
                    }
                    break;
                case ElaNodeType.TupleLiteral:
                    {
                        var n = (ElaTupleLiteral)exp;
                        CompileTuplePattern(sysVar, n, failLab, allowBang);
                    }
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        //An infix pattern, currently the only case is head/tail pattern.
                        var n = (ElaJuxtaposition)exp;
                        CompileComplexPattern(sysVar, n, failLab, allowBang);
                    }
                    break;                
                case ElaNodeType.ListLiteral:
                    {
                        var n = (ElaListLiteral)exp;

                        //We a have a nil pattern '[]'
                        if (!n.HasValues())
                        {
                            PushVar(sysVar);
                            cw.Emit(Op.Isnil);
                            cw.Emit(Op.Brfalse, failLab);
                        }
                        else
                        {
                            //We don't want to write the same compilation logic twice,
                            //so here we transform a list literal into a chain of function calls, e.g.
                            //[1,2,3] goes to 1::2::3::[] with a mandatory nil at the end.
                            var len = n.Values.Count;
                            ElaExpression last = ElaListLiteral.Empty;
                            var fc = default(ElaJuxtaposition);

                            //Loops through all elements in literal backwards
                            for (var i = 0; i < len; i++)
                            {
                                var nn = n.Values[len - i - 1];
                                fc = new ElaJuxtaposition();
                                fc.SetLinePragma(nn.Line, nn.Column);
                                fc.Parameters.Add(nn);
                                fc.Parameters.Add(last);
                                last = fc;
                            }

                            //Now we can compile it as head/tail pattern
                            CompilePattern(sysVar, fc, failLab, allowBang, false /*forceStrict*/);
                        }
                    }
                    break;
                default:
                    AddError(ElaCompilerError.InvalidPattern, exp, FormatNode(exp));
                    break;
            }
        }
        
        //Compile a record pattern in the form: {fieldName=pat,..}. Here we don't check the
        //type of an expression on the top of the stack - in a case if try to match a non-record
        //using this pattern the whole match would fail on Pushfld operation.
        private void CompileRecordPattern(int sysVar, ElaRecordLiteral rec, Label failLab, bool allowBang)
        {
            //Loops through all record fields
            for (var i = 0; i < rec.Fields.Count; i++)
            {
                var fld = rec.Fields[i];
                var addr = AddVariable();
                var si = AddString(fld.FieldName);

                PushVar(sysVar);
                cw.Emit(Op.Pushstr, si);
                cw.Emit(Op.Hasfld);
                cw.Emit(Op.Brfalse, failLab);

                PushVar(sysVar);
                cw.Emit(Op.Pushstr, si);
                cw.Emit(Op.Pushfld);
                PopVar(addr);

                //We obtain a value of field, now we need to match it using a pattern in
                //a field value (it could be a name reference or a non-irrefutable pattern).
                CompilePattern(addr, fld.FieldValue, failLab, allowBang, false /*forceStrict*/);
            }
        }

        //Compile a xs pattern in the form: {fieldName=pat,..}. This pattern can fail at
        //run-time if a given expression doesn't support Len and Pushelem op codes.
        private void CompileTuplePattern(int sysVar, ElaTupleLiteral tuple, Label failLab, bool allowBang)
        {
            var len = tuple.Parameters.Count;
            
            //Check the length first
            PushVar(sysVar);
            cw.Emit(Op.Len);            
            cw.Emit(Op.PushI4, len);
            cw.Emit(Op.Cneq);
            cw.Emit(Op.Brtrue, failLab); //Length not equal, proceed to fail

            //Loops through all xs patterns
            for (var i = 0; i < len; i++)
            {
                var pat = tuple.Parameters[i];
                PushVar(sysVar);
                
                //Generate a 'short' op typeId for the first entry
                if (i == 0)
                    cw.Emit(Op.PushI4_0);
                else
                    cw.Emit(Op.PushI4, i);

                cw.Emit(Op.Pushelem);

                //Here we need to bind a value of an element to a new system
                //variable in order to match it.
                var sysVar2 = AddVariable();
                PopVar(sysVar2);
                
                //Match an element of a xs
                CompilePattern(sysVar2, pat, failLab, allowBang, false /*forceStrict*/);
            }
        }

        //Currently this method only compiles head/tail pattern which is processed by parser
        //as function application. However it can be extended to support custom 'infix' patterns in future.
        private void CompileComplexPattern(int sysVar, ElaJuxtaposition call, Label failLab, bool allowBang)
        {
            if (call.Target == null)
                CompileHeadTail(sysVar, call, failLab, allowBang);
            else if (call.Target.Type == ElaNodeType.NameReference)
            {
                var targetName = call.Target.GetName();
                var sv = GetVariable(call.Target.GetName(), CurrentScope, GetFlags.NoError, call.Target.Line, call.Target.Column);

                //The head symbol corresponds to a constructor, this is a special case of pattern
                if ((sv.VariableFlags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin && (ElaBuiltinKind)sv.Data == ElaBuiltinKind.Cons)
                    CompileHeadTail(sysVar, call, failLab, allowBang);
                else
                    CompileConstructorPattern(sysVar, call, failLab, allowBang);
            }
            else if (call.Target.Type == ElaNodeType.FieldReference)
                CompileConstructorPattern(sysVar, call, failLab, allowBang);
            else
            {
                //We don't yet support other cases
                AddError(ElaCompilerError.InvalidPattern, call.Target, FormatNode(call.Target));
                return;
            }
        }

        //A generic case of constructor pattern
        private void CompileConstructorPattern(int sysVar, ElaJuxtaposition call, Label failLab, bool allowBang)
        {
            var n = String.Empty;
            PushVar(sysVar);
            
            //We have a qualified name here, in such case we don't just check
            //the presence of a constructor but ensure that this constructor originates
            //from a given module
            if (call.Target.Type == ElaNodeType.FieldReference)
            {
                var fr = (ElaFieldReference)call.Target;
                n = fr.FieldName;
                var alias = fr.TargetObject.GetName();

                if (fr.TargetObject.Type != ElaNodeType.NameReference)
                    AddError(ElaCompilerError.InvalidPattern, fr, FormatNode(fr));
                else
                    EmitSpecName(alias, "$$$$" + n, fr, ElaCompilerError.UndefinedName);
            }
            else
            {
                //Here we simply check that a constructor symbol is defined
                n = call.Target.GetName();
                EmitSpecName(null, "$$$$" + n, call.Target, ElaCompilerError.UndefinedName);
            }

            //This op codes skips one offset if an expression
            //on the top of the stack has a specified tag.
            cw.Emit(Op.Skiptag);
            cw.Emit(Op.Br, failLab); //We will skip this if tags are equal

            for (var i = 0; i < call.Parameters.Count; i++)
            {
                PushVar(sysVar);
                cw.Emit(Op.Untag, i); //Unwrap it
                                
                //Now we need to create a new system variable to hold
                //an unwrapped value.
                var sysVar2 = -1;
                var p = call.Parameters[i];
                
                //Don't do redundant bindings for simple patterns
                if (!IsSimplePattern(p))
                {
                    sysVar2 = AddVariable();
                    PopVar(sysVar2);
                }

                CompilePattern(sysVar2, p, failLab, allowBang, false /*forceStrict*/);
            }
        }

        //Compiles a special case of constructor pattern - head/tail pattern.
        private void CompileHeadTail(int sysVar, ElaJuxtaposition call, Label failLab, bool allowBang)
        {
            var fst = call.Parameters[0];
            var snd = call.Parameters[1];

            //Now check if a list is nil. If this is the case proceed to fail.
            PushVar(sysVar);
            cw.Emit(Op.Isnil);
            cw.Emit(Op.Brtrue, failLab);

            //Take a head of a list
            PushVar(sysVar);
            cw.Emit(Op.Head);
            var sysVar2 = -1;

            //For a case of a simple pattern we don't need to create to additional system
            //variable - these patterns are aware that they might accept -1 and it means that
            //the value is already on the top of the stack.
            if (!IsSimplePattern(fst))
            {
                sysVar2 = AddVariable();
                PopVar(sysVar2);
            }

            CompilePattern(sysVar2, fst, failLab, allowBang, false /*forceStrict*/);

            //Take a tail of a list
            PushVar(sysVar);
            cw.Emit(Op.Tail);
            sysVar2 = -1;

            //Again, don't do redundant bindings for simple patterns
            if (!IsSimplePattern(snd))
            {
                sysVar2 = AddVariable();
                PopVar(sysVar2);
            }

            CompilePattern(sysVar2, snd, failLab, allowBang, false /*forceStrict*/);
        }

        //Compiles a pattern as lazy by creating a thunk an initializing all pattern names with these thunks.
        private void CompileLazyPattern(int sys, ElaExpression pat, bool allowBang)
        {
            //First we need to obtain a list of all names, that declared in this pattern.
            var names = new List<String>();
            ExtractPatternNames(pat, names);

            //Walk through all names and create thunks
            for (var i = 0; i < names.Count; i++)
            {
                var n = names[i];
                Label funSkipLabel;
                int address;
                LabelMap newMap;

                CompileFunctionProlog(null, 1, pat.Line, pat.Column, out funSkipLabel, out address, out newMap);

                var next = cw.DefineLabel();
                var exit = cw.DefineLabel();

                //As soon as already compiling pattern as lazy, we should enforce strictness even if
                //pattern is declared as lazy. Otherwise everything would crash, because here assume
                //that names are bound in this bound in the same scope.
                CompilePattern(1 | ((sys >> 8) << 8), pat, next, allowBang, true /*forceStrict*/);
                cw.Emit(Op.Br, exit);
                cw.MarkLabel(next);
                cw.Emit(Op.Failwith, (Int32)ElaRuntimeError.MatchFailed);
                cw.MarkLabel(exit);
                cw.Emit(Op.Nop);

                //Here we expect that our target variable is already declared by a pattern in the
                //same physical scope - so we can obtain and push it as a return value.
                var sv = GetVariable(n, pat.Line, pat.Column);
                PushVar(sv);
                CompileFunctionEpilog(null, 1, address, funSkipLabel);
                cw.Emit(Op.Newlazy);

                ScopeVar var;
                if (CurrentScope.Locals.TryGetValue(n, out var))
                {
                    //If it doesn't have a NoInit flag we are not good
                    if ((var.Flags & ElaVariableFlags.NoInit) == ElaVariableFlags.NoInit)
                    {
                        PopVar(var.Address = 0 | var.Address << 8); //Aligning it to local scope
                        CurrentScope.RemoveFlags(n, ElaVariableFlags.NoInit);
                    }
                }
            }
        }

        //Builds a lists of all variables that are declared in the given pattern.
        private void ExtractPatternNames(ElaExpression pat, List<String> names)
        {
            switch (pat.Type)
            {
                case ElaNodeType.LazyLiteral:
                    {
                        //This whole method is used to extract names before making a
                        //pattern lazy, so we need to extract all names here as well,
                        //because this lazy literal will be compiled strict anyway (because
                        //the whole pattern is lazy already).
                        var vp = (ElaLazyLiteral)pat;
                        ExtractPatternNames(vp.Expression, names);
                    }
                    break;
                case ElaNodeType.UnitLiteral: //Idle
                    break;
                case ElaNodeType.As:
                    {
                        var asPat = (ElaAs)pat;
                        names.Add(asPat.Name);
                        ExtractPatternNames(asPat.Expression, names);
                    }
                    break;
                case ElaNodeType.Primitive: //Idle
                    break;
                case ElaNodeType.NameReference:
                    {
                        var vexp = (ElaNameReference)pat;

                        if (!vexp.Uppercase) //Uppercase is constructor
                            names.Add(vexp.Name);
                    }
                    break;
                case ElaNodeType.RecordLiteral:
                    {
                        var rexp = (ElaRecordLiteral)pat;

                        foreach (var e in rexp.Fields)
                            if (e.FieldValue != null)
                                ExtractPatternNames(e.FieldValue, names);
                    }
                    break;
                case ElaNodeType.TupleLiteral:
                    {
                        var texp = (ElaTupleLiteral)pat;

                        foreach (var e in texp.Parameters)
                            ExtractPatternNames(e, names);
                    }
                    break;
                case ElaNodeType.Placeholder: //Idle
                    break;
                case ElaNodeType.Juxtaposition:
                    {
                        var hexp = (ElaJuxtaposition)pat;

                        foreach (var e in hexp.Parameters)
                            ExtractPatternNames(e, names);
                    }
                    break;
                case ElaNodeType.ListLiteral: 
                    {
                        var l = (ElaListLiteral)pat;

                        if (l.HasValues())
                        {
                            foreach (var e in l.Values)
                                ExtractPatternNames(e, names);
                        }
                    }
                    break;
            }
        }

        //Tests if a given expression is a name reference of a placeholder (_).
        private bool IsSimplePattern(ElaExpression exp)
        {
            return exp.Type == ElaNodeType.NameReference || exp.Type == ElaNodeType.Placeholder;
        }

        //Adds a provided variable to the current scope or modifies an existing
        //variable with the same name (this is required because match variables unlike
        //regular variables in the same scope can shadow each other).
		private int AddMatchVariable(string varName, ElaExpression exp, out bool newV)
		{
            var sv = ScopeVar.Empty;
            newV = false;

            //Here we first check if such a name is already added and if this is the case simply fetch an
            //existing name.
            if (CurrentScope.Parent != null && CurrentScope.Parent.Locals.TryGetValue(varName, out sv)
                && (sv.Flags & ElaVariableFlags.Parameter) == ElaVariableFlags.Parameter)
                return 0 | sv.Address << 8;
            else
            {
                var res = CurrentScope.TryChangeVariable(varName);
                newV = true;

                if (res != -1)
                    return 0 | res << 8;

                return AddVariable(varName, exp, ElaVariableFlags.None, -1);
            }
		}

        //Performs validation of overlapping for simple case of pattern matching 
        //(such as 'match' expression with a single pattern per entry).
        private void ValidateOverlapSimple(IEnumerable<ElaEquation> seq, ElaExpression mexp)
        {
            var lst = new List<ElaExpression>();
            
            foreach (var e in seq)
            {
                foreach (var o in lst)
                    if (!e.Left.CanFollow(o))
                        AddWarning(ElaCompilerWarning.MatchEntryNotReachable, e.Left, FormatNode(e.Left), FormatNode(o));

                lst.Add(e.Left);
            }
        }

        //Performs validation of overlapping for complex case of pattern matchine
        //(such as when pattern matching is done in a function definition).
        private void ValidateOverlapComplex(IEnumerable<ElaEquation> seq)
        {
            var lst = new List<ElaJuxtaposition>();

            foreach (var ejx in seq)
            {
                var jx = (ElaJuxtaposition)ejx.Left;

                foreach (var ojx in lst)
                {
                    var can = false;

                    for (var i = 0; i < ojx.Parameters.Count; i++)
                    {
                        if (i < jx.Parameters.Count && jx.Parameters[i].CanFollow(ojx.Parameters[i]))
                        {
                            can = true;
                            break;
                        }
                    }

                    if (!can)
                        AddWarning(ElaCompilerWarning.MatchEntryNotReachable, jx, FormatNode(jx), FormatNode(ojx));
                }

                lst.Add(jx);
            }
        }
	}
}