using Elide.Console;
using Elide.Environment;
using Elide.Core;
using Elide.Scintilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Elide.ElaCode.Views
{
    public sealed class InteractiveTextBox : ConsoleTextBox
    {
        private Thread execThread;
        
        public InteractiveTextBox()
        {

        }

        protected override void Lex(object sender, StyleNeededEventArgs e)
        {
            var sci = GetScintilla();
            e.AddStyleItem(sci.GetPositionFromLine(0) - e.StartPosition, sci.GetLineEndColumn(0), TextStyle.Style1);

            var count = sci.LineCount;
            for (var i = 0; i < count; i++)
            {
                var p = sci.GetPositionFromLine(i);
                var c = sci.CharAt(p);

                if (c == '`')
                {
                    var ln = sci.GetLine(i);
                    var txt = ln.Text;
                    var style = TextStyle.None;

                    if (txt.StartsWith("``ela>"))
                    {
                        e.AddStyleItem(ln.StartPosition - e.StartPosition, 2, TextStyle.Invisible);
                        e.AddStyleItem(ln.StartPosition - e.StartPosition + 2, 4, TextStyle.Style5);
                        e.AddStyleItem(ln.StartPosition - e.StartPosition + 6, ln.EndPosition - ln.StartPosition - 6, TextStyle.Style6);
                    }
                    else
                    {
                        if (txt.StartsWith("``!!!"))
                            style = TextStyle.Style2;
                        else if (txt.StartsWith("``|||"))
                            style = TextStyle.Style3;
                        else if (txt.StartsWith("``???"))
                            style = TextStyle.Style4;

                        if (style != TextStyle.None)
                        {
                            e.AddStyleItem(ln.StartPosition - e.StartPosition, 5, TextStyle.Invisible);
                            e.AddStyleItem(ln.StartPosition - e.StartPosition + 5, ln.EndPosition - ln.StartPosition - 5, style);
                        }
                    }
                }
            }
        }

        protected override ContextMenuStrip BuildMenu(IMenuBuilder<ContextMenuStrip> builder)
        {
            var sci = GetScintilla();
            return builder
                .Item("Cut", "Ctrl+X", Cut, sci.HasSelections)
                .Item("Copy", "Ctrl+C", sci.Copy, sci.HasSelections)
                .Item("Paste", "Ctrl+V", Paste, sci.CanPaste)
                .Separator()
                .Item("Clear All", ClearAll, () => sci.GetTextLength() > GetLastLength())
                .Separator()
                .Item("Stop Execution", StopExecution, () => execThread != null && execThread.IsAlive)
                .Item("Reset Session", ResetSession)
                .Separator()
                .Item("Search", "Ctrl+F", Search, () => sci.GetTextLength() > 0)
                .Finish();
        }

        public void ClearAll()
        {
            if (!InputDisabled)
            {
                base.SmartClear();
                PrintBanner();
            }
        }

        private void PrintBanner()
        {
            if (!InputDisabled)
            {
                PrintHeader(Header);
                Print(Prompt + ">");
            }
        }

        private void StopExecution()
        {
            if (execThread != null)
            {
                try
                {
                    InputDisabled = true;
                    var t = execThread;

                    if (t != null && t.IsAlive)
                        execThread.Abort();
                    else
                        return;
                }
                catch { }
                finally
                {
                    InputDisabled = false;
                }

                PrintLine();
                PrintLine("``!!!Execution aborted.");
                PrintLine();
                ReadOnly = false;
                ScrollToCaret();
                App.GetService<IStatusBarService>().ClearStatusString();
                execThread = null;
            }
        }
        
        internal void SetExecControl(Thread execThread)
        {
            this.execThread = execThread;
        }

        private void ResetSession()
        {
            ClearAll();
            OnSessionReset(EventArgs.Empty);
        }

        public event EventHandler SessionReset;
        private void OnSessionReset(EventArgs e)
        {
            var h = SessionReset;

            if (h != null)
                h(this, e);
        }

        public string Header { get; set; }
        public string Prompt { get; set; }
    }
}
