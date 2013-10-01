using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class FunctionInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return value.ToString();
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.FUN)
            {
                NoOverloadBinary(TCF.FUN, right, "equal", ctx);
                return false;
            }

            var f1 = (ElaFunction)left.Ref;
            var f2 = (ElaFunction)left.Ref;
            return f1 == f2 || (f1.Handle == f2.Handle && f1.ModuleHandle == f2.ModuleHandle && 
                f1.AppliedParameters == f2.AppliedParameters && f1.AppliedParameters == 0);
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.FUN)
            {
                NoOverloadBinary(TCF.FUN, right, "notequal", ctx);
                return false;
            }

            var f1 = (ElaFunction)left.Ref;
            var f2 = (ElaFunction)left.Ref;
            return f1.Handle != f2.Handle || f1.ModuleHandle != f2.ModuleHandle || 
                f1.AppliedParameters != f2.AppliedParameters || f1.AppliedParameters != 0;
        }
    }
}
