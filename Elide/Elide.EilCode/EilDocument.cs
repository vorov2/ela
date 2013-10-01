using System;
using System.IO;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;
using Elide.CodeEditor;

namespace Elide.EilCode
{
    public sealed class EilDocument : CodeDocument
    {
        internal EilDocument(FileInfo fileInfo, SciDocument sciDoc) : base(fileInfo, sciDoc)
        {

        }

        internal EilDocument(string title, SciDocument sciDoc) : base(title, sciDoc)
        {

        }

        internal new SciDocument GetSciDocument()
        {
            return base.GetSciDocument();
        }
    }
}
