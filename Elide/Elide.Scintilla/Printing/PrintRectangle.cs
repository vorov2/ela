using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Printing
{
    [StructLayout(LayoutKind.Sequential)]
    internal struct PrintRectangle
    {
        public readonly int Left;
        public readonly int Top;
        public readonly int Right;
        public readonly int Bottom;

        public PrintRectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }
}
