using System;
using System.IO;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class CodeException : Exception
    {
        private readonly Exception ex;
        private readonly string message;

        public CodeException(Document doc, int line, Exception ex)
        {
            this.ex = ex;
            Document = doc;
            Line = line;
        }

        public CodeException(Document doc, int line, string message)
        {
            this.message = message;
            Document = doc;
            Line = line;
        }

        public override string ToString()
        {
            return ex != null ? ex.ToString() : message;
        }

        public Document Document { get; private set; }

        public int Line { get; private set; }

        public override string Message
        {
            get
            {
                return ex != null ? ex.Message : message;
            }
        }
    }
}
