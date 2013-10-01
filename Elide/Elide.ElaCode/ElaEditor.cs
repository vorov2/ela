using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Elide.CodeEditor;
using Elide.CodeEditor.Infrastructure;
using Elide.Core;
using Elide.ElaCode.Images;
using Elide.ElaCode.Lexer;
using Elide.Environment;
using Elide.Forms;
using Elide.Scintilla;

namespace Elide.ElaCode
{
    public sealed class ElaEditor : CodeEditor<ElaDocument>
    {
        private FoldingManager folding;

        public ElaEditor() : base("ElaCode")
        {
            
        }

        protected override void InternalInitialize()
        {
            base.InternalInitialize();

            var sci = GetScintilla();
            sci.SetWordChars("abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789'_");
            sci.SmartIndentRequired += SmartIndentRequired;
            sci.AutocompleteIgnoreCase = true;
            folding = new FoldingManager(sci);            
            ElaFuns = new ElaFunctions(App, sci);
        }

        private void SmartIndentRequired(object sender, EventArgs e)
        {
            var sci = GetScintilla();
            var ln = sci.GetLine(sci.CurrentLine - 1).Text;

            if (ln.TrimStart().StartsWith("|"))
            {
                if (!ln.TrimStart(' ', '|').StartsWith("else"))
                {
                    var indent = ln.IndexOf("|");
                    sci.SetLineIndentation(sci.CurrentLine, indent);
                    sci.CaretPosition = sci.GetPositionByColumn(sci.CurrentLine, indent);
                }

                return;
            }

            var trimEnd = ln.TrimEnd('\r', '\n', ' ', '\0');

            if (trimEnd.EndsWith("=") || trimEnd.EndsWith("where"))
            {
                var indent = sci.GetLineIndentation(sci.CurrentLine - 1) + 2;
                sci.SetLineIndentation(sci.CurrentLine, indent);
                sci.CaretPosition = sci.GetPositionByColumn(sci.CurrentLine, indent);
            }
            else
            {
                var idx = ln.IndexOf("|");

                if (idx != -1 && ln.IndexOf("=") > idx && ln.IndexOf("|", idx + 1) == -1)
                {
                    sci.SetLineIndentation(sci.CurrentLine, idx);
                    sci.CaretPosition = sci.GetPositionByColumn(sci.CurrentLine, idx);
                }
            }
        }

        private void Indent()
        {
            var sci = GetScintilla();
            sci.ExecCommand(2329);//NewLine
            var ln = sci.GetLine(sci.CurrentLine - 1).Text.ToUpper();
            var indent = 0;

            if (ln.TrimStart().StartsWith("WHERE"))
                indent = sci.GetLineIndentation(sci.CurrentLine - 1) + 6;
            else
                indent = sci.GetLineIndentation(sci.CurrentLine - 1);

            sci.SetLineIndentation(sci.CurrentLine, indent);
            sci.CaretPosition = sci.GetPositionByColumn(sci.CurrentLine, indent);
        }

        protected override void BuildAuxMenus(IMenuBuilder<MenuStrip> builder)
        {
            var sci = GetScintilla();            
            var rs = App.GetService<ICodeRunnerService>();
            builder
                  .Menu("&Code")
                      .Item("&Find Symbol", "Alt+F12", ElaFuns.FindSymbol, () => sci.GetTextLength() > 0)
                      .Item("&Autocomplete", "Ctrl+Space", ElaFuns.Autocomplete)
                      .CloseMenu()
                  .Menu("&Build")
                      .Item("Run", "F5", ElaFuns.Run, () => sci.GetTextLength() > 0 && !rs.IsRunning())
                      .Item("Stop Execution", () => rs.AbortExecution(), rs.IsRunning)
                      .Item("Eval Selected", "Ctrl+F5", ElaFuns.RunSelected, sci.HasSelections)
                      .Separator()
                      .Item("Generate EIL", ElaFuns.GenerateEil)
                      .Item("Make Object File", ElaFuns.MakeObjectFile)
                      .CloseMenu();
        }

        protected override void BuildAuxEditMenu(IMenuBuilder<MenuStrip> builder)
        {
            var sci = GetScintilla();
            builder
                .Menu("&Outlining")
                    .Item("&Toggle Outlining Expansion", "Ctrl+M", () => sci.ToggleFold(sci.CurrentLine))
                    .Item("&Collapse to Definitions", sci.CollapseAllFold)
                    .Item("&Expand All Code", "Ctrl+Shift+M", sci.ExpandAllFold)
                    .CloseMenu()
                .Menu("&Indentantion")
                    .Item("&New Line with Indent", "Ctrl+Enter", Indent)
                    .CloseMenu();
        }

        protected override void BuildAuxContextMenu(IMenuBuilder<ContextMenuStrip> builder)
        {
            var sci = GetScintilla();
            builder
                .Item("Run", ElaFuns.Run, () => sci.GetTextLength() > 0)
                .Item("Eval Selected", ElaFuns.RunSelected, sci.HasSelections)
                .Separator()
                .Item("Find Symbol", ElaFuns.FindSymbol, () => sci.GetTextLength() > 0)
                .Menu("Outlining")
                    .Item("Toggle Outlining Expansion", "Ctrl+M", () => sci.ToggleFold(sci.CurrentLine))
                    .Item("Collapse to Definitions", sci.CollapseAllFold)
                    .Item("Expand All Code", "Ctrl+Shift+M", sci.ExpandAllFold)
                    .CloseMenu()
                .Separator();
        }
        
        protected override void FoldNeeded(FoldNeededEventArgs e)
        {
            folding.Fold(e);
        }

        protected override void ShowAutocomplete(int position)
        {
            ElaFuns.Autocomplete(position);
        }

        public override bool TestDocumentType(FileInfo fileInfo)
        {
            return fileInfo != null && fileInfo.HasExtension("ela");
        }

        private ElaDocument Doc()
        {
            return App.Document() as ElaDocument;
        }

        public override Image DocumentIcon
        {
            get { return Bitmaps.Load<NS>("Icon"); }
        }

        protected override CodeEditorConfig GetConfig()
        {
            return App.Config<CodeEditorConfig>();
        }

        internal ElaFunctions ElaFuns { get; private set; }
    }
}
