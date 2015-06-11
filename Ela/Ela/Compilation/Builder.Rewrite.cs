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
                        //Thunks are created if we have a binding which is recursive or when we
                        //'suspect' that this one cannot be executed in a strict manner (e.g.
                        //it references non-initialized names).

                        if (ShouldCompileLazy(b.Right))
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

        //Test if a right hand side contains any NoInt names (or a custom numeric literal) and if yes - make it lazy.
        private bool ShouldCompileLazy(ElaExpression exp)
        {
            foreach (var n in exp.ExtractNames())
                if (n == "$literal" || (GetVariable(n, CurrentScope, GetFlags.NoError, 0, 0).Flags & ElaVariableFlags.NoInit) == ElaVariableFlags.NoInit)
                    return true;

            return false;
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
                    
                    if (hints != Hints.Left)
                        expCount++;
                }
            }

            //It may happens that nothing is left on this stage, however, Ela program have to return
            //something. Therefore just return unit.
            if (expCount == 0)
                cw.Emit(Op.Pushunit);
        }
    }
}
