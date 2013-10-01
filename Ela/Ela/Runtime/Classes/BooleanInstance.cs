using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class BooleanInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return value.ToString();
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.BYT)
            {
                NoOverloadBinary(TCF.BOOL, right, "equal", ctx);
                return false;
            }

            return left.I4 == right.I4;
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.BYT)
            {
                NoOverloadBinary(TCF.BOOL, right, "notequal", ctx);
                return false;
            }

            return left.I4 != right.I4;
        }
    }
}
