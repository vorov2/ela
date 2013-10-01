using System;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaDouble : ElaObject
	{
		public ElaDouble(double value) : base(ElaTypeCode.Double)
		{
			Value = value;
		}
		
        internal override double AsDouble()
        {
            return Value;
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            return Value.ToString(format, provider);
        }

		public double Value { get; private set; }
	}
}