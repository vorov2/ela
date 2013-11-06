using System;

namespace Ela.Compilation
{
	internal sealed class MemoryLayout
	{
		#region Construction
		internal MemoryLayout(int size, int stackSize, int address)
		{
			Size = size;
			StackSize = stackSize;
			Address = address;
		}
		#endregion


		#region Properties
		internal int StackSize { get; set; }

		internal int Size { get; set; }

		internal int Address { get; private set; }
		#endregion
	}
}