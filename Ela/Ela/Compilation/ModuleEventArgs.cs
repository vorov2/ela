using System;

namespace Ela.Compilation
{
	public sealed class ModuleEventArgs : EventArgs
	{
		#region Construction
		public ModuleEventArgs(ModuleReference mod)
		{
			Module = mod;
		}
		#endregion


		#region Properties
		public ModuleReference Module { get; private set; }

        internal CodeFrame Frame { get; set; }
		#endregion
	}
}
