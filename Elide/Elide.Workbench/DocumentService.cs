using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;

namespace Elide.Workbench
{
    public sealed class DocumentService : Service, IDocumentService
    {
        private List<Document> documents;
        private Document activeDocument;

        public DocumentService()
        {
            this.documents = new List<Document>();
        }

        public IEnumerable<Document> EnumerateDocuments()
        {
            return documents.ToArray();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void AddDocument(Document doc)
        {
            var opDoc = doc;
            
            if (doc != null && doc.IsAlive)
            {
                if (!documents.Any(d => d == doc))
                {
                    opDoc = doc.FileInfo == null ? null :
                        documents.FirstOrDefault(d => d.FileInfo != null && d.FileInfo.ToString() == doc.FileInfo.ToString());

                    if (opDoc == null)                        
                    {
                        documents.Add(doc);
                        doc.FileChanged += (o, e) => CreateWatcher(doc);
                        CreateWatcher(doc);
                        OnDocumentAdded(doc);
                        opDoc = doc;
                    }
                }

                SetActiveDocument(opDoc);
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Document GetOpenedDocument(FileInfo fileInfo)
        {
            return documents.FirstOrDefault(d => d.FileInfo != null && d.FileInfo.FullName == fileInfo.FullName);
        }

        private void CreateWatcher(Document doc)
        {
            if (doc.FileInfo != null)
            {
                var oldw = doc.Tag as FileSystemWatcher;

                if (oldw != null)
                    oldw.Dispose();

                var w = new FileSystemWatcher(doc.FileInfo.DirectoryName);
                w.Filter = doc.FileInfo.Name;
                w.EnableRaisingEvents = true;
                w.Deleted += (o, e) => doc.IsDirty = true;
                w.Renamed += (o, e) => doc.IsDirty = true;
                w.Changed += (o, e) =>
                {
                    try
                    {
                        w.EnableRaisingEvents = false;
                        var ed = App.GetService<IEditorService>().GetEditor(doc.GetType());

                        if (doc.IsDirty)
                        {
                            var srv = App.GetService<IDialogService>();

                            if (srv.ShowWarningDialog("File '{0}' was modified outside of Elide. Do you want to reload it and loose changes?", doc.FileInfo))
                            {
                                ed.Instance.ReloadDocument(doc, true);
                                SetActiveDocument(doc);
                            }
                            else
                                doc.IsDirty = true;
                        }
                        else
                        {
                            try
                            {
                                ed.Instance.ReloadDocument(doc, true);
                            }
                            catch
                            {
                                ed.Instance.ReloadDocument(doc, true);
                            }
                        }
                    }
                    finally
                    {
                        w.EnableRaisingEvents = true;
                    }
                };
                doc.Tag = w;
            }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDocument(Document doc)
        {
            if (IsValidDocument(doc))
            {
                if (activeDocument == doc)
                {
                    activeDocument = null;
                    OnDocumentClosed(doc);

                    var next = GetNextDocument(doc, null);
                    
                    if (next != doc)
                        SetActiveDocument(next);
                }

                doc.Dispose();
                
                var oldw = doc.Tag as FileSystemWatcher;

                if (oldw != null)
                    try { oldw.Dispose(); } catch {}

                documents.Remove(doc);
                OnDocumentRemoved(doc);
            }
        }

        public Document GetActiveDocument()
        {
            return activeDocument;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Document GetNextDocument(Document doc, Type docType)
        {
            if (documents.Count == 1 && documents[0] == doc)
                return null;

            if (IsValidDocument(doc))
            {
                var idx = documents.IndexOf(doc);

                if (idx + 1 < documents.Count)
                {
                    var ret = documents[idx + 1];
                    var t = ret.GetType();

                    if (docType == null || t == docType || t.BaseType == docType)
                        return ret;
                    else
                        return GetNextDocument(ret, docType);
                }
                else if (documents.Count > 0)
                {
                    var ret = documents[0];
                    var t = ret.GetType();

                    if (docType == null || t == docType || t.BaseType == docType)
                        return ret;
                    else
                        return GetNextDocument(ret, docType);
                }
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public Document GetPreviousDocument(Document doc, Type docType)
        {
            if (documents.Count == 1 && documents[0] == doc)
                return null;

            if (IsValidDocument(doc))
            {
                var idx = documents.IndexOf(doc);

                if (idx - 1 >= 0)
                {
                    var ret = documents[idx - 1];
                    var t = ret.GetType();

                    if (docType == null || t == docType || t.BaseType == docType)
                        return ret;
                    else
                        return GetPreviousDocument(ret, docType);
                }
                else if (documents.Count > 0)
                {
                    var ret = documents[documents.Count - 1];
                    var t = ret.GetType();

                    if (docType == null || t == docType || t.BaseType == docType)
                        return ret;
                    else
                        return GetPreviousDocument(ret, docType);
                }
                    
            }

            return null;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public bool SetActiveDocument(Document doc)
        {
            var old = activeDocument;
            
            if (doc == null)
            {
                activeDocument = null;

                if (old != null)
                    OnDocumentClosed(old);

                return true;
            }

            if (doc == activeDocument)
                return true;

            if (!IsValidDocument(doc))
                return false;

            activeDocument = doc;

            if (old != null)
                OnDocumentClosed(old);

            OnActiveDocumentChanged(doc);
            return true;
        }

        private bool IsValidDocument(Document doc)
        {
            return doc != null && doc.IsAlive && documents.Any(d => d == doc);
        }

        public event EventHandler<DocumentEventArgs> ActiveDocumentChanged;
        private void OnActiveDocumentChanged(Document doc)
        {
            var h = ActiveDocumentChanged;
            

            if (h != null)
                Application.OpenForms[0].Invoke(() => h(this, new DocumentEventArgs(doc)));
        }

        public event EventHandler<DocumentEventArgs> DocumentAdded;
        private void OnDocumentAdded(Document doc)
        {
            var h = DocumentAdded;

            if (h != null)
                Application.OpenForms[0].Invoke(() => h(this, new DocumentEventArgs(doc)));
        }

        public event EventHandler<DocumentEventArgs> DocumentRemoved;
        private void OnDocumentRemoved(Document doc)
        {
            var h = DocumentRemoved;

            if (h != null)
                Application.OpenForms[0].Invoke(() => h(this, new DocumentEventArgs(doc)));
        }

        public event EventHandler<DocumentEventArgs> DocumentClosed;
        private void OnDocumentClosed(Document doc)
        {
            var h = DocumentClosed;

            if (h != null)
                Application.OpenForms[0].Invoke(() => h(this, new DocumentEventArgs(doc)));
        }
    }
}
