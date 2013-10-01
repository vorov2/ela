using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Scintilla;
using Elide.Scintilla.Printing;

namespace Elide.Workbench
{
    public sealed class PrintService : Service, IPrintService
    {
        public PrintService()
        {

        }

        public bool IsPrintAvailable(Document doc)
        {
            if (doc == null)
                return false;

            var editor = App.GetService<IEditorService>().GetEditor(doc.GetType());
            return editor.Instance.Control is ScintillaControl;
        }

        public void Print(Document doc)
        {
            var sci = App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance.Control as ScintillaControl;
            
            if (sci != null)
                Print(true, new ScintillaPrintDocument(sci, doc.Title));
        }

        public void PageSetup(Document doc)
        {
            var sci = App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance.Control as ScintillaControl;
            
            if (sci != null)
            {
                var printDoc = new ScintillaPrintDocument(sci, doc.Title);

                if (ShowPageSetupDialog(printDoc) == DialogResult.OK)
                    Print(false, printDoc);
            }
        }

        public void PrintPreview(Document doc)
        {
            var sci = App.GetService<IEditorService>().GetEditor(doc.GetType()).Instance.Control as ScintillaControl;
           
            if (sci != null)
                PrintPreview(new ScintillaPrintDocument(sci, doc.Title));
        }

        private bool Print(bool showPrintDialog, ScintillaPrintDocument doc)
        {
            if (showPrintDialog)
            {
                var pd = new PrintDialog();
                pd.Document = doc;
                pd.UseEXDialog = true;
                pd.AllowCurrentPage = true;
                pd.AllowSelection = true;
                pd.AllowSomePages = true;
                pd.PrinterSettings = doc.DefaultPageSettings.PrinterSettings;

                if (pd.ShowDialog(WB.Form) == DialogResult.OK)
                {
                    doc.PrinterSettings = pd.PrinterSettings;
                    doc.Print();
                    return true;
                }

                return false;
            }

            doc.Print();
            return true;
        }

        public DialogResult PrintPreview(ScintillaPrintDocument doc)
        {
            var ppd = new PrintPreviewDialog();
            ppd.WindowState = FormWindowState.Maximized;
            ppd.Icon = WB.Form.Icon;
            ppd.Document = doc;
            return ppd.ShowDialog(WB.Form);
        }

        public DialogResult ShowPageSetupDialog(ScintillaPrintDocument doc)
        {
            var psd = new PageSetupDialog();
            psd.AllowPrinter = true;
            psd.PageSettings = doc.DefaultPageSettings;
            psd.PrinterSettings = doc.DefaultPageSettings.PrinterSettings;
            return psd.ShowDialog(WB.Form);
        }
    }
}
