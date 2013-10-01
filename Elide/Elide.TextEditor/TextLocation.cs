using System;

namespace Elide.TextEditor
{
    public struct TextLocation
    {
        public readonly int Line;
        public readonly int Column;

        public TextLocation(int line, int col)
        {
            Line = line;
            Column = col;
        }
    }
}
