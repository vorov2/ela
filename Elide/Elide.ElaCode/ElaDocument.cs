using System;
using System.IO;
using Elide.CodeEditor;
using Elide.Scintilla.ObjectModel;

namespace Elide.ElaCode
{
    public sealed class ElaDocument : CodeDocument
    {
        internal ElaDocument(FileInfo fileInfo, SciDocument sciDoc) : base(fileInfo, sciDoc)
        {
            Features = CodeEditorFeatures.Outline | CodeEditorFeatures.Tasks;
        }

        internal ElaDocument(string title, SciDocument sciDoc) : base(title, sciDoc)
        {
            Features = CodeEditorFeatures.Outline | CodeEditorFeatures.Tasks;
        }

        internal new SciDocument GetSciDocument()
        {
            return base.GetSciDocument();
        }
    }
}
