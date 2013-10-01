using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;
using Elide.CodeEditor.Views;

namespace Elide.Console
{
    public class ConsoleTextBox : Control
    {
        private const int HISTORY_SIZE = 100;
        private readonly ScintillaControl sci;
        private HistoryList<String> history;
        private int lastLen;

        public ConsoleTextBox() : this(HISTORY_SIZE)
        {

        }

        public ConsoleTextBox(int historySize)
        {
            sci = new ScintillaControl();
            history = new HistoryList<String>(historySize);
            Initialize();
        }

        private void Initialize()
        {
            Controls.Add(sci);
            sci.KeyDown += OnKeyDown;

            sci.Dock = DockStyle.Fill;
            sci.Visible = true;
            sci.MarginVisible = false;
            sci.WordWrapMode = WordWrapMode.Char;
            sci.Keyboard.ClearAllKeys();
            sci.Keyboard.ClearKey(SciKey.Return, SciModifier.None);
            sci.Keyboard.ClearKey(SciKey.Back, SciModifier.None);
            sci.Keyboard.ClearKey(SciKey.Escape, SciModifier.None);
            sci.StyleNeeded += Lex;
        }

        private void Lex(object sender, StyleNeededEventArgs e)
        {
            if (!Styling)
                return;

            e.AddStyleItem(sci.GetPositionFromLine(0), sci.GetLineEndColumn(0), TextStyle.Style1);

            if (sci.ReadOnly && sci.LineCount > 2)
                e.AddStyleItem(sci.GetPositionFromLine(sci.LineCount - 2), 100, TextStyle.Style1);
        }

        private void BuildMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();            
            sci.ContextMenuStrip = builder
                .Item("Cut", "Ctrl+X", Cut, sci.HasSelections)
                .Item("Copy", "Ctrl+C", sci.Copy, sci.HasSelections)
                .Item("Paste", "Ctrl+V", Paste, sci.CanPaste)
                .Separator()
                .Item("Clear All", SmartClear, () => sci.GetTextLength() > lastLen)
                .Item("Clear History", () => history.Clear(), () => history.Size > 0)
                .Separator()
                .Item("Search", "Ctrl+F", Search, () => sci.GetTextLength() > 0)
                .Finish();
        }

        private void Search()
        {
            var dlg = new ConsoleSearchForm { Sci = sci, App = App };
            dlg.ShowDialog((IWin32Window)App.GetService<IEnvironmentService>().GetMainWindow());
        }

        private void Cut()
        {
            var sel = sci.GetSelection();
            var sl = sci.GetLineFromPosition(sel.Start);

            if (sl != sci.LineCount - 1 || sci.GetColumnFromPosition(sel.Start) > lastLen)
                sci.Copy();
            else
                sci.Cut();
        }

        private void Paste()
        {
            var cl = sci.GetColumnFromPosition(sci.CurrentPosition);
            var sl = sci.GetLineFromPosition(sci.CurrentPosition);

            if (sl != sci.LineCount - 1 || cl > lastLen)
                sci.CaretPosition = sci.GetPositionFromLine(sci.LineCount - 1) + lastLen;

            sci.Paste();
        }

        private void SmartClear()
        {
            if (sci.ReadOnly)
            {
                Clear();
                return;
            }

            var text = String.Empty;

            if (lastLen != 0)
            {
                var p = sci.GetPositionFromLine(sci.LineCount - 1);
                sci.GetTextRangeUnicode(p, p + lastLen);
            }

            sci.SetText(text);
            sci.SetLineState(0, 0);
            sci.CurrentPosition = sci.GetTextLength();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            var n = NoMod(e);

            if (n && e.KeyCode == Keys.Left && InPos())
                sci.Keyboard.ExecuteCommand(SciCommand.CharLeft);
            else if (n && e.KeyCode == Keys.Right)
                sci.Keyboard.ExecuteCommand(SciCommand.CharRight);
            else if (n && e.KeyCode == Keys.Up && InPosLine())
                ReplaceIf(history.Previous());
            else if (n && e.KeyCode == Keys.Down && InPosLine())
                ReplaceIf(history.Next());
            else if (n && e.KeyCode == Keys.Insert)
                sci.Overtype = !sci.Overtype;
            else if (n && e.KeyCode == Keys.Return)
            {
                var s = sci.GetSelection();

                if (s.Start != s.End)
                {
                    sci.Copy();
                    sci.ClearSelections();
                    sci.CaretPosition = sci.GetTextLength();
                }
                else
                {
                    var line = GetLine(sci.CurrentLine);
                    history.Add(line.Trim('\0'));
                    var ev = new SubmitEventArgs(line);
                    PrintLine();
                    OnSubmit(ev);
                }
            }
            else if (n && e.KeyCode == Keys.Back && InPos())
                sci.Keyboard.ExecuteCommand(SciCommand.DeleteBack);
            else if (n && e.KeyCode == Keys.Delete && InPosRelax())
                sci.ReplaceText(sci.CurrentPosition, sci.CurrentPosition + 1, String.Empty);
            else if (n && e.KeyCode == Keys.Home && InPos())
                sci.CaretPosition = sci.GetPositionFromLine(sci.LineCount - 1) + lastLen;
            else if (n && (sci.CurrentLine != sci.LineCount - 1 || sci.GetColumnFromPosition(sci.CurrentPosition) < lastLen))
                e.SuppressKeyPress = true;
            else if (n && e.KeyCode == Keys.Escape)
                Replace(String.Empty);
            else if (e.Shift && !e.Alt && !e.Control && e.KeyCode == Keys.Left && InPos())
                sci.Keyboard.ExecuteCommand(SciCommand.CharLeftExtend);
            else if (e.Shift && !e.Alt && !e.Control && e.KeyCode == Keys.Right)
                sci.Keyboard.ExecuteCommand(SciCommand.CharRightExtend);
            else if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Right)
                sci.WordRight();
            else if (e.Control && !e.Alt && !e.Shift && e.KeyCode == Keys.Left && sci.GetColumnFromPosition(sci.CurrentPosition) > lastLen)
                sci.WordLeft();
        }

        private bool InPos()
        {
            return sci.CurrentLine == sci.LineCount - 1
                && sci.GetColumnFromPosition(sci.CurrentPosition) > lastLen;
        }

        private bool InPosLine()
        {
            return sci.CurrentLine == sci.LineCount - 1;
        }

        private bool InPosRelax()
        {
            return sci.CurrentLine == sci.LineCount - 1
                && sci.GetColumnFromPosition(sci.CurrentPosition) >= lastLen;
        }

        public void Clear()
        {
            sci.SetText(String.Empty);
        }

        public void Print(string text)
        {
            AppendText(text);
            sci.CaretPosition = sci.GetTextLength();
            lastLen = sci.GetLine(sci.CurrentLine).Text.Trim('\0').Length;
        }

        public void PrintHeader(string header)
        {
            PrintLine(header);
            PrintLine();
        }

        public void PrintLine()
        {
            PrintLine(String.Empty);
        }

        public void PrintLine(string text)
        {
            AppendText(text);
            sci.Keyboard.ExecuteCommand(SciCommand.NewLine);
            sci.CaretPosition = sci.GetTextLength();
            lastLen = 0;
        }

        private void AppendText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return;

            sci.AppendText(text, true);
        }
        
        private void ReplaceIf(string val)
        {
            if (!String.IsNullOrEmpty(val))
                Replace(val);
        }
        
        private void Replace(string val)
        {
            var pos = sci.GetPositionFromLine(sci.CurrentLine) + lastLen;
            sci.ReplaceText(pos, sci.GetPositionByColumn(sci.CurrentLine, sci.GetLineEndColumn(sci.CurrentLine)), val);
            sci.CaretPosition = sci.GetTextLength();
        }
        
        private bool NoMod(KeyEventArgs e)
        {
            return !e.Alt && !e.Control && !e.Shift;
        }
        
        private string GetLine(int line)
        {
            return sci.GetLine(line).Text.Substring(lastLen);
        }

        internal ScintillaControl GetScintilla()
        {
            return sci;
        }
        
        public event EventHandler<SubmitEventArgs> Submit;
        protected virtual void OnSubmit(SubmitEventArgs e)
        {
            var h = Submit;

            if (h != null)
                h(this, e);
        }

        private int _historySize = HISTORY_SIZE;
        public int HistorySize
        {
            get { return _historySize; }
            set
            {
                _historySize = value;
                history = new HistoryList<String>(value, history);                
            }
        }

        internal HistoryList<String> History
        {
            get { return history; }
            set { history = value; }
        }

        private bool _styling;
        public bool Styling
        {
            get { return _styling; }
            set
            {
                _styling = value;
                sci.RestyleDocument();
            }
        }

        public CaretStyle CaretStyle
        {
            get { return sci.CaretStyle; }
            set { sci.CaretStyle = value; }
        }

        public CaretWidth CaretWidth
        {
            get { return sci.CaretWidth; }
            set { sci.CaretWidth = value; }
        }

        private IApp _app;
        internal IApp App
        {
            get { return _app; }
            set
            {
                _app = value;

                if (_app != null && sci.ContextMenuStrip == null)
                    BuildMenu();
            }
        }
    }
}
