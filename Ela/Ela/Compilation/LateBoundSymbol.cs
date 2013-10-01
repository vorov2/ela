using System;
using Ela.CodeModel;
using Ela.Debug;

namespace Ela.Compilation
{
	public struct LateBoundSymbol
	{
        public readonly string Name;
        public readonly int Address;
        public readonly int Data;
        public readonly int Flags;
        public readonly int Line;
        public readonly int Column;

		internal LateBoundSymbol(string name, int address, int data, int flags, int line, int col)
		{
			Name = name;
			Address = address;
			Data = data;
            Flags = flags;
			Line = line;
			Column = col;
		}
	}
}
