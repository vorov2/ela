using System;
using System.Linq;
using System.Windows.Forms;
using Elide.Environment.Configuration;
using Elide.Scintilla.ObjectModel;
using Elide.Core;

namespace Elide.Workbench.Configuration
{
    public partial class OutputConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        private sealed class Value
        {
            public Value(string text, int value)
            {
                Text = text;
                Val = value;
            }

            public override string ToString()
            {
                return Text;
            }

            public int Val;
            public string Text;
        }

        public OutputConfigPage()
        {
            InitializeComponent();
        }

        private void WorkspaceConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            
            selTransparencyCombo.Items.AddRange(new Value[] { new Value("No", 255), new Value("Low", 200), new Value("Average", 150), new Value("High", 100), new Value("Very High", 50), new Value("Transparent", 10) });
            selColorCombo.Populate();
            selBackgroundCombo.Populate();
            selColorCombo.SetShowDefault(false);
            selBackgroundCombo.SetShowDefault(false);

            styling.Checked = Con.Styling;
            selColor.Checked = Con.UseSelectionColor;
            selBackgroundCombo.SelectedColor = Con.SelectionBackColor;
            selColorCombo.SelectedColor = Con.SelectionForeColor;
            selTransparencyCombo.SelectedItem = selTransparencyCombo.Items.OfType<Value>().First(i => i.Val == Con.SelectionTransparency);

            noevents = false;
        }

        private void ControlChanges(object sender, EventArgs e)
        {
            if (noevents)
                return;

            Con.SelectionForeColor = selColorCombo.SelectedColor.Value;
            Con.SelectionBackColor = selBackgroundCombo.SelectedColor.Value;
            Con.SelectionTransparency = ((Value)selTransparencyCombo.SelectedItem).Val;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            selColorLabel.Enabled = selColorCombo.Enabled = selColor.Checked;
            
            if (noevents)
                return;

            Con.Styling = styling.Checked;
            Con.UseSelectionColor = selColor.Checked;
        }

        internal OutputConfig Con
        {
            get { return (OutputConfig)Config; }
        }

        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
