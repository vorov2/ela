using System;
using System.IO;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;
using System.Collections.Generic;

namespace Elide.TextEditor
{
    public abstract class TextDocument : Document
    {
        private SciDocument sciDoc;

        protected TextDocument(FileInfo fileInfo, SciDocument sciDoc) : base(fileInfo)
        {
            this.sciDoc = sciDoc;
        }

        protected TextDocument(string title, SciDocument sciDoc) : base(title)
        {
            this.sciDoc = sciDoc;
        }

        public override void Dispose()
        {
            if (sciDoc != null)
            {
                sciDoc.Dispose();
                sciDoc = null;
                base.Dispose();
            }
        }

        public override void Close()
        {
            DocumentClosed(this);
        }

        internal void ChangeFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public SciDocument GetSciDocument()
        {
            return sciDoc;
        }

        public override bool IsDirty { get; set; }

        internal int FirstVisibleLine { get; set; }

        internal IEnumerable<TextSelection> Selections { get; set; }

        internal event Action<TextDocument> DocumentClosed;
    }
}
