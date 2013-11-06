using System;

namespace Ela.Runtime.ObjectModel
{
	internal sealed class ElaChar : ElaObject
	{
		internal static readonly ElaChar Instance = new ElaChar();
        
		private ElaChar() : base(ElaTypeCode.Char)
		{

		}
	}
}