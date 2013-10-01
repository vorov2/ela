using System;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.Workbench.Configuration
{
    public partial class WorkbenchConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        public WorkbenchConfigPage()
        {
            InitializeComponent();
        }

        private void WorkspaceConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            recentFilesCombo.Items.AddRange(new object[] { 5, 10, 15, 20, 30 });
            recentFilesCombo.SelectedItem = Con.RecentFilesMax;
            autosaveCombo.Items.AddRange(new object[] { "1 minute", "2 minutes", "5 minutes", "10 minutes", "15 minutes", "20 minutes" });

            fullPath.Checked = Con.FullFilePath;
            restore.Checked = Con.RememberOpenFiles;
            blank.Checked = Con.StartWithBlank;
            windowHeader.Checked = Con.DocumentTitleInWindowHeader;
            rememberTools.Checked = Con.RememberOpenTools;
            autosave.Checked = Con.EnableAutoSave;
            showWelcome.Checked = Con.ShowWelcomePage;
            SetPeriod(Con.AutoSavePeriod);
            noevents = false;
        }

        private void ControlChanges(object sender, EventArgs e)
        {
            if (noevents)
                return;
            
            Con.RecentFilesMax = (Int32)recentFilesCombo.SelectedItem;
            Con.AutoSavePeriod = GetPeriod();
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            Con.FullFilePath = fullPath.Checked;
            Con.RememberOpenFiles = restore.Checked;
            Con.StartWithBlank = blank.Checked;
            Con.DocumentTitleInWindowHeader = windowHeader.Checked;
            Con.RememberOpenTools = rememberTools.Checked;
            Con.EnableAutoSave = autosave.Checked;
            Con.ShowWelcomePage = showWelcome.Checked;
        }

        private int GetPeriod()
        {
            var min = 1000 * 60;

            switch (autosaveCombo.SelectedIndex)
            {
                case 0: return min;
                case 1: return min * 2;
                case 2: return min * 5;
                case 3: return min * 10;
                case 4: return min * 15;
                default: return min * 20;
            }
        }
        
        private void SetPeriod(int val)
        {
            const int min = 1000 * 60;
            var idx = 0;

            switch (val)
            {
                case min: idx = 0; break;
                case min * 2: idx = 1; break;
                case min * 5: idx = 2; break;
                case min * 10: idx = 3; break;
                case min * 15: idx = 4; break;
                default: idx = 5; break;
            }

            autosaveCombo.SelectedIndex = idx;
        }

        internal WorkbenchConfig Con
        {
            get { return (WorkbenchConfig)Config; }
        }

        public Config Config { get; set; }

        public IApp App { get; set; }
    }
}
