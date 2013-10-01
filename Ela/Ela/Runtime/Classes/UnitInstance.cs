using System;

namespace Ela.Runtime.Classes
{
    internal sealed class UnitInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return value.ToString();
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.UNI)
            {
                NoOverloadBinary(TCF.UNIT, right, "equal", ctx);
                return false;
            }

            return left.Ref == right.Ref;
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.UNI)
            {
                NoOverloadBinary(TCF.UNIT, right, "notequal", ctx);
                return false;
            }

            return left.Ref == right.Ref;
        }
    }
}
