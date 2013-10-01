using System;
using System.Windows.Forms;
using Elide.Forms.State;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;

namespace Elide.Workbench.Views
{
    public partial class OutputControl : StateUserControl
    {
        private ScintillaControl sci;

        public OutputControl()
        {
            InitializeComponent();
            
            sci = new ScintillaControl();
            sci.Dock = DockStyle.Fill;
            sci.MarginVisible = false;
            sci.ViewWhiteSpace = false;
            sci.IndentationGuides = false;
            sci.UseTabs = false;
            sci.AttachDocument(sci.CreateDocument());
            sci.ReadOnly = true;
            sci.UseUnicodeLexing = true;
            panel.Controls.Add(sci);
        }

        internal ScintillaControl Scintilla
        {
            get { return sci; }
        }
        
        [StateItem]
        public bool WordWrap
        {
            get { return sci.WordWrapMode != WordWrapMode.None; }
            set { sci.WordWrapMode = value ? WordWrapMode.Char : WordWrapMode.None; }
        }
    }
}
