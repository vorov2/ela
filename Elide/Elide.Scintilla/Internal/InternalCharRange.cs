using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct InternalCharRange
	{
		public int Min;

		public int Max;
	}
}
