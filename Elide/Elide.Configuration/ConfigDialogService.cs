using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.Configuration
{
    public sealed class ConfigDialogService : Service, IConfigDialogService
    {
        public ConfigDialogService()
        {

        }

        public bool ShowConfigDialog()
        {
            var dlg = new ConfigDialog { App = App };
            return dlg.ShowDialog() == DialogResult.OK;
        }
    }
}
