using System;

namespace Ela.Runtime.ObjectModel
{
	internal sealed class ElaInteger : ElaObject
	{
		internal static readonly ElaInteger Instance = new ElaInteger();
		
		private ElaInteger() : base(ElaTypeCode.Integer)
		{
			
		}

        internal override ElaValue Quot(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.INT)
            {
                if (right.TypeId == ElaMachine.LNG)
                {
                    var r = right.Ref.AsLong();

                    if (r == 0)
                    {
                        ctx.DivideByZero(left);
                        return Default();
                    }

                    return new ElaValue(left.I4 / r);
                }
                else
                {
                    ctx.NoOverload(left.GetTypeName() + "->" + left.GetTypeName() + "->*", left.GetTypeName() + "->" + right.GetTypeName() + "->*", "quot");
                    return Default();
                }
            }

            if (right.I4 == 0)
            {
                ctx.DivideByZero(left);
                return Default();
            }

            return new ElaValue(left.I4 / right.I4);
        }
	}
}