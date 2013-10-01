using System;

namespace Ela.Compilation
{
	internal enum DataKind
	{
		None = 0,

		FunParams = 1,

		FunCurry = 2,

		VarType = 3,

		Builtin = 4,

        FastCall = 5,
	}
}
