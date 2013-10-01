using System;
using System.IO;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;

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

        internal void ChangeFile(FileInfo fileInfo)
        {
            FileInfo = fileInfo;
        }

        public SciDocument GetSciDocument()
        {
            return sciDoc;
        }

        public override bool IsDirty { get; set; }
    }
}
