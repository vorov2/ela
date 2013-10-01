using System;
using Elide.Environment;

namespace Elide.TextEditor
{
    public sealed class ResultItem
    {
        public ResultItem(string text, Document doc, int line, int col, int length)
        {
            Text = text;
            Document = doc;
            Line = line;
            Column = col;
            Length = length;
        }

        public string Text { get; private set; }

        public Document Document { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public int Length { get; private set; }
    }
}
