using System;

namespace Ela.Runtime.ObjectModel
{
	internal sealed class ElaBoolean : ElaObject
	{
		internal static readonly ElaBoolean Instance = new ElaBoolean();
        
		private ElaBoolean() : base(ElaTypeCode.Boolean)
		{

		}

        internal override bool True(ElaValue @this, ExecutionContext ctx)
        {
            return @this.I4 == 1;
        }

        internal override bool False(ElaValue @this, ExecutionContext ctx)
        {
            return @this.I4 == 0;
        }
	}
}