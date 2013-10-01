using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
	internal static class NativeMethods
	{
        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, byte[] lParam);
        
        [DllImport("kernel32")]
		internal static extern IntPtr LoadLibrary(string libraryName);

		[DllImport("kernel32.dll")]
		internal static extern bool FreeLibrary(IntPtr handle);

        [DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, uint Msg, int wParam, uint lParam);

        [DllImport("user32.dll")]
		internal static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, [MarshalAs(UnmanagedType.LPStr)] string lParam);
	}
}
