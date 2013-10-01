using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    public partial class CompilerConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        public CompilerConfigPage()
        {
            InitializeComponent();
        }

        private void CompilerConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            var c = GetConfig();
            debug.Checked = c.GenerateDebugInfo;
            noWarn.Checked = c.NoWarnings;
            warnAsError.Checked = c.WarningsAsErrors;
            hints.Checked = c.ShowHints;
            optimize.Checked = c.Optimize;
            libBox.Text = c.Prelude;
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            var c = GetConfig();
            c.GenerateDebugInfo = debug.Checked;
            c.NoWarnings = noWarn.Checked;
            c.WarningsAsErrors = warnAsError.Checked;
            c.ShowHints = hints.Checked;
            c.Optimize = optimize.Checked;
        }

        private void libBox_TextChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            GetConfig().Prelude = libBox.Text;
        }

        private CompilerConfig GetConfig()
        {
            return (CompilerConfig)Config;
        }
        
        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
