using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using Elide.Forms;
using Elide.Forms.State;
using Elide.Core;
using Elide.Console;

namespace Elide.ElaCode.Views
{
    public partial class InteractiveControl : StateUserControl
    {
        private InteractiveTextBox cout;
        private SingleBorderPanel panel;
    
        public InteractiveControl()
        {
            InitializeComponent();
        }

        public InteractiveTextBox Cout
        {
            get { return cout; }
        }

        [StateItem]
        public HistoryList<String> History
        {
            get { return cout.History; }
            set { cout.History = value; }
        }

        public IApp App
        {
            get { return cout.App; }
            set { cout.App = value; }
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InteractiveControl));
            this.panel = new Elide.Forms.SingleBorderPanel();
            this.cout = new Elide.ElaCode.Views.InteractiveTextBox();
            this.panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel
            // 
            this.panel.BottomBorder = false;
            this.panel.Controls.Add(this.cout);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.LeftBorder = false;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Padding = new System.Windows.Forms.Padding(0, 1, 0, 0);
            this.panel.RightBorder = false;
            this.panel.Size = new System.Drawing.Size(568, 233);
            this.panel.TabIndex = 1;
            // 
            // cout
            // 
            this.cout.App = null;
            this.cout.CaretStyle = Elide.Scintilla.ObjectModel.CaretStyle.None;
            this.cout.CaretWidth = Elide.Scintilla.ObjectModel.CaretWidth.None;
            this.cout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cout.Header = null;
            this.cout.History = ((Elide.Console.HistoryList<string>)(resources.GetObject("cout.History")));
            this.cout.HistorySize = 100;
            this.cout.Location = new System.Drawing.Point(0, 1);
            this.cout.Name = "cout";
            this.cout.Prompt = null;
            this.cout.ReadOnly = false;
            this.cout.Size = new System.Drawing.Size(568, 232);
            this.cout.Styling = false;
            this.cout.TabIndex = 0;
            this.cout.Text = "interactiveTextBox1";
            // 
            // InteractiveControl
            // 
            this.Controls.Add(this.panel);
            this.Name = "InteractiveControl";
            this.Size = new System.Drawing.Size(568, 233);
            this.panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
