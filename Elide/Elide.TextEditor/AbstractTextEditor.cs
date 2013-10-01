using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Editors;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;
using Elide.TextEditor;
using Elide.TextEditor.Configuration;
using Elide.Main;

namespace Elide.TextEditor
{
    public abstract class AbstractTextEditor<T> : IEditor, ITextEditor where T : TextDocument
    {
        private ScintillaControl sci;
        private List<T> documents;
        private readonly string editorKey;

        protected AbstractTextEditor(string editorKey)
        {
            this.editorKey = editorKey;
            this.sci = new ScintillaControl();
            this.documents = new List<T>();
        }

        public virtual void Initialize(IApp app)
        {
            App = app;
            
            this.sci.SavePointReached += (o, e) => WithCurrentDocument(d => d.IsDirty = false);
            this.sci.SavePointLeft += (o, e) => WithCurrentDocument(d => d.IsDirty = true);
            this.sci.DocumentAttached += (o, e) =>
            {
                var def = App.Config<TextEditorsConfig>().Default;
                var c = App.Config<TextEditorsConfig>().Configs[editorKey];
                sci.UseTabs = c.UseTabs == null ? def.UseTabs.Value : c.UseTabs.Value;
                sci.TabSize = c.TabSize == null ? def.TabSize.Value : c.TabSize.Value;
                sci.IndentSize = c.IndentSize == null ? def.IndentSize.Value : c.IndentSize.Value;            
            };
            App.GetService<IConfigService>().ConfigUpdated += ConfigUpdated;

            var srv = App.GetService<IStylesConfigService>();
            srv.EnumerateStyleItems(editorKey).UpdateStyles(sci);
            InternalInitialize();
            UpdateTextEditorSettings();

            this.sci.ContextMenuStrip = BuildContextMenu();            
        }

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {
            if (e.Config is StylesConfig)
            {
                var con = (StylesConfig)e.Config;
                con.Styles[editorKey].UpdateStyles(sci);
            }
            else if (e.Config is TextEditorsConfig)
            {
                var con = (TextEditorsConfig)e.Config;
                var c = con.Configs[editorKey];
                UpdateTextEditorSettings(c);
            }

            ConfigUpdated(e.Config);
        }

        protected void UpdateTextEditorSettings()
        {
            var c = App.Config<TextEditorsConfig>().Configs[editorKey];
            UpdateTextEditorSettings(c);
        }

        protected void UpdateTextEditorSettings(TextConfig c)
        {
            var def = App.Config<TextEditorsConfig>().Default;

            var cc = (Func<KnownColor?,Color>)(k => Color.FromKnownColor(k.Value));

            sci.CaretLineVisible = c.CaretLineVisible == null ? def.CaretLineVisible.Value : c.CaretLineVisible.Value;
            sci.CaretLineAlpha = c.CaretLineAlpha == null ? def.CaretLineAlpha.Value : c.CaretLineAlpha.Value;
            sci.CaretLineColor = c.CaretLineColor == null ? cc(def.CaretLineColor) : cc(c.CaretLineColor);
            sci.CaretColor = c.CaretColor == null ? cc(def.CaretColor) : cc(c.CaretColor);
            sci.CaretStyle = c.CaretStyle == null ? def.CaretStyle.Value : c.CaretStyle.Value;
            sci.CaretWidth = c.CaretWidth == null ? def.CaretWidth.Value : c.CaretWidth.Value;
            sci.CaretPeriod = c.CaretBlinkPeriod == null ? def.CaretBlinkPeriod.Value : c.CaretBlinkPeriod.Value;
            sci.CaretSticky = c.CaretSticky == null ? def.CaretSticky.Value : c.CaretSticky.Value;

            sci.UseTabs = c.UseTabs == null ? def.UseTabs.Value : c.UseTabs.Value;
            sci.TabSize = c.TabSize == null ? def.TabSize.Value : c.TabSize.Value;
            sci.IndentSize = c.IndentSize == null ? def.IndentSize.Value : c.IndentSize.Value;
            sci.IndentMode = c.IndentMode == null ? def.IndentMode.Value : c.IndentMode.Value;
            sci.LineEndings = c.LineEndings == null ? def.LineEndings.Value : c.LineEndings.Value;

            sci.MultipleSelection = c.MultipleSelection == null ? def.MultipleSelection.Value : c.MultipleSelection.Value;
            sci.MultipleSelectionTyping = c.MultipleSelectionTyping == null ? def.MultipleSelectionTyping.Value : c.MultipleSelectionTyping.Value;
            sci.MultipleSelectionPaste = c.MultipleSelectionPaste == null ? def.MultipleSelectionPaste.Value : c.MultipleSelectionPaste.Value;
            sci.VirtualSpace = c.VirtualSpace == null ? (def.VirtualSpace.Value ? VirtualSpaceMode.AllSelections : VirtualSpaceMode.Disabled) : 
                (c.VirtualSpace.Value ? VirtualSpaceMode.AllSelections : VirtualSpaceMode.Disabled);

            sci.IndentationGuides = c.IndentationGuides == null ? def.IndentationGuides.Value : c.IndentationGuides.Value;
            sci.ViewWhiteSpace = c.VisibleWhiteSpace == null ? def.VisibleWhiteSpace.Value : c.VisibleWhiteSpace.Value;
            sci.ViewEol = c.VisibleLineEndings == null ? def.VisibleLineEndings.Value : c.VisibleLineEndings.Value;
            sci.WordWrapMode = c.WordWrap == null ? def.WordWrap.Value : c.WordWrap.Value;
            sci.WordWrapIndicators = c.WordWrapIndicator == null ? def.WordWrapIndicator.Value : c.WordWrapIndicator.Value;
            sci.LongLineIndicator = c.LongLineIndicator == null ? def.LongLineIndicator.Value : c.LongLineIndicator.Value;
            sci.LongLineColumn = c.LongLine == null ? def.LongLine.Value : c.LongLine.Value;
            sci.UseSelectionColor = c.UseSelectionColor == null ? def.UseSelectionColor.Value : c.UseSelectionColor.Value;
            sci.MainSelectionBackColor = c.SelectionBackColor == null ? cc(def.SelectionBackColor) : cc(c.SelectionBackColor);
            sci.MainSelectionForeColor = c.SelectionForeColor == null ? cc(def.SelectionForeColor) : cc(c.SelectionForeColor);
            sci.SelectionAlpha = c.SelectionTransparency == null ? def.SelectionTransparency.Value : c.SelectionTransparency.Value;
        }

        protected virtual void ConfigUpdated(Config config)
        {

        }

        protected virtual void InternalInitialize()
        {

        }

        protected ScintillaControl GetScintilla()
        {
            return sci;
        }

        public abstract bool TestDocumentType(FileInfo fileInfo);

        public Document CreateDocument(string title)
        {
            return CreateDocument(title, null);
        }

        public Document OpenDocument(FileInfo fileInfo)
        {
            return CreateDocument(null, fileInfo);
        }

        public void OpenDocument(Document doc)
        {
            sci.AttachDocument(((TextDocument)doc).GetSciDocument());
            try { sci.Select(); }
            catch { }
            OnDocumentOpened((T)doc);
        }

        public void ReloadDocument(Document doc, bool silent)
        {
            if (!silent)
            {
                OpenDocument(doc);
                ReadDocumentFile(sci, doc.FileInfo);
            }
            else
            {
                var sciDoc = ((TextDocument)doc).GetSciDocument();

                if (sci.GetCurrentDocument() == sciDoc)
                {
                    OpenDocument(doc);
                    ReadDocumentFile(sci, doc.FileInfo);
                }
                else
                    using (var sciTemp = new BasicScintillaControl())
                    {
                        sciTemp.AttachDocument(sciDoc);
                        ReadDocumentFile(sciTemp, doc.FileInfo);
                    }
            }

            doc.IsDirty = false;
        }

        public virtual void Save(Document doc, FileInfo fileInfo)
        {
            var txtDoc = (TextDocument)doc;
            var source = ((ITextEditor)this).GetContent(doc);
            App.GetService<IOutputInputService>().WriteStringToFile(fileInfo, source);
            txtDoc.ChangeFile(fileInfo);

            if (txtDoc.GetSciDocument() == sci.GetCurrentDocument())
                sci.SetSavePoint();

            txtDoc.IsDirty = false;
        }

        public void CloseDocument(Document doc)
        {
            documents.Remove((T)doc);
            doc.Dispose();
        }

        private TextDocument CreateDocument(string title, FileInfo fileInfo)
        {
            var sciDoc = sci.CreateDocument();
            sci.AttachDocument(sciDoc);
            
            if (fileInfo != null)
                ReadDocumentFile(sci, fileInfo);

            var txtDoc = fileInfo != null ? Reflect.Create<T>(fileInfo, sciDoc) : Reflect.Create<T>(title, sciDoc);
            OnDocumentCreateInstance((T)txtDoc);
            documents.Add((T)txtDoc);
            sci.Select();
            OnDocumentOpened((T)txtDoc);
            return txtDoc;
        }

        protected virtual void OnDocumentCreateInstance(T doc)
        {

        }

        protected virtual void OnDocumentOpened(T doc)
        {

        }

        private void ReadDocumentFile(BasicScintillaControl sci, FileInfo fileInfo)
        {
            if (fileInfo != null)
            {
                var src = App.GetService<IOutputInputService>().ReadFileAsString(fileInfo);
                sci.SetText(src);
                sci.ClearDirtyFlag();
                sci.EmptyUndoBuffer();
            }
            else
                sci.Text = String.Empty;
        }

        private void WithCurrentDocument(Action<T> fun)
        {
            var sciDoc = sci.GetCurrentDocument();
            var txtDoc = documents.FirstOrDefault(d => d.GetSciDocument() == sciDoc);

            if (txtDoc != null)
                fun(txtDoc);
        }

        protected virtual MenuStrip BuildMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<MenuStrip>();
            var bs = App.GetService<IBookmarkService>();
            var ss = App.GetService<ISearchService>();
            var gs = App.GetService<IGotoDialogService>();

            var menu = builder
                .Menu("&Edit")
                    .Item("&Undo", "Ctrl+Z", sci.Undo, sci.CanUndo)
                    .Item("&Redo", "Ctrl+Y", sci.Redo, sci.CanRedo)
                    .Separator()
                    .Item("Cu&t", "Ctrl+X", sci.Cut, sci.HasSelections)
                    .Item("&Copy", "Ctrl+C", sci.Copy, sci.HasSelections)
                    .Item("&Paste", "Ctrl+V", sci.Paste, sci.CanPaste)
                    .Item("Clipboard &Swap", "Ctrl+Shift+V", sci.SwapClipboard, sci.CanPaste)
                    .Item("&Delete", () => sci.ReplaceSelection(String.Empty), sci.HasSelections)
                    .Separator()
                    .Item("Select &All", "Ctrl+A", sci.SelectAll)
                    .Separator()
                        .Menu("&Find and Replace")
                            .Item("&Find", "Ctrl+F", ss.ShowSearchDialog, () => App.Editor() is ITextEditor)
                            .Item("Find &Next", "F3", ss.SearchNext, () => App.Editor() is ITextEditor && ss.CanContinueSearch())
                            .Item("&Replace", "Ctrl+H", ss.ShowSearchReplaceDialog, () => App.Editor() is ITextEditor)
                            .CloseMenu()
                    .Item("&Go To...", "Ctrl+G", gs.ShowGotoDialog, () => sci.LineCount > 1)
                    .Menu("&Bookmarks")
                        .Item("&Toggle Bookmark", "Ctrl+B", bs.ToggleBookmark)
                        .Separator()
                        .Item("&Previous Bookmark", "Ctrl+P", bs.PreviousBookmark)
                        .Item("&Next Bookmark", "Ctrl+E", bs.NextBookmark)
                        .Separator()
                        .Item("&Clear All Bookmarks", bs.ClearBookmarks)
                        .CloseMenu()
                    .Separator()
                    .Menu("&Zoom")
                        .Item("Zoom &In", "Alt+Oemplus", sci.ZoomIn)
                        .Item("Zoom &Out", "Alt+OemMinus", sci.ZoomOut)
                        .Item("Reset Zoom Level", sci.ResetZoom)
                        .CloseMenu()
                    .Menu("A&dvanced")
                        .Item("C&ut Line", "Ctrl+L", sci.CutLine)
                        .Item("&Copy Line", "Ctrl+Shift+T", sci.CopyLine)
                        .Item("&Delete Line", "Ctrl+Shift+L", sci.DeleteLine)
                        .Item("D&uplicate Line", "Ctrl+D", sci.DuplicateLine)
                        .Item("&Transpose Line", "Ctrl+T", sci.TransposeLine)
                        .Item("Move L&ines Up", "Ctrl+Shift+Up", sci.MoveLinesUp)
                        .Item("Move Lines Dow&n", "Ctrl+Shift+Down", sci.MoveLinesDown)
                        .Separator()
                        .Item("&Duplicate Selection", "Ctrl+Shift+D", sci.DuplicateSelection)
                        .Item("&Clear Selections", "Ctrl+Shift+C", sci.ClearSelections)
                        .Separator()
                        .Item("Delete &Word Left", "Ctrl+Alt+L", sci.DeleteWordLeft)
                        .Item("Delete Word &Right", "Ctrl+Alt+R", sci.DeleteWordRight)
                        .Separator()
                        .Item("Make U&ppercase", "Ctrl+Shift+U", sci.MakeUppercase, sci.HasSelections)
                        .Item("Make L&owercase", "Ctrl+U", sci.MakeLowercase, sci.HasSelections)
                        .Separator()
                        .Item("&Insert File As Text...", InsertFileAsText)
                        .CloseMenu()
                    .Items(BuildAuxEditMenu)
                    .CloseMenu()
                .Items(BuildAuxMenus)
                .Finish();
            return (MenuStrip)menu;
        }

        protected virtual void BuildAuxEditMenu(IMenuBuilder<MenuStrip> builder)
        {

        }

        protected virtual void BuildAuxMenus(IMenuBuilder<MenuStrip> builder)
        {
            
        }

        protected virtual ContextMenuStrip BuildContextMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            var menu = (ContextMenuStrip)builder
                .Items(BuildAuxContextMenu)
                .Item("Cut", sci.Cut, sci.HasSelections)
                .Item("Copy", sci.Copy, sci.HasSelections)
                .Item("Paste", sci.Paste, sci.CanPaste)
                .Item("Delete", () => sci.ReplaceSelection(String.Empty), sci.HasSelections)
                .Item("Select All", sci.SelectAll)
                .Separator()
                .Menu("Transform")
                    .Item("Make Uppercase", sci.MakeUppercase, sci.HasSelections)
                    .Item("Make Lowercase", sci.MakeLowercase, sci.HasSelections)
                    .CloseMenu()
                .Menu("Zoom")
                    .Item("Zoom In", sci.ZoomIn)
                    .Item("Zoom Out", sci.ZoomOut)
                    .Item("Reset Zoom Level", sci.ResetZoom)
                    .CloseMenu()
                .Finish();
            return (ContextMenuStrip)menu;
        }

        protected virtual void BuildAuxContextMenu(IMenuBuilder<ContextMenuStrip> builder)
        {

        }

        private void InsertFileAsText()
        {
            var fi = App.GetService<IDialogService>().ShowOpenDialog(false);

            if (fi != null)
            {
                var src = App.GetService<IOutputInputService>().ReadFileAsString(fi.First());
                sci.InsertText(src, sci.CurrentPosition);
            }
        }

        void ITextEditor.SetContent(Document doc, string text)
        {
            if (doc == App.Document())
                sci.SetText(text);
            else
                using (var sciTemp = new BasicScintillaControl())
                {
                    sciTemp.AttachDocument(((TextDocument)doc).GetSciDocument());
                    sciTemp.SetText(text);
                }
        }

        void ITextEditor.BeginUndoAction()
        {
            sci.BeginUndoAction();
        }

        void ITextEditor.EndUndoAction()
        {
            sci.EndUndoAction();
        }

        string ITextEditor.GetContent(Document doc)
        {
            if (doc == App.Document())
                return sci.GetTextUnicode();
            else
                using (var sciTemp = new BasicScintillaControl())
                {
                    sciTemp.AttachDocument(((TextDocument)doc).GetSciDocument());
                    return sciTemp.GetTextUnicode();
                }
        }

        string ITextEditor.GetContent(Document doc, int start, int length)
        {
            if (doc == App.Document())
                return sci.GetTextRangeUnicode(start, start + length);
            else
                using (var sciTemp = new BasicScintillaControl())
                {
                    sciTemp.AttachDocument(((TextDocument)doc).GetSciDocument());
                    return sciTemp.GetTextRangeUnicode(start, start + length);
                }
        }

        string ITextEditor.GetContent(Document doc, int lineNumber)
        {
            if (doc == App.Document())
                return sci.GetLine(lineNumber).Text;
            else
                using (var sciTemp = new BasicScintillaControl())
                {
                    sciTemp.AttachDocument(((TextDocument)doc).GetSciDocument());
                    return sciTemp.GetLine(lineNumber).Text;
                }
        }

        TextLocation ITextEditor.GetLocationFromPosition(Document doc, int pos)
        {
            using (var sciTemp = new BasicScintillaControl())
            {
                sciTemp.AttachDocument(((TextDocument)doc).GetSciDocument());
                var line = sciTemp.GetLineFromPosition(pos);
                var col = sciTemp.GetColumnFromPosition(pos);
                return new TextLocation(line, col);
            }
        }

        void ITextEditor.SelectText(int start, int length)
        {
            sci.Select(start, length, SelectionFlags.MakeOnlySelection | SelectionFlags.ScrollToCaret);

            if (sci.CanSelect)
                sci.Select();
        }
        
        void ITextEditor.SelectText(int line, int col, int length)
        {
            var pos = sci.GetPositionByColumn(line, col);
            sci.Select(pos, length, SelectionFlags.MakeOnlySelection | SelectionFlags.ScrollToCaret);
            
            if (sci.CanSelect)
                sci.Select();
        }

        void ITextEditor.ReplaceText(int start, int end, string text)
        {
            sci.ReplaceText(start, end, text);
        }

        bool ITextEditor.Overtype
        {
            get { return sci.Overtype; }
            set { sci.Overtype = value; }
        }

        int ITextEditor.CurrentLine
        {
            get { return sci.CurrentLine; }
        }

        int ITextEditor.CurrentColumn
        {
            get { return sci.GetColumnFromPosition(sci.CurrentPosition); }
        }

        int ITextEditor.SelectionStart
        {
            get { return sci.GetSelection().Start; }
        }

        int ITextEditor.SelectionEnd
        {
            get { return sci.GetSelection().End; }
        }

        int ITextEditor.CaretPosition
        {
            get { return sci.GetSelection().CaretPosition; }
        }

        public object Control
        {
            get { return sci; }
        }

        public virtual object Menu
        {
            get { return BuildMenu(); }
        }

        public string EditorKey
        {
            get { return editorKey; }
        }

        public abstract Image DocumentIcon { get; }

        protected IApp App { get; private set; }
    }
}
