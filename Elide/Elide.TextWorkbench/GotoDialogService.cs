using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.TextWorkbench.Dialogs;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.TextWorkbench
{
    public sealed class GotoDialogService : Service, IGotoDialogService
    {
        public GotoDialogService()
        {
            
        }

        public void ShowGotoDialog()
        {
            var sci = Scintilla;

            if (sci == null)
                return;

            var d = new GotoDialog { Max = sci.LineCount, SelectedNumber = sci.CurrentLine + 1 };

            if (d.ShowDialog((Form)App.GetService<IEnvironmentService>().GetMainWindow()) == DialogResult.OK)
            {
                sci.GotoLine(d.SelectedNumber - 1);
                sci.PutArrow(d.SelectedNumber - 1);
            }
        }

        internal ScintillaControl Scintilla
        {
            get
            {
                var ed = App.Editor();
                return ed != null ? ed.Control as ScintillaControl : null;
            }
        }
    }
}
