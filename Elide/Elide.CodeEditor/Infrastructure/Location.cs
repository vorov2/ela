using System;

namespace Elide.CodeEditor.Infrastructure
{
    public struct Location
    {
        public readonly int Line;
        public readonly int Column;

        public Location(int line, int column)
        {
            Line = line;
            Column = column;
        }
    }
}
