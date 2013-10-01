using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class ModuleInstance : Class
    {
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return value.ToString();
        }

        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.MOD)
            {
                NoOverloadBinary(TCF.MOD, right, "equal", ctx);
                return false;
            }

            return ((ElaModule)left.Ref).Handle == ((ElaModule)right.Ref).Handle;
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.MOD)
            {
                NoOverloadBinary(TCF.MOD, right, "notequal", ctx);
                return false;
            }

            return ((ElaModule)left.Ref).Handle != ((ElaModule)right.Ref).Handle;
        }

        internal override ElaValue GetField(ElaValue obj, ElaValue field, ExecutionContext ctx)
        {
            return ((ElaModule)obj.Ref).GetField(field, ctx);
        }

        internal override bool HasField(ElaValue obj, ElaValue field, ExecutionContext ctx)
        {
            return ((ElaModule)obj.Ref).HasField(field, ctx);
        }
    }
}
