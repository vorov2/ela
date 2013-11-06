using System;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaLong : ElaObject
	{
		public ElaLong(long value) : base(ElaTypeCode.Long)
		{
			Value = value;
		}
		
        internal override long AsLong()
        {
            return Value;
        }
        
        public override string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

        internal override ElaValue Quot(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                {
                    if (right.I4 == 0)
                    {
                        ctx.DivideByZero(left);
                        return Default();
                    }

                    return new ElaValue(left.Ref.AsLong() / right.I4);
                }
                else
                {
                    ctx.NoOverload(left.GetTypeName() + "->" + left.GetTypeName() + "->*", left.GetTypeName() + "->" + right.GetTypeName() + "->*", "quot");
                    return Default();
                }
            }

            var r = right.Ref.AsLong();

            if (r == 0)
            {
                ctx.DivideByZero(left);
                return Default();
            }

            return new ElaValue(left.Ref.AsLong() / right.Ref.AsLong());
        }

		public long Value { get; private set; }
	}
}