using System;
using System.Runtime.InteropServices;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.Printing
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct RangeToFormat
    {
        public IntPtr hdc;
        public IntPtr hdcTarget;
        public PrintRectangle rc;
        public PrintRectangle rcPage;
        public InternalCharRange chrg;
    }
}
