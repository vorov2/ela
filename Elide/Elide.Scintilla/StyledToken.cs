using System;

namespace Elide.Scintilla
{
	public struct StyledToken
	{
		public StyledToken(int position, int length, TextStyle styleKey)
		{
			Position = position;
			Length = length;
			StyleKey = styleKey;
		}
	
		public readonly int Position;

		public readonly int Length;

		public readonly TextStyle StyleKey;
	}
}
