using System;
using System.Collections.Generic;
using System.Text;

namespace Ela.Debug
{
	internal sealed class SymGenerator
	{
		#region Construction
		private const string LINE_FORMAT = "Offset: {0}\t->\tLine: {1}, Col:{2}\r\n";
		private const string VAR_FORMAT = "Offset: {0}\t->\tAddress: {1}, Scope:{2}, Name:{3}\r\n";
		private const string SCOPE_FORMAT = "Offset: {0}\t->\tEnd Offset: {1}, Index:{2}, Parent:{3}\r\n";
		private const string FUN_FORMAT = "Offset: {0}\t->\tEnd Offset: {1}, Name:{2}, Index:{3}\r\n";

		internal SymGenerator(DebugInfo syms)
		{
			Symbols = syms;
		}
		#endregion


		#region Methods
		internal string Generate(SymTables tables)
		{
			var sb = new StringBuilder();

			if ((tables & SymTables.Lines) == SymTables.Lines)
			{
				for (var i = 0; i < Symbols.Lines.Count; i++)
				{
					var l = Symbols.Lines[i];
					sb.AppendFormat(LINE_FORMAT, l.Offset, l.Line, l.Column);
				}
			}

			if ((tables & SymTables.Scopes) == SymTables.Scopes)
			{
				for (var i = 0; i < Symbols.Scopes.Count; i++)
				{
					var s = Symbols.Scopes[i];
					sb.AppendFormat(SCOPE_FORMAT, s.StartOffset, s.EndOffset, s.Index, s.ParentIndex);
				}
			}

			if ((tables & SymTables.Vars) == SymTables.Vars)
			{
				for (var i = 0; i < Symbols.Vars.Count; i++)
				{
					var v = Symbols.Vars[i];
					sb.AppendFormat(VAR_FORMAT, v.Offset, v.Address, v.Scope, v.Name);
				}
			}

			if ((tables & SymTables.Functions) == SymTables.Functions)
			{
				for (var i = 0; i < Symbols.Functions.Count; i++)
				{
					var f = Symbols.Functions[i];
					sb.AppendFormat(FUN_FORMAT, f.StartOffset, f.EndOffset, f.Name ?? String.Empty, f.Handle);
				}
			}

			return sb.ToString();
		}
		#endregion


		#region Properties
		internal DebugInfo Symbols { get; private set; }
		#endregion
	}
}
