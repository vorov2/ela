using System;
using Elide.Core;
using Elide.CodeEditor;

namespace Elide.CodeEditor
{
    public sealed class SymbolLocation
    {
        public SymbolLocation(CodeDocument doc, int line, int column)
        {
            Document = doc;
            Line = line - 1;
            Column = column - 1;
        }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public CodeDocument Document { get; private set; }
    }
}
