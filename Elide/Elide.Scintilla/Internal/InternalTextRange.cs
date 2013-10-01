using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct InternalTextRange
	{
		public InternalCharRange Range;

		public IntPtr Text;
	}
}
