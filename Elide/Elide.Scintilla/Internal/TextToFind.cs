using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct TextToFind
    {
        public InternalCharRange chrg;
        public IntPtr lpstrText;
        public InternalCharRange chrgText;
    }
}
