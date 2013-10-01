using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;
using Elide.TextWorkbench.Dialogs;

namespace Elide.TextWorkbench
{
    public sealed class SearchService : Service, ISearchService
    {
        private sealed class SearchSettings
        {
            internal string Text { get; set; }
            internal string TextToReplace { get; set; }
            internal SearchFlags Flags { get; set; }
            internal SearchScope Scope { get; set; }
            internal int LastStartPosition { get; set; }
            internal int LastEndPosition { get; set; }
            internal int MaxPosition { get; set; }
            internal Document LastDocument { get; set; }
            internal List<Document> ScannedDocuments { get; set; }
            internal SearchManager SearchManager { get; set; }
        }

        private SearchSettings lastSettings;
        private List<Document> scanned;
        
        public SearchService()
        {
            
        }

        public void ShowSearchDialog()
        {
            var dlg = new SearchDialog { App = App };
            dlg.Show((IWin32Window)App.GetService<IEnvironmentService>().GetMainWindow());
        }

        public void ShowSearchReplaceDialog()
        {
            var dlg = new SearchDialog { App = App, ReplaceMode = true };
            dlg.Show((IWin32Window)App.GetService<IEnvironmentService>().GetMainWindow());
        }
        
        public void ReplaceAll(string text, string textToReplace, Document doc, SearchFlags flags, SearchScope scope)
        {
            lastSettings = null;
            scanned = null;
            var items = new List<ResultItem>();
            var sw = new Stopwatch();
            sw.Start();
            var rootEditor = (ITextEditor)App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance;
            rootEditor.BeginUndoAction();

            var startPos = scope == SearchScope.Selection ? rootEditor.SelectionStart :
                scope == SearchScope.Current ? rootEditor.CaretPosition : 0;
            var endPos = scope == SearchScope.Selection ? rootEditor.SelectionEnd : 0;

            for (;;)
            {
                var res = Search(text, lastSettings != null ? lastSettings.LastDocument : doc, flags, scope, (d, r) => {
                    var editor = (ITextEditor)App.GetService<IEditorService>().GetEditor(d.GetType()).Instance;
                    var loc = editor.GetLocationFromPosition(d, r.StartPosition);
                    var txt = editor.GetContent(d, loc.Line).TrimStart(' ', '\t');
                    var item = new ResultItem(txt, d, loc.Line, loc.Column, r.EndPosition - r.StartPosition);

                    var docSrv = App.GetService<IDocumentService>();

                    if (docSrv.GetActiveDocument() != d)
                    {
                        rootEditor.EndUndoAction();
                        docSrv.SetActiveDocument(d);
                        editor.BeginUndoAction();
                        rootEditor = editor;
                    }

                    var sci = editor.Control as ScintillaControl;

                    if (sci != null)
                        sci.ReplaceText(r.StartPosition, r.EndPosition, textToReplace);
                    else
                        editor.ReplaceText(r.StartPosition, r.EndPosition, textToReplace);

                    lastSettings.LastStartPosition = r.StartPosition;
                    lastSettings.TextToReplace = textToReplace;
                    lastSettings.LastEndPosition = r.StartPosition + textToReplace.Length;

                    if (items.Contains(item))
                        return false;
                    else
                    {
                        items.Add(item);
                        return true;
                    }
                }, true, 
                lastSettings != null ? lastSettings.LastEndPosition : startPos,
                lastSettings != null ? lastSettings.MaxPosition : endPos, null);

                if (!res)
                    break;
            }

            rootEditor.EndUndoAction();
            sw.Stop();
            var s = (Single)sw.ElapsedMilliseconds / 1000;
            
            var svc = App.GetService<IResultGridService>();
            svc.AddItems(items);
            App.OpenView("ResultGrid");
            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "{0} entries replaced. Replace took: {1:F2} s.", items.Count, s);            
        }

        public void SearchAll(string text, Document doc, SearchFlags flags, SearchScope scope)
        {
            lastSettings = null;
            scanned = null;
            var items = new List<ResultItem>();
            var sw = new Stopwatch();
            sw.Start();
            var sm = default(SearchManager);
            var status = "{0} element(s) found. Search took: {1:F2} s.";
            var rootEditor = (ITextEditor)App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance;
            var startPos = scope == SearchScope.Selection ? rootEditor.SelectionStart :
                scope == SearchScope.Current ? rootEditor.CaretPosition : 0;
            var endPos = scope == SearchScope.Selection ? rootEditor.SelectionEnd : 0;

            for(;;)
            {
                var res = Search(text, lastSettings != null ? lastSettings.LastDocument : doc, flags, scope, (d,r) =>
                    {
                        var editor = (ITextEditor)App.GetService<IEditorService>().GetEditor(d.GetType()).Instance;
                        var loc = editor.GetLocationFromPosition(d, r.StartPosition);
                        var txt = editor.GetContent(d, loc.Line).TrimStart(' ', '\t');
                        var item = new ResultItem(txt, d, loc.Line, loc.Column, r.EndPosition - r.StartPosition);

                        if (items.Count >= 1000)
                        {
                            status = "Too many entries found. Showing first {0}. Search took: {1:F2} s.";
                            return false;
                        }
                        else if (items.Contains(item))
                            return false;
                        else
                        {
                            items.Add(item);
                            return true;
                        }                        
                    }, true, 
                    lastSettings != null ? lastSettings.LastEndPosition : 0,
                    lastSettings != null ? lastSettings.MaxPosition : endPos, sm);

                if (!res)
                    break;

                sm = lastSettings.SearchManager;
            }

            sw.Stop();
            var s = (Single)sw.ElapsedMilliseconds / 1000;

            var svc = App.GetService<IResultGridService>();
            svc.AddItems(items);
            App.OpenView("ResultGrid");
            App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, status, items.Count, s);            
        }

        public void Search(string text, Document doc, SearchFlags flags, SearchScope scope)
        {
            if (lastSettings != null)
                Search(text, doc, flags, scope, lastSettings.LastEndPosition, lastSettings.MaxPosition);
            else
            {
                var rootEditor = (ITextEditor)App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance;
                var sp = scope == SearchScope.Selection ? rootEditor.SelectionStart : 
                    scope == SearchScope.Current ? rootEditor.CaretPosition : 0;
                var ep = scope == SearchScope.Selection ? rootEditor.SelectionEnd : 0;
                Search(text, doc, flags, scope, sp, ep);
            }
            
        }

        public void Search(string text, Document doc, SearchFlags flags, SearchScope scope, int startPosition, int endPosition)
        {
            if (lastSettings == null)
                App.GetService<IStatusBarService>().ClearStatusString();

            var res = Search(text, doc, flags, scope, (d,r) => {
                var editor = (ITextEditor)App.GetService<IEditorService>().GetEditor(d.GetType()).Instance;
                editor.SelectText(r.StartPosition, r.EndPosition - r.StartPosition);
                var sci = editor.Control as ScintillaControl;

                if (sci != null)
                    sci.PutArrow(sci.GetLineFromPosition(r.StartPosition));
                return true;
            }, false, startPosition, endPosition, null);
            IsFinished(res);
        }

        public void SearchNext()
        {
            if (lastSettings != null)
            {
                if (lastSettings.LastDocument.IsAlive)
                    Search(lastSettings.Text, lastSettings.LastDocument, lastSettings.Flags, lastSettings.Scope, lastSettings.LastEndPosition, lastSettings.MaxPosition);
                else
                    lastSettings = null;
            }
        }

        public void Replace(string textToFind, string textToReplace, Document doc, SearchFlags flags, SearchScope scope)
        {
            if (lastSettings != null)
                Replace(textToFind, textToReplace, doc, flags, scope, lastSettings.LastEndPosition, lastSettings.MaxPosition);
            else
            {
                var rootEditor = (ITextEditor)App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance;
                var sp = scope == SearchScope.Selection ? rootEditor.SelectionStart : 
                    scope == SearchScope.Current ? rootEditor.CaretPosition : 0;
                var ep = scope == SearchScope.Selection ? rootEditor.SelectionEnd : 0;
                Replace(textToFind, textToReplace, doc, flags, scope, sp, ep);
            }            
        }
            
        public void Replace(string textToFind, string textToReplace, Document doc, SearchFlags flags, SearchScope scope, int startPosition, int endPosition)
        {
            var ed = App.Editor(doc.GetType()) as ITextEditor;

            if (lastSettings != null &&
                (lastSettings.LastDocument != doc || ed.SelectionStart != lastSettings.LastStartPosition || ed.SelectionEnd != lastSettings.LastEndPosition))
                lastSettings = null;

            if (lastSettings != null)
            {
                ed.ReplaceText(lastSettings.LastStartPosition, lastSettings.LastEndPosition, lastSettings.TextToReplace);
                lastSettings.LastEndPosition = lastSettings.LastStartPosition + lastSettings.TextToReplace.Length;
            }

            var res = Search(textToFind, doc, flags, scope, (d,r) => {
                var editor = (ITextEditor)App.GetService<IEditorService>().GetEditor(d.GetType()).Instance;
                
                var docServ = App.GetService<IDocumentService>();
                
                if (docServ.GetActiveDocument() != d)
                    docServ.SetActiveDocument(d);

                editor.SelectText(r.StartPosition, r.EndPosition - r.StartPosition);
                lastSettings.LastStartPosition = r.StartPosition;
                lastSettings.TextToReplace = textToReplace; 
                var sci = editor.Control as ScintillaControl;

                if (sci != null)
                    sci.PutArrow(sci.GetLineFromPosition(r.StartPosition));
                return true;
            }, false, lastSettings != null ? lastSettings.LastEndPosition : startPosition, endPosition, null);
            IsFinished(res);
        }

        public bool CanContinueSearch()
        {
            return lastSettings != null;
        }
        
        private bool Search(string textToFind, Document doc, SearchFlags flags, SearchScope scope, Func<Document,SearchResult,Boolean> foundAction, bool silent, 
            int startPosition, int endPosition, SearchManager sm)
        {
            lastSettings = null;            
            var editor = (ITextEditor)App.Editor(doc.GetType());            
            sm = sm ?? new SearchManager(editor.GetContent(doc));
            
            var sp = startPosition;
            var ep = endPosition;
            
            var res = sm.Search(flags, textToFind, sp, ep);

            if (res.Found)
            {
                lastSettings = new SearchSettings
                {
                    Text = textToFind,
                    Flags = flags,
                    Scope = scope,
                    LastEndPosition = res.EndPosition,
                    LastDocument = doc,
                    MaxPosition = ep,
                    SearchManager = silent ? sm : null
                };

                if (!silent)
                    sm.Dispose();

                return foundAction(doc, res);
            }
            else if (scope == SearchScope.AllDocuments)
            {
                sm.Dispose();
                sm = null;

                var srv = App.GetService<IDocumentService>();
                var newDoc = GetNextDocument(doc);

                if (newDoc != null)
                {
                    if (scanned == null)
                        scanned = new List<Document>();

                    if (scanned.IndexOf(newDoc) != -1)
                        return false;
                    else
                        scanned.Add(newDoc);

                    if (!silent)
                        srv.SetActiveDocument(newDoc);

                    if (silent)
                        sm = new SearchManager(editor.GetContent(newDoc));

                    return Search(textToFind, newDoc, flags, scope, foundAction, silent, 0, 0, sm);
                }

                return false;
            }
            else
            {
                sm.Dispose();
                return false;
            }
        }

        private Document GetNextDocument(Document oldDoc)
        {
            var srv = App.GetService<IDocumentService>();
            var newDoc = oldDoc;

            for (;;)
            {
                newDoc = srv.GetNextDocument(newDoc, typeof(TextDocument));

                if (newDoc == null || newDoc is TextDocument)
                    break;
            }

            return newDoc;
        }

        private void IsFinished(bool val)
        {
            if (!val)
            {
                var srv = App.GetService<IStatusBarService>();
                srv.SetStatusString(StatusType.Warning, "Search finished. No more occurences found");
            }
        }
    }
}
