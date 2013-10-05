using System;
using Elide.Core;
using Elide.Environment.Editors;
using Elide.Environment.Views;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.ElaCode.Views
{
    public sealed class InteractiveService : Service, IInteractiveService
    {
        public InteractiveService()
        {

        }

        public void EvaluateSelected()
        {
            View().ResetSession();
            var sci = (ScintillaControl)((ITextEditor)((EditorInfo)App.GetService<IEditorService>().GetInfo("editors", "ElaCode")).Instance).Control;
            var sel = sci.HasSelections() ? sci.GetSelection().Text : sci.GetLine(sci.CurrentLine).Text;
            var src = sci.Text + "\r\n_=()\r\n" + sel;
            View().PrintLine();
            
            if (!View().RunCode(src, fastFail: true, onlyErrors: true))
                View().RunCode(sel, fastFail: false, onlyErrors: false);

            App.GetService<IViewService>().OpenView("ElaInteractive");
        }

        public void EvaluateCurrentModule()
        {
            View().ResetSession();
            var sci = (ScintillaControl)((ITextEditor)((EditorInfo)App.GetService<IEditorService>().GetInfo("editors", "ElaCode")).Instance).Control;
            View().PrintLine();
            View().RunCode(sci.Text, fastFail: false, onlyErrors: false);
            App.GetService<IViewService>().OpenView("ElaInteractive");
        }
    
        private InteractiveView View()
        {
            return (InteractiveView)App.GetService<IViewService>().GetView("ElaInteractive");
        }
    }
}
