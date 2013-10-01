using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Xsl;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Forms;
using Elide.HelpViewer.Images;
using Elide.CodeEditor.Infrastructure;
using System.Text;

namespace Elide.HelpViewer
{
    public sealed class HelpEditor : IEditor
    {
        private HelpView control;

        public HelpEditor()
        {
            control = new HelpView();
        }

        public void Initialize(IApp app)
        {
            App = app;
        }

        public bool TestDocumentType(FileInfo fileInfo)
        {
            return fileInfo != null && (fileInfo.HasExtension("htm") || fileInfo.HasExtension("html"));
        }

        public Document CreateDocument(string title)
        {
            var doc = new HelpDocument(title);
            return doc;
        }

        public Document OpenDocument(FileInfo fileInfo)
        {
            control.SetContent(ReadFile(fileInfo));
            var doc = new HelpDocument(fileInfo);
            return doc;
        }

        public void OpenDocument(Document doc)
        {
            control.SetContent(ReadFile(doc.FileInfo)); 
            OpenDocument(doc.FileInfo);
        }

        public void ReloadDocument(Document doc, bool silent)
        {
            
        }

        public void Save(Document doc, FileInfo fileInfo)
        {
            
        }

        public void CloseDocument(Document doc)
        {
            
        }

        private string ReadFile(FileInfo file)
        {
            using (var sr = new StreamReader(file.OpenRead()))
                return sr.ReadToEnd();
        }

        public Image DocumentIcon
        {
            get { return Bitmaps.Load<NS>("Icon"); }
        }

        public object Control
        {
            get { return control; }
        }

        public object Menu
        {
            get { return null; }
        }

        internal IApp App { get; private set; }
    }
}
