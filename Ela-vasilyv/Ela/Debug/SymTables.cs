using System;

namespace Ela.Debug
{
	[Flags]
	public enum SymTables
	{
		None,

		Lines = 0x02,

		Scopes = 0x04,

		Vars = 0x08,

		Functions = 0x10
	}
}
