using System;
using Ela.CodeModel;
using Ela.Parsing;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class ListInstance : Class
    {
        internal override ElaValue Head(ElaValue left, ExecutionContext ctx)
        {
            return ((ElaList)left.Ref).InternalValue;
        }

        internal override ElaValue Tail(ElaValue left, ExecutionContext ctx)
        {
            return left.Ref.Tail(ctx);
        }

        internal override bool IsNil(ElaValue left, ExecutionContext ctx)
        {
            return left.Ref == ElaList.Empty;
        }
    }
}
