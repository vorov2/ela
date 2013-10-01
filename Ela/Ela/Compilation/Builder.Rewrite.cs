using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Ela.Compilation
{
    //This part is responsible for compilation of top level equations. It does rewrite order of equations as well.
    internal sealed partial class Builder
    {
        //Main compilation method that runs compilation in seven steps by rewriting binding order.
        private void CompileProgram(ElaProgram prog, LabelMap map)
        {
            ProcessIncludesClassesTypes(prog, map);
            var list = RunForwardDeclaration(prog.TopLevel.Equations, map);
            list = ProcessFunctions(list, map);
            ProcessInstances(prog, map);
            list = ProcessBindings(list, map);
            ProcessExpressions(list, map);
        }

        //Compiles a provided set of equations. This method is to be used for local
        //scopes only.
        private void CompileEquationSet(ElaEquationSet set, LabelMap map)
        {
            var list = RunForwardDeclaration(set.Equations, map);
            list = ProcessFunctions(list, map);
            list = ProcessBindings(list, map);

            //Expressions are not allowed in this context
            if (list.Count > 0)
                for (var i = 0; i < list.Count; i++)
                    AddError(ElaCompilerError.InvalidExpression, list[i], FormatNode(list[i]));
        }

        //Type class declarations, modules includes and type declarations can be compiled in the first place.
        private void ProcessIncludesClassesTypes(ElaProgram prog, LabelMap map)
        {
            CompileModuleIncludes(prog, map);
            
            if (prog.Classes != null)
                CompileClass(prog.Classes, map);

            if (prog.Types != null)
                CompileTypes(prog.Types, map);
        }

        //This method declares all names from global bindings in advance (so that top level can be mutualy recursive).
        //It also compiles built-ins (all of them don't reference any names but instead do a lot of bindings that are 
        //used by the rest of the typeId).
        private FastList<ElaEquation> RunForwardDeclaration(List<ElaEquation> exps, LabelMap map)
        {
            var len = exps.Count;
            var list = new FastList<ElaEquation>(len);
            var head = default(ElaHeader);

            //We walk through all expressions and create a new list of expression that contains
            //only elements that are not compiled by this routine
            for (var i = 0; i < len; i++)
            {
                var b = exps[i];

                if (b.Right != null)
                {
                    //If a header is not null, we need to append some attributes to this binding
                    if (head != null)
                    {
                        //We add attributes if this binding is either a function or a name binding.
                        if (b.IsFunction() && b.GetFunctionName() == head.Name ||
                            b.Left.Type == ElaNodeType.NameReference && b.Left.GetName() == head.Name)
                            b.VariableFlags |= head.Attributes;
                        else
                            AddError(ElaCompilerError.HeaderNotConnected, head, FormatNode(head));

                        head = null;
                    }

                    if (AddNoInitVariable(b))
                    {
                        //This is a global binding that is initialized with a built-in. Or a 'safe'
                        //expression (no variable references). It is perfectly safe to compile it right away. 
                        if (b.Right.Type == ElaNodeType.Builtin || (!b.IsFunction() && b.Right.Safe()))
                            CompileDeclaration(b, map, Hints.Left);
                        else
                            list.Add(b);
                    }
                }
                else if (b.Left.Type == ElaNodeType.Header)
                {
                    //One header can't follow another another
                    if (head != null)
                        AddError(ElaCompilerError.HeaderNotConnected, head, FormatNode(head));

                    head = (ElaHeader)b.Left;
                }
                else
                {
                    //A header before an expression, that is not right
                    if (head != null)
                    {
                        AddError(ElaCompilerError.HeaderNotConnected, head, FormatNode(head));
                        head = null;
                    }

                    list.Add(b); //The rest will be compiled later
                }
            }

            return list;
        }

        //This step compiles instances - this should be done after types (as soon as instances
        //reference types) and after the first step as well (as soon as instances reference type classes
        //and can reference any other local and non-local names). It is important however to compile
        //instances before any user typeId gets executed because they effectively mutate function tables.
        private void ProcessInstances(ElaProgram prog, LabelMap map)
        {
            if (prog.Instances != null)
                CompileInstances(prog.Instances, map);
        }
            
        //Now we can compile global user defined functions and lazy sections. This is
        //user typeId however it is not executed when bindings are done therefore we wouldn't need to enforce
        //laziness here. TopLevel done through pattern matching are rejected on this stage.
        private FastList<ElaEquation> ProcessFunctions(FastList<ElaEquation> exps, LabelMap map)
        {
            var len = exps.Count;
            var list = new FastList<ElaEquation>(len);

            for (var i = 0; i < len; i++)
            {
                var b = exps[i];

                if (b.Right != null)
                {
                    //We need to ensure that this is a global binding that it is not defined by pattern matching
                    if (b.IsFunction() || (b.Right.Type == ElaNodeType.LazyLiteral && 
                        (b.Left.Type == ElaNodeType.NameReference || b.Left.Type == ElaNodeType.LazyLiteral)))
                        CompileDeclaration(b, map, Hints.None);
                    else
                        list.Add(b);
                }
                else
                    list.Add(b);
            }

            return list;
        }

        //This step is to compile all global bindings, including regular bindings and bindings defined
        //by pattern matching.
        private FastList<ElaEquation> ProcessBindings(FastList<ElaEquation> exps, LabelMap map)
        {
            var len = exps.Count;
            var small = len == 1;
            var list = new FastList<ElaEquation>(len);

            for (var i = 0; i < len; i++)
            {
                var b = exps[i];

                if (b.Right != null && b.Left.Type != ElaNodeType.Placeholder)
                {
                    var hints = Hints.Left;

                    //Unless somebody explicitly told us to compile everything in a
                    //strict manner, we need to check if we need to create some thunks here.
                    if (!options.Strict)
                    {                        
                        //Thunks are created if we have a single non-function binding which is recursive
                        //or when we have more than one binding, and we 'suspect' that this one cannot
                        //be executed in a strict manner (e.g. it references non-initialized names).
                        if ((small && IsRecursive(b)) || (!small && !CanCompileStrict(b.Right, null)))
                        {
#if DEBUG
                            Console.WriteLine("lazy:::" + FormatNode(b));
#endif
                            hints |= Hints.Lazy;
                        }
                    }

                    CompileDeclaration(b, map, hints);
                }
                else
                    list.Add(b);
            }

            return list;
        }
        
        //The last step is to compile let/in (local bindings) and the rest of expressions - we do not enforce
        //thunks here and in some cases execution of such typeId may result in 'BottomReached' run-time error.
        private void ProcessExpressions(FastList<ElaEquation> exps, LabelMap map)
        {
            var len = exps.Count;
            var expCount = 0;
            
            for (var i = 0; i < len; i++)
            {
                var e = exps[i];

                //This is a binding in the form '_ = exp' which we consider to be
                //just a hanging expression "without a warning".
                if (e.Left != null && e.Right != null)
                {
                    CompileExpression(e.Right, map, Hints.None, e);
                    cw.Emit(Op.Pop);
                }
                else
                {
                    var hints = i == len - 1 ? Hints.None : Hints.Left;

                    //Compile everything that is left
                    CompileExpression(e.Left, map, hints, e);
                    expCount++;
                }
            }

            //It may happens that nothing is left on this stage, however, Ela program have to return
            //something. Therefore just return unit.
            if (expCount == 0)
                cw.Emit(Op.Pushunit);
        }
        
        //Checks whether a given expression can be compiled in a strict manner. The test is
        //very conservative and may refuse to compile expressions as strict even if they can safely
        //executed in a strict manner.
        private bool CanCompileStrict(ElaExpression exp, List<String> locals)
        {
            if (exp == null)
                return true;

            switch (exp.Type)
            {
                case ElaNodeType.Comprehension:
                    {
                        var b = (ElaComprehension)exp;
                        return CanCompileStrict(b.Generator, locals);
                    }
                case ElaNodeType.Condition:
                    {
                        var b = (ElaCondition)exp;
                        return CanCompileStrict(b.Condition, locals) && 
                            CanCompileStrict(b.True, locals) && CanCompileStrict(b.False, locals);
                    }
                case ElaNodeType.LazyLiteral:
                    {
                        var b = (ElaLazyLiteral)exp;
                        return CanCompileStrict(b.Expression, locals);
                    }
                case ElaNodeType.Context:
                    {
                        var c = (ElaContext)exp;
                        return CanCompileStrict(c.Expression, locals) && (!c.Context.Parens || CanCompileStrict(c.Context, locals));
                    }
                case ElaNodeType.Equation:
                    return true;
                case ElaNodeType.FieldDeclaration:
                    {
                        var b = (ElaFieldDeclaration)exp;
                        return CanCompileStrict(b.FieldValue, locals);
                    }
                case ElaNodeType.FieldReference:
                    {
                        var b = (ElaFieldReference)exp;
                        return CanCompileStrict(b.TargetObject, locals);
                    }
                case ElaNodeType.Generator:
                    {
                        var g = (ElaGenerator)exp;
                        return CanCompileStrict(g.Target, locals) && 
                            CanCompileStrict(g.Guard, locals) && CanCompileStrict(g.Body, locals);
                    }
                case ElaNodeType.Juxtaposition:
                    {
                        //We are very conservative here. Basically we can't compile in a strict manner any application of a 
                        //non clean local function (except of a partial application).
                        var g = (ElaJuxtaposition)exp;

                        if (g.Target.Type == ElaNodeType.NameReference)
                        {
                            var n = g.Target.GetName();

                            //If a target is a 'local' name, we're ok with that. Local means
                            //that this whole expression is a part of 'let/in' or 'where' and we reference here
                            //a name declared by these constructs.
                            if (!IsLocal(locals, n))
                            {
                                var sv = GetVariable(n, CurrentScope, GetFlags.NoError, 0, 0);

                                if ((sv.Address & Byte.MaxValue) == 0 && //If it is not 0 than the call is non-local
                                    (sv.Flags & ElaVariableFlags.External) != ElaVariableFlags.External &&
                                    (sv.Flags & ElaVariableFlags.TypeFun) != ElaVariableFlags.TypeFun &&
                                    (sv.Flags & ElaVariableFlags.Builtin) != ElaVariableFlags.Builtin &&
                                    (sv.Flags & ElaVariableFlags.Parameter) != ElaVariableFlags.Parameter &&
                                    (sv.Flags & ElaVariableFlags.Clean) != ElaVariableFlags.Clean)
                                {
                                    //When we fall here, we can't compile the code in a strict manner, if this 
                                    //is not a function and not a partial application of a function.
                                    if ((sv.Flags & ElaVariableFlags.Function) != ElaVariableFlags.Function ||
                                        sv.Data <= g.Parameters.Count)
                                        return false;
                                }
                            }
                        }
                        else if (g.Target.Type == ElaNodeType.FieldReference)
                        {
                            //Here we check if this is a fully qualified function reference.
                            var fr = (ElaFieldReference)g.Target;

                            if (fr.TargetObject.Type != ElaNodeType.NameReference)
                                return false;

                            var sv = GetVariable(fr.TargetObject.GetName(), CurrentScope, GetFlags.NoError, 0, 0);

                            if ((sv.Flags & ElaVariableFlags.Module) != ElaVariableFlags.Module)
                                return false;
                        }
                        else
                            return false;

                        foreach (var p in g.Parameters)
                            if (!CanCompileStrict(p, locals))
                                return false;

                        return true;
                    }
                case ElaNodeType.LetBinding:
                    {
                        var g = (ElaLetBinding)exp;

                        if (locals == null)
                            locals = new List<String>();

                        //For a 'let' binding we need to extract all names declared in it.
                        //This will be our list of 'locals', which can be safely references in the
                        //expression, that comes right after 'in' clause. This is done to avoid
                        //generation of redundant thunks.
                        foreach (var e in g.Equations.Equations)
                        {
                            if (e.IsFunction())
                                locals.Add(e.GetFunctionName());
                            else
                                ExtractPatternNames(e, locals);
                        }

                        return CanCompileStrict(g.Expression, locals);
                    }
                case ElaNodeType.ListLiteral:
                    {
                        var b = (ElaListLiteral)exp;

                        if (!b.HasValues())
                            return true;

                        foreach (var v in b.Values)
                            if (!CanCompileStrict(v, locals))
                                return false;

                        return true;
                    }
                case ElaNodeType.Try:
                case ElaNodeType.Match:
                    {
                        var b = (ElaMatch)exp;

                        if (!CanCompileStrict(b.Expression, locals))
                            return false;

                        foreach (var e in b.Entries.Equations)
                            if (!CanCompileStrict(e.Right, locals))
                                return false;

                        return true;
                    }
                case ElaNodeType.NameReference:
                    {
                        var b = (ElaNameReference)exp;

                        //If a name is a 'local' name, we're ok with that. Local means
                        //that this whole expression is a part of 'let/in' or 'where' and we reference here
                        //a name declared by these constructs.                            
                        if (!IsLocal(locals, b.Name))
                        {
                            var sv = GetVariable(b.Name, CurrentScope, GetFlags.NoError | GetFlags.Local, 0, 0);
                            return (sv.Flags & ElaVariableFlags.NoInit) != ElaVariableFlags.NoInit;
                        }

                        return true;
                    }
                case ElaNodeType.Raise:
                    {
                        var r = (ElaRaise)exp;
                        return CanCompileStrict(r.Expression, locals);
                    }
                case ElaNodeType.Range:
                    {
                        var r = (ElaRange)exp;
                        return CanCompileStrict(r.First, locals) && CanCompileStrict(r.Second, locals) && CanCompileStrict(r.Last, locals);
                    }
                case ElaNodeType.RecordLiteral:
                    {
                        var r = (ElaRecordLiteral)exp;

                        foreach (var f in r.Fields)
                            if (!CanCompileStrict(f.FieldValue, locals))
                                return false;

                        return true;
                    }
                case ElaNodeType.TupleLiteral:
                    {
                        var t = (ElaTupleLiteral)exp;

                        foreach (var v in t.Parameters)
                            if (!CanCompileStrict(v, locals))
                                return false;

                        return true;
                    }
                default:
                    return true;
            }
        }

        //Checks if a given binding is a recursive binding, e.g. contains a reference to itself 
        //in the right side.
        private bool IsRecursive(ElaEquation eq)
        {
            //For a simple case we don't need to construct a name list and can do the 
            //things much easier.
            if (eq.Left.Type == ElaNodeType.NameReference && !((ElaNameReference)eq.Left).Uppercase)
            {
                var sv = GetVariable(eq.Left.GetName(), CurrentScope, GetFlags.NoError, 0, 0);
                return IsRecursive(eq.Right, sv.Address, null);
            }
            else
            {
                //Complex case, a binding contains a pattern in the left hand side. We then need to
                //extract all names declared in this pattern first, and the build a list of all addresses
                //that can be referenced by right hand side.
                var names = new List<String>();
                ExtractPatternNames(eq.Left, names);
                var arr = new int[names.Count];

                for (var i = 0; i < names.Count; i++)
                    arr[i] = GetVariable(names[i], CurrentScope, GetFlags.NoError, 0, 0).Address;

                if (IsRecursive(eq.Right, -1, arr))
                    return true;

                return false;
            }
        }

        //This method checks if an expression references any of the given addresses. It may accept
        //either a single address (addr) or an array of addresses.
        private bool IsRecursive(ElaExpression exp, int addr, int[] arr)
        {
            if (exp == null)
                return false;

            switch (exp.Type)
            {
                case ElaNodeType.Comprehension:
                    {
                        var b = (ElaComprehension)exp;
                        return IsRecursive(b.Generator, addr, arr);
                    }
                case ElaNodeType.LazyLiteral:
                    {
                        var b = (ElaLazyLiteral)exp;
                        return IsRecursive(b.Expression, addr, arr);
                    }
                case ElaNodeType.Condition:
                    {
                        var b = (ElaCondition)exp;
                        return IsRecursive(b.Condition, addr, arr) ||
                            IsRecursive(b.True, addr, arr) || IsRecursive(b.False, addr, arr);
                    }
                case ElaNodeType.Context:
                    {
                        var c = (ElaContext)exp;
                        return IsRecursive(c.Expression, addr, arr) || (!c.Context.Parens && IsRecursive(c.Context, addr, arr));
                    }
                case ElaNodeType.Equation:
                    return true;
                case ElaNodeType.FieldDeclaration:
                    {
                        var b = (ElaFieldDeclaration)exp;
                        return IsRecursive(b.FieldValue, addr, arr);
                    }
                case ElaNodeType.FieldReference:
                    {
                        var b = (ElaFieldReference)exp;
                        return IsRecursive(b.TargetObject, addr, arr);
                    }
                case ElaNodeType.Generator:
                    {
                        var g = (ElaGenerator)exp;
                        return IsRecursive(g.Target, addr, arr) || 
                            IsRecursive(g.Guard, addr, arr) || IsRecursive(g.Body, addr, arr);
                    }
                case ElaNodeType.Juxtaposition:
                    {
                        var g = (ElaJuxtaposition)exp;

                        if (IsRecursive(g.Target, addr, arr))
                            return true;

                        foreach (var p in g.Parameters)
                            if (IsRecursive(p, addr, arr))
                                return true;

                        return false;
                    }
                case ElaNodeType.LetBinding:
                    {
                        var g = (ElaLetBinding)exp;

                        foreach (var e in g.Equations.Equations)
                        {
                            if (!e.IsFunction() && IsRecursive(e.Right, addr, arr))
                                return true;
                        }

                        return IsRecursive(g.Expression, addr, arr);
                    }
                case ElaNodeType.ListLiteral:
                    {
                        var b = (ElaListLiteral)exp;

                        if (!b.HasValues())
                            return false;

                        foreach (var v in b.Values)
                            if (IsRecursive(v, addr, arr))
                                return true;

                        return false;
                    }
                case ElaNodeType.Try:
                case ElaNodeType.Match:
                    {
                        var b = (ElaMatch)exp;

                        if (IsRecursive(b.Expression, addr, arr))
                            return true;

                        foreach (var e in b.Entries.Equations)
                            if (IsRecursive(e.Right, addr, arr))
                                return true;

                        return false;
                    }
                case ElaNodeType.NameReference:
                    {
                        var b = (ElaNameReference)exp;
                        var sv = GetVariable(b.Name, CurrentScope, GetFlags.NoError | GetFlags.Local, 0, 0);

                        if (arr == null)
                            return sv.Address == addr;

                        for (var i = 0; i < arr.Length; i++)
                            if (arr[i] == sv.Address)
                                return true;

                        return false;
                    }
                case ElaNodeType.Raise:
                    {
                        var r = (ElaRaise)exp;
                        return IsRecursive(r.Expression, addr, arr);
                    }
                case ElaNodeType.Range:
                    {
                        var r = (ElaRange)exp;
                        return IsRecursive(r.First, addr, arr) || 
                            IsRecursive(r.Second, addr, arr) || IsRecursive(r.Last, addr, arr);
                    }
                case ElaNodeType.RecordLiteral:
                    {
                        var r = (ElaRecordLiteral)exp;

                        foreach (var f in r.Fields)
                            if (IsRecursive(f.FieldValue, addr, arr))
                                return true;

                        return false;
                    }
                case ElaNodeType.TupleLiteral:
                    {
                        var t = (ElaTupleLiteral)exp;

                        foreach (var v in t.Parameters)
                            if (IsRecursive(v, addr, arr))
                                return true;

                        return false;
                    }
                default:
                    return false;
            }
        }

        //Checks if a given name is present in the list of local names.
        //List may be null.
        private bool IsLocal(List<String> names, string n)
        {
            if (names == null)
                return false;

            for (var i = 0; i < names.Count; i++)
                if (names[i] == n)
                    return true;

            return false;
        }


        #region Disabled staff
        //**********************************TEMPORARY DISABLED
        //This method checks if a given expression is a regular function definition or a function
        //defined through partial application of another function.
        private bool IsFunction(ElaEquation eq)
        {
            //A simple case - regular function definition
            if (eq.IsFunction())
                return true;

            //This may be a function defined through partial application. Here we only
            //recognize two cases - when the head is an ordinary identifier and if a head is
            //a fully qualified name.
            if (eq.Right != null && eq.Right.Type == ElaNodeType.Juxtaposition)
            {
                var jx = (ElaJuxtaposition)eq.Right;

                //A head is an identifier
                if (jx.Target.Type == ElaNodeType.NameReference)
                {
                    var sv = GetVariable(jx.Target.GetName(), CurrentScope, GetFlags.NoError, 0, 0);
                    
                    //This is a partially applied function, therefore we can "count" this as a function.
                    return IfFunction(eq, sv, jx.Parameters.Count, false);
                }
                else if (jx.Target.Type == ElaNodeType.FieldReference) //This might be a fully qualified name
                {
                    var tr = (ElaFieldReference)jx.Target;

                    //A target can only be a name reference; otherwise we don't see it as a function.
                    if (tr.TargetObject.Type == ElaNodeType.NameReference)
                    {
                        CodeFrame _;
                        var sv = FindByPrefix(tr.TargetObject.GetName(), tr.FieldName, out _);

                        return IfFunction(eq, sv, jx.Parameters.Count, false);
                    }
                }
            }
            else if (eq.Right != null && eq.Right.Type == ElaNodeType.NameReference)
            {
                //This may be a function defined as an alias for another function.
                var sv = GetVariable(eq.Right.GetName(), CurrentScope, GetFlags.NoError, 0, 0);

                //This is an alias, we can "count" this as a function.
                return IfFunction(eq, sv, 0, true);
            }

            return false;
        }

        //**********************************TEMPORARY DISABLED
        //Here we try to check if a right side is a function reference of a partially applied function.
        //This function might (or might not) set a 'PartiallyApplied' flag. If this flag is set, than a
        //function is eta expanded during compilation. Normally this flag is not needed when functions
        //are defined as simple aliases for other functions.
        private bool IfFunction(ElaEquation eq, ScopeVar sv, int curArgs, bool noPartial)
        {
            //This is a partially applied function, therefore we can "count" this as a function.
            if ((sv.Flags & ElaVariableFlags.Function) == ElaVariableFlags.Function && sv.Data > curArgs)
            {
                if (!noPartial)
                {
                    eq.VariableFlags |= ElaVariableFlags.PartiallyApplied;
                    eq.Arguments = sv.Data - curArgs;
                }

                return true;
            }
            else if ((sv.Flags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin)
            {
                //If this a built-in, we need to use another method to determine number of arguments.
                var args = BuiltinParams((ElaBuiltinKind)sv.Data);

                if (args > curArgs)
                {
                    if (!noPartial)
                    {
                        eq.VariableFlags |= ElaVariableFlags.PartiallyApplied;
                        eq.Arguments = args - curArgs;
                    }

                    return true;
                }
            }

            return false;
        }
        #endregion
    }
}
