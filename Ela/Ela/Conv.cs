 using System;
using System.Runtime.InteropServices;

namespace Ela
{
	[StructLayout(LayoutKind.Explicit)]
	internal struct Conv
	{
		[FieldOffset(0)]
		internal long I8;
		
		[FieldOffset(0)]
		internal double R8;
		
		[FieldOffset(0)]
		internal float R4;
		
		[FieldOffset(0)]
		internal int I4_1;
		
		[FieldOffset(4)]
		internal int I4_2;
	}
}