using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct NotifyHeader
	{
		public IntPtr HWndFrom;
		
		public uint IdFrom;
		
		public uint Code;
	}
}
