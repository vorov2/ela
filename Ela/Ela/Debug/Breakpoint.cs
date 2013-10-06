using System;

namespace Ela.Debug
{
	public sealed class Breakpoint
	{
		#region Construction
		public Breakpoint(int line)
		{
			Line = line;
		}
		#endregion


		#region Properties
		public int Line { get; private set; }

		public int MaxHitCount { get; set; }

		internal int HitCount { get; set; }

		internal int? Offset { get; set; }
		#endregion
	}
}