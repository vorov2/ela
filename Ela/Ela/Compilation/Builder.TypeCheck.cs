using System;
using Ela.CodeModel;
using Ela.Runtime;

namespace Ela.Compilation
{
    //This part is responsible for omitting type check/class check related code.
    internal sealed partial class Builder
    {
        //Performs a type check and throws and error if the type is not as expected.
        //This method assumes that a value to be checked is on the top of the stack.
        private ScopeVar TypeCheckConstructor(string cons, string prefix, string name, ElaExpression par, bool force)
        {
            var sv = EmitSpecName(prefix, "$$" + name, par, ElaCompilerError.UndefinedType);
            TypeCheckConstructorAllStack(cons, force);
            return sv;
        }

        //This method is similar to TypeCheckConstructor, but assumes that everything is on stack
        private void TypeCheckConstructorAllStack(string cons, bool force)
        {
            if (force)
                cw.Emit(Op.Force);

            var succ = cw.DefineLabel();
            cw.Emit(Op.Ctype);
            cw.Emit(Op.Brtrue, succ);
            cw.Emit(Op.Pushstr, AddString(String.Format("Type constraint is violated for a constructor '{0}'.", cons)));
            cw.Emit(Op.Throw);
            cw.MarkLabel(succ);
        }

        //Compiles Ela 'is' expression.
        private void CompileTypeCheck(ElaTypeCheck n, LabelMap map, Hints hints)
        {
            var failLab = cw.DefineLabel();
            var endLab = cw.DefineLabel();

            var sysVar = AddVariable();
            CompileExpression(n.Expression, map, Hints.None, n);
            PopVar(sysVar);

            //Here we are checking all classes specified in a pattern. We have to loop
            //through all classes and generate a check instruction (Traitch) for each.
            for (var i = 0; i < n.Traits.Count; i++)
            {
                var t = n.Traits[i];
                PushVar(sysVar);
                CheckTypeOrClass(t.Prefix, t.Name, failLab, n);
            }

            cw.Emit(Op.PushI1_1);
            cw.Emit(Op.Br, endLab);
            cw.MarkLabel(failLab);
            cw.Emit(Op.PushI1_0);
            cw.MarkLabel(endLab);
            cw.Emit(Op.Nop);

            if ((hints & Hints.Left) == Hints.Left)
                AddValueNotUsed(n);
        }

        //This method check if a given name (qualified or not, prefix may be null) is a type or class
        //and emit an appropriate code to check it.
        //This method assumes that a value to check is already on the top of the stack.
        private void CheckTypeOrClass(string prefix, string name, Label failLab, ElaExpression exp)
        {
            //This item can either check a specific type or a trait.
            var isType = NameExists(prefix, "$$" + name); //A type with such a name exists
            var isClass = NameExists(prefix, "$$$" + name); //A class with such a name exists

            //OK, this is ambiguity, better to report about that. We will consider a symbol
            //to be a type and compile further.
            if (isType && isClass)
            {
                AddWarning(ElaCompilerWarning.TypeClassAmbiguity, exp, prefix == null ? name : prefix + "." + name, FormatNode(exp));

                //This hint suggests to use prefix, it is stupid to generate it, if we have a prefix already.
                if (prefix == null)
                    AddHint(ElaCompilerHint.TypeClassAmbiguity, exp);
            }

            cw.Emit(Op.Force);                        

            if (isType)
            {
                EmitSpecName(prefix, "$$" + name, exp, ElaCompilerError.UndefinedType);
                cw.Emit(Op.Ctype);
            }
            else
            {
                EmitSpecName(prefix, "$$$" + name, exp, ElaCompilerError.UnknownClass);
                cw.Emit(Op.Traitch);
            }

            cw.Emit(Op.Brfalse, failLab);
        }
    }
}
