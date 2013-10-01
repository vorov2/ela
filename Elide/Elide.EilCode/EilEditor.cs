using System;
using System.Drawing;
using System.IO;
using Elide.CodeEditor;
using Elide.EilCode.Images;

namespace Elide.EilCode
{
    public sealed class EilEditor : CodeEditor<EilDocument>
    {
        public EilEditor() : base("EilCode")
        {
            
        }

        public override bool TestDocumentType(FileInfo fileInfo)
        {
            return fileInfo != null && fileInfo.HasExtension("eil");
        }

        protected override CodeEditorConfig GetConfig()
        {
            return null;
        }

        public override Image DocumentIcon
        {
            get { return Elide.Forms.Bitmaps.Load<NS>("Icon"); }
        }
    }
}
