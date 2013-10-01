using System;
using System.Windows.Forms;
using Elide.CodeEditor;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.Scintilla;
using System.IO;

namespace Elide.CodeWorkbench.Views
{
    public sealed class OutlineView : AbstractView
    {
        private OutlineControl control;
        
        public OutlineView()
        {
            this.control = new OutlineControl();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            control.App = app;
            app.GetService<IDocumentService>().DocumentAdded += DocumentAdded;
            app.GetService<IDocumentService>().DocumentRemoved += DocumentRemoved;
            BuildNodes();
        }

        private void DocumentChanged()
        {
            var codeDoc = App.Document() as CodeDocument;

            if (codeDoc != null)
                control.CollapseNode(codeDoc);
        }
        
        private void ChangeNodeText(CodeDocument doc, TreeNode node)
        {
            var title = new FileInfo(doc.Title).ShortName();
            node.Text = title;
        }

        private void BuildNodes()
        {
            foreach (var d in App.GetService<IDocumentService>().EnumerateDocuments())
                AddDocument(d as CodeDocument);
        }

        private void DocumentRemoved(object sender, DocumentEventArgs e)
        {
            var codeDoc = e.Document as CodeDocument;

            if (codeDoc != null)
                control.RemoveDocumentNode(codeDoc);
        }

        private void DocumentAdded(object sender, DocumentEventArgs e)
        {
            AddDocument(e.Document as CodeDocument);
        }

        private void AddDocument(CodeDocument codeDoc)
        {
            if (codeDoc != null && codeDoc.Features.Set(CodeEditorFeatures.Outline))
            {
                var tn = control.AddDocumentNode(codeDoc);
                var sci = (ScintillaControl)App.Editor(codeDoc.GetType()).Control;
                sci.TextChanged += (o,e) => DocumentChanged();
                codeDoc.FileChanged += (o,e) => ChangeNodeText(codeDoc, tn);
            }
        }

        public override object Control
        {
            get { return control; }
        }
    }
}
