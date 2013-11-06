using System;

namespace Ela.Compilation
{
	public enum ElaCompilerHint
	{
		None = 0,


        TypeClassAmbiguity = 500,

		UseIgnoreToPop = 501,

		MatchEntryNotReachable = 502,

        AddElse = 503,

        AddSpaceSection = 504,

        AddSpaceApplication = 505,

        BangsOnlyFunctions = 506,
	}
}
