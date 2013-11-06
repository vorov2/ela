using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class CharInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return "'" + value.ToString() + "'";
        }

        internal override ElaValue Concatenate(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            right = right.Ref.Force(right, ctx);
            
            if (right.TypeId != ElaMachine.CHR)
            {
                if (right.TypeId == ElaMachine.STR)
                    return new ElaValue(left.ToString() + right.DirectGetString());

                NoOverloadBinary(TCF.CHAR, right, "concatenate", ctx);
                return Default();
            }

            return new ElaValue(((Char)left.I4).ToString() + (Char)right.I4);
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "equal", ctx);
                return false;
            }

            return left.I4 == right.I4;
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "notequal", ctx);
                return false;
            }

            return left.I4 != right.I4;
        }

        internal override bool Greater(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "greater", ctx);
                return false;
            }

            return left.I4 > right.I4;
        }

        internal override bool Lesser(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "lesser", ctx);
                return false;
            }

            return left.I4 < right.I4;
        }

        internal override bool GreaterEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "greaterequal", ctx);
                return false;
            }

            return left.I4 >= right.I4;
        }

        internal override bool LesserEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.CHR)
            {
                NoOverloadBinary(TCF.CHAR, right, "lesserequal", ctx);
                return false;
            }

            return left.I4 <= right.I4;
        }
    }
}
