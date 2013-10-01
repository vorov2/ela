using System;
using System.Collections.Generic;
using System.IO;
using Elide.CodeEditor.Infrastructure;
using Elide.CodeEditor.Views;
using Elide.Environment.Editors;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;

namespace Elide.CodeEditor
{
    public abstract class CodeDocument : TextDocument
    {
        protected CodeDocument(FileInfo fileInfo, SciDocument sciDoc) : base(fileInfo, sciDoc)
        {
            UnitVersion = -1;
        }

        protected CodeDocument(string title, SciDocument sciDoc) : base(title, sciDoc)
        {
            UnitVersion = -1;
        }

        public string GetContent()
        {
            return ((ITextEditor)CodeEditor.Instance).GetContent(this);
        }

        public CodeEditorFeatures Features { get; protected set; }

        public IEnumerable<MessageItem> Messages { get; internal set; }

        internal EditorInfo CodeEditor { get; set; }

        public ICompiledUnit Unit { get; internal set; }

        public int Version { get; internal set; }

        public int UnitVersion { get; internal set; }
    }
}
