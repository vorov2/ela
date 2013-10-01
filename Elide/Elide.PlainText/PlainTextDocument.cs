using System;
using System.IO;
using Elide.Core;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;

namespace Elide.PlainText
{
    public sealed class PlainTextDocument : TextDocument
    {
        internal PlainTextDocument(FileInfo fileInfo, SciDocument sciDoc) : base(fileInfo, sciDoc)
        {
               
        } 
        
        internal PlainTextDocument(string title, SciDocument sciDoc) : base(title, sciDoc)
        {
               
        }
    }
}
