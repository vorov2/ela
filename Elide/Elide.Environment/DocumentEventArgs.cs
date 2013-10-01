using System;

namespace Elide.Environment
{
    public sealed class DocumentEventArgs : EventArgs
    {
        public DocumentEventArgs(Document doc)
        {
            Document = doc;
        }

        public Document Document { get; private set; }
    }
}
