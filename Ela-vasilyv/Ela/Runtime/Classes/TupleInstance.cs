using System;
using Ela.CodeModel;
using Ela.Parsing;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class TupleInstance : Class
    {
        internal override ElaValue Concatenate(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            left = left.Ref.Force(left, ctx);
            right = right.Ref.Force(right, ctx);

            if (right.TypeId != ElaMachine.TUP)
            {
                NoOverloadBinary(TCF.TUPLE, right, "concatenate", ctx);
                return Default();
            }

            return new ElaValue(ElaTuple.Concat((ElaTuple)left.Ref, (ElaTuple)right.Ref));
        }

        internal override ElaValue GetLength(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(((ElaTuple)value.Ref).Length);
        }

        internal override ElaValue GetValue(ElaValue value, ElaValue key, ExecutionContext ctx)
        {
            if (key.TypeId != ElaMachine.INT)
            {
                ctx.InvalidIndexType(key);
                return Default();
            }

            var tup = (ElaTuple)value.Ref;

            if (key.I4 < tup.Length && key.I4 > -1)
                return tup.Values[key.I4];

            ctx.IndexOutOfRange(key, value);
            return Default();
        }
    }
}
