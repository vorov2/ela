using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    public partial class EilGeneratorConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        public EilGeneratorConfigPage()
        {
            InitializeComponent();
        }


        private void LinkerConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            var c = GetConfig();
            offsets.Checked = c.IncludeCodeOffsets;
            debug.Checked = c.GenerateInDebugMode;
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            var c = GetConfig();
            c.IncludeCodeOffsets = offsets.Checked;
            c.GenerateInDebugMode = debug.Checked;
        }

        private EilGeneratorConfig GetConfig()
        {
            return (EilGeneratorConfig)Config;
        }

        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
