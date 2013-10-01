using System;

namespace Ela.Compilation
{
	internal struct ExprData
	{
		internal static readonly ExprData Empty = new ExprData(DataKind.None, 0);

		internal ExprData(DataKind type, int data)
		{
			Type = type;
			Data = data;
		}

		internal readonly DataKind Type;

		internal readonly int Data;
	}
}
