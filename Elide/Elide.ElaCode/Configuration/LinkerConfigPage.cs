using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.ElaCode.Configuration
{
    public partial class LinkerConfigPage : UserControl, IOptionPage
    {
        private bool noevents;
            
        public LinkerConfigPage()
        {
            InitializeComponent();
        }

        private void folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(ParentForm) == DialogResult.OK)
                dirInputText.Text = folderBrowserDialog.SelectedPath;
        }

        private void dirList_SelectedIndexChanged(object sender, EventArgs e)
        {
            remove.Enabled = dirList.SelectedIndex != -1;
        }

        private void dirInputText_TextChanged(object sender, EventArgs e)
        {
            errorProvider.SetError(dirInputText, null);
            add.Enabled = !String.IsNullOrEmpty(dirInputText.Text);
        }

        private void add_Click(object sender, EventArgs e)
        {
            try
            {
                var di = new DirectoryInfo(dirInputText.Text);
                
                if (dirList.Items.IndexOf(di.FullName) == -1)
                    dirList.Items.Add(dirInputText.Text);

                dirInputText.Text = String.Empty;
                PopulateDirs();
                errorProvider.SetError(dirInputText, null);
            }
            catch
            {
                errorProvider.SetError(dirInputText, "Invalid directory path.");
            }
        }

        private void remove_Click(object sender, EventArgs e)
        {
            dirList.Items.RemoveAt(dirList.SelectedIndex);
            PopulateDirs();
        }

        private void LinkerConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            var c = GetConfig();
            recompile.Checked = c.ForceRecompile;
            nowarn.Checked = c.NoWarnings;
            warnAsError.Checked = c.WarningsAsErrors;
            skipCheck.Checked = c.SkipTimeStampCheck;
            lookup.Checked = c.LookupStartupDirectory;

            c.Directories.ForEach(d => dirList.Items.Add(d));
            noevents = false;
        }

        private void PopulateDirs()
        {
            var c = GetConfig();
            c.Directories.Clear();
            dirList.Items.OfType<String>().ForEach(i => c.Directories.Add(i));
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            var c = GetConfig();
            c.ForceRecompile = recompile.Checked;
            c.NoWarnings = nowarn.Checked;
            c.WarningsAsErrors = warnAsError.Checked;
            c.SkipTimeStampCheck = skipCheck.Checked;
            c.LookupStartupDirectory = lookup.Checked;
        }

        private LinkerConfig GetConfig()
        {
            return (LinkerConfig)Config;
        }

        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
