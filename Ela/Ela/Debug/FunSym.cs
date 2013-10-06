using System;

namespace Ela.Debug
{
	public sealed class FunSym
	{
		#region Construction
		internal FunSym(string name, int offset, int pars)
		{
			Name = name;
			StartOffset = offset;
			Parameters = pars;
		}
		#endregion


		#region Properties
		public string Name { get; private set; }

		public int Parameters { get; private set; }
		
		public int Handle { get; internal set; }

		public int StartOffset { get; private set; }

		public int EndOffset { get; internal set; }
		#endregion
	}
}
