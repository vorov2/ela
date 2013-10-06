using System;
using System.IO;

namespace Ela.Debug
{
	public sealed class DebugInfo
	{
		#region Construction
		internal DebugInfo()
		{
			Scopes = new FastList<ScopeSym>();
			Lines = new FastList<LineSym>();
			Vars = new FastList<VarSym>();
			Functions = new FastList<FunSym>();
		}
		#endregion


		#region Methods
		internal DebugInfo Merge(DebugInfo syms)
		{
			Scopes.AddRange(syms.Scopes);
			Lines.AddRange(syms.Lines);
			Vars.AddRange(syms.Vars);
			Functions.AddRange(syms.Functions);
			return this;
		}


		internal DebugInfo Clone()
		{
			var di = new DebugInfo();
			di.Scopes = Scopes.Clone();
			di.Lines = Lines.Clone();
			di.Vars = Vars.Clone();
			di.Functions = Functions.Clone();
			di.File = File;
			return di;
		}
		#endregion


		#region Properties
		internal FileInfo File { get; set; }

		internal FastList<ScopeSym> Scopes { get; private set; }

		internal FastList<LineSym> Lines { get; private set; }

		internal FastList<VarSym> Vars { get; private set; }
		
		internal FastList<FunSym> Functions { get; private set; }
		#endregion
	}
}