using System;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaUnit : ElaObject
	{
		public static readonly ElaUnit Instance = new ElaUnit();

		private ElaUnit() : base(ElaTypeCode.Unit)
		{

		}

        public override string ToString(string format, IFormatProvider provider)
        {
			return "()";
		}
	}
}
