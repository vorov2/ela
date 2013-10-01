using System;
using System.ComponentModel;
using System.Windows.Forms;
using Elide.Scintilla.Printing;
using Elide.Scintilla;

namespace Elide.Environment
{
    internal sealed class PrintHelper
    {
        private IWin32Window window;

        internal PrintHelper(IWin32Window window, ScintillaPrintDocument printDocument)
        {
            this.window = window;
            PrintDocument = printDocument;
        }

        public bool Print()
        {
            return Print(true);
        }

        public bool Print(bool showPrintDialog)
        {
            if (showPrintDialog)
            {
                var pd = new PrintDialog();
                pd.Document = PrintDocument;
                pd.UseEXDialog = true;
                pd.AllowCurrentPage = true;
                pd.AllowSelection = true;
                pd.AllowSomePages = true;
                pd.PrinterSettings = PageSettings.PrinterSettings;

                if (pd.ShowDialog(window) == DialogResult.OK)
                {
                    PrintDocument.PrinterSettings = pd.PrinterSettings;
                    PrintDocument.Print();
                    return true;
                }

                return false;
            }

            PrintDocument.Print();
            return true;
        }
        
        public DialogResult PrintPreview()
        {
            var ppd = new PrintPreviewDialog();
            ppd.WindowState = FormWindowState.Maximized;
            ppd.Icon = ((Form)window).Icon;
            ppd.Document = PrintDocument;
            return ppd.ShowDialog(window);
        }
        
        internal bool ShouldSerialize()
        {
            return ShouldSerializePageSettings() || ShouldSerializePrintDocument();
        }
        
        private bool ShouldSerializePageSettings()
        {
            return PrintDocument.ShouldSerialize();
        }
        
        private bool ShouldSerializePrintDocument()
        {
            return PrintDocument.ShouldSerialize();
        }

        public DialogResult ShowPageSetupDialog()
        {
            var psd = new PageSetupDialog();
            psd.AllowPrinter = true;
            psd.PageSettings = PageSettings;
            psd.PrinterSettings = PageSettings.PrinterSettings;
            return psd.ShowDialog(window);
        }

        public ScintillaPageSettings PageSettings
        {
            get { return (ScintillaPageSettings)PrintDocument.DefaultPageSettings; }
            set { PrintDocument.DefaultPageSettings = value; }
        }

        public ScintillaPrintDocument PrintDocument { get; private set; }
    }
}
