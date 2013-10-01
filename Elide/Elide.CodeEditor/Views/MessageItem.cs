using System;
using Elide.Environment;

namespace Elide.CodeEditor.Views
{
    public sealed class MessageItem
    {
        public MessageItem(MessageItemType type, string message, Document doc, int line, int col)
        {
            Type = type;
            Message = message;
            Document = doc;
            Line = line;
            Column = col;
        }

        public MessageItemType Type { get; private set; }

        public string Message { get; private set; }

        public Document Document { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }

        public object Tag { get; set; }
    }
}
