using System;

namespace Ela.Debug
{
	public sealed class LineSym
	{
		#region Construction
		internal LineSym(int offset, int line, int column)
		{
			Offset = offset;
			Line = line;
			Column = column;
		}
		#endregion


		#region Properties
		public int Offset { get; private set; }

		public int Line { get; private set; }

		public int Column { get; private set; }
		#endregion
	}
}
