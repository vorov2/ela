using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class StringInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return "\"" + value.ToString() + "\"";
        }

        internal override ElaValue Concatenate(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            right = right.Ref.Force(right, ctx);

            if (right.TypeId != ElaMachine.STR)
            {
                if (right.TypeId == ElaMachine.CHR)
                    return new ElaValue(left.DirectGetString() + right.ToString());

                NoOverloadBinary(TCF.STRING, right, "concatenate", ctx);
                return Default();
            }

            return new ElaValue(left.DirectGetString() + right.DirectGetString());
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "equal", ctx);
                return false;
            }

            return left.DirectGetString() == right.DirectGetString();
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "notequal", ctx);
                return false;
            }

            return left.DirectGetString() != right.DirectGetString();
        }

        internal override ElaValue GetLength(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(((ElaString)value.Ref).Value.Length);
        }

        internal override ElaValue GetValue(ElaValue value, ElaValue index, ExecutionContext ctx)
        {
            return ((ElaString)value.Ref).GetValue(index, ctx);
        }

        internal override bool Greater(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "greater", ctx);
                return false;
            }

            return left.DirectGetString().CompareTo(right.DirectGetString()) < 0;
        }

        internal override bool Lesser(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "lesser", ctx);
                return false;
            }

            return left.DirectGetString().CompareTo(right.DirectGetString()) > 0;
        }

        internal override bool GreaterEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "greaterequal", ctx);
                return false;
            }

            return left.DirectGetString().CompareTo(right.DirectGetString()) <= 0;
        }

        internal override bool LesserEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.STR)
            {
                NoOverloadBinary(TCF.STRING, right, "lesserequal", ctx);
                return false;
            }

            return left.DirectGetString().CompareTo(right.DirectGetString()) >= 0;
        }

        internal override ElaValue Head(ElaValue left, ExecutionContext ctx)
        {
            return new ElaValue(left.DirectGetString()[0]);
        }

        internal override ElaValue Tail(ElaValue left, ExecutionContext ctx)
        {
            return left.Ref.Tail(ctx);
        }

        internal override bool IsNil(ElaValue left, ExecutionContext ctx)
        {
            return left.DirectGetString().Length == 0;
        }
    }
}
