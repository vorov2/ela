using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Forms;
using Elide.Workbench.Configuration;

namespace Elide.Workbench
{
    public sealed class FileService : Service, IFileService
    {
        private static readonly object stub = new Object();
        
        public FileService()
        {

        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            var docs = app.GetService<IDocumentService>();
            docs.ActiveDocumentChanged += ActiveDocumentChanged;
            docs.DocumentAdded += DocumentAdded;
            docs.DocumentClosed += DocumentClosed;
            docs.DocumentRemoved += DocumentRemoved;
        }

        public void NewFile(string editorKey)
        {
            var editor = (EditorInfo)App.GetService<IEditorService>().GetInfo("editors", editorKey);
            var doc = editor.Instance.CreateDocument("untitled" + editor.FileExtension);
            App.GetService<IDocumentService>().AddDocument(doc);
        }

        public void NewDefaultFile()
        {
            NewFile(EditorFlags.Default);
        }

        public void NewMainFile()
        {
            NewFile(EditorFlags.Main);    
        }

        public void OpenFile()
        {
            var files = App.GetService<IDialogService>().ShowOpenDialog(true);

            if (files != null)
                files.ForEach(OpenFile);
        }

        public void OpenFile(FileInfo fi)
        {
            var doc = App.GetService<IDocumentService>().GetOpenedDocument(fi);

            if (doc == null)
            {
                doc = App.Editor(fi).OpenDocument(fi);
                App.GetService<IDocumentService>().AddDocument(doc);
            }
            else
                App.GetService<IDocumentService>().SetActiveDocument(doc);

            App.GetService<IRecentFilesService>().AddRecentFile(fi);
            App.GetService<IStatusBarService>().ClearStatusString();
        }

        public void Save()
        {
            var doc = App.Document();

            if (doc.FileInfo == null)
                SaveAs(doc);
            else
                Save(doc, doc.FileInfo);

            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "File '{0}' saved", doc.Title);
        }

        public void Save(Document doc)
        {
            Save(doc, doc.FileInfo);
        }

        public void Save(Document doc, FileInfo file)
        {
            if (file == null)
                SaveAs(doc);
            else
            {
                var fw = doc.Tag as FileSystemWatcher;

                if (fw != null)
                    fw.EnableRaisingEvents = false;

                try
                {
                    var editor = App.GetService<IEditorService>().GetEditor(doc.GetType());
                    editor.Instance.Save(doc, file);
                }
                finally
                {
                    if (fw != null)
                        fw.EnableRaisingEvents = false;
                }
            }
        }

        public void SaveAll()
        {
            foreach (var doc in App.GetService<IDocumentService>().EnumerateDocuments())
            {
                if (doc.FileInfo == null)
                    SaveAs(doc);
                else
                    Save(doc, doc.FileInfo);
            }

            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "File(s) saved");
        }

        public void SaveAs()
        {
            var doc = App.Document();
            SaveAs(doc);
            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "File '{0}' saved", doc.FileInfo);
        }

        public void SaveAs(Document doc)
        {
            var fi = App.GetService<IDialogService>().ShowSaveDialog(doc.Title);

            if (fi != null)
                Save(doc, fi);
        }

        public void Close()
        {
            var active = App.Document();
            
            if (active != null)
                Close(active);
        }

        public void CloseAll()
        {
            var active = App.Document();
            App.GetService<IDocumentService>().EnumerateDocuments()
                .OrderByDescending(d => d != active)
                .ForEach(d => Close(d));
        }

        public void CloseAllOther()
        {
            var doc = App.Document();
            App.GetService<IDocumentService>().EnumerateDocuments()
                .Where(d => d != doc)
                .ForEach(d => Close(d));
        }

        public bool Close(Document doc)
        {
            if (!CheckDirty(doc))
                return false;

            App.GetService<IDocumentService>().RemoveDocument(doc);
            return true;
        }

        private bool CheckDirty(Document doc)
        {
            if (doc.IsDirty)
            {
                var res = App.GetService<IDialogService>().ShowPromptDialog("Document '{0}' is changed. Do you want to save it?", doc.Title);

                if (res == null)
                    return false;
                else if (res.Value)
                {
                    Save(doc, doc.FileInfo);
                    return true;
                }
                else
                    return true;
            }
            else
                return true;
        }

        private void NewFile(EditorFlags flags)
        {
            var editor = App.EditorInfo(flags);
            var doc = editor.Instance.CreateDocument("untitled" + editor.FileExtension);
            App.GetService<IDocumentService>().AddDocument(doc);
        }

        private void DocumentRemoved(object sender, DocumentEventArgs e)
        {
            var item = WB.Form.DocumentContainer.FindItemByTag(e.Document);
            WB.Form.DocumentContainer.RemoveDocument(item);
            WB.Form.DocumentContainer.Refresh();
        }

        private void DocumentClosed(object sender, DocumentEventArgs e)
        {
            var active = App.Document();

            if (active == null || active.GetType() != e.Document.GetType())
            {
                WB.Form.DocumentPanel.Controls.Clear();
                WB.Form.DocumentPanel.Refresh();

                foreach (var i in WB.Form.MenuStrip.Items.OfType<ToolStripItem>().ToList().Where(m => ((Tag)m.Tag).Data == stub))
                    WB.Form.MenuStrip.Items.Remove(i);
            }
        }

        private void DocumentAdded(object sender, DocumentEventArgs e)
        {
            var doc = e.Document;
            Func<String> fun = () =>
            {
                var conf = App.Config<WorkbenchConfig>();
                var title = conf.FullFilePath ? doc.Title : new FileInfo(doc.Title).Name;
                return doc.IsDirty ? title + "*" : title;
            };
            WB.Form.DocumentContainer.AddDocument(fun, doc);
            WB.Form.DocumentContainer.Refresh();
        }

        private void ActiveDocumentChanged(object sender, DocumentEventArgs e)
        {
            var doc = e.Document;
            var editor = App.GetService<IEditorService>().GetEditor(doc.GetType());

            var ctl = (Control)editor.Instance.Control;

            if (ctl.CanSelect)
                ctl.Select();

            if (WB.Form.DocumentPanel.Controls.Count == 0 || WB.Form.DocumentPanel.Controls[0] != ctl)
            {
                ctl.Dock = DockStyle.Fill;
                WB.Form.DocumentPanel.Controls.Add(ctl);

                if (editor.Instance.Menu != null)
                {
                    var idx = 1;
                    var auxMenu = (MenuStrip)editor.Instance.Menu;
                    auxMenu.Items.OfType<ToolStripItem>().ToList().ForEach(i =>
                    {
                        ((Tag)i.Tag).Data = stub;
                        WB.Form.MenuStrip.Items.Insert(++idx, i);
                    });

                    WB.Form.MenuStrip.Renderer = new MenuRenderer();
                }
            }

            WB.Form.DocumentContainer.Refresh();
            WB.Form.UpdateWindowHeader();
            editor.Instance.OpenDocument(doc);
        }
    }
}
