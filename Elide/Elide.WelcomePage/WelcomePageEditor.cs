using System;
using System.Drawing;
using System.IO;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Forms;
using Elide.WelcomePage.Images;

namespace Elide.WelcomePage
{
    public sealed class WelcomePageEditor : IEditor
    {
        private WelcomePageView control;

        public WelcomePageEditor()
        {
            control = new WelcomePageView();
        }

        public void Initialize(IApp app)
        {
            App = app;
            control.App = app;
        }

        public bool TestDocumentType(FileInfo fileInfo)
        {
            return false;
        }

        public Document CreateDocument(string title)
        {
            return WelcomePageDocument.Instance;
        }

        public Document OpenDocument(FileInfo fileInfo)
        {
            throw new NotSupportedException("WelcomePage editor is unable to open files.");
        }

        public void OpenDocument(Document doc)
        {
            if (doc != WelcomePageDocument.Instance)
                throw new ElideException("Unable to open document '{0}'.", doc.Title);
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
