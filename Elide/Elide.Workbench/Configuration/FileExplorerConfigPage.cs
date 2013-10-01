using System;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment.Configuration;
using System.IO;

namespace Elide.Workbench.Configuration
{
    public partial class FileExplorerConfigPage : UserControl, IOptionPage
    {
        private bool noevents;

        public FileExplorerConfigPage()
        {
            InitializeComponent();
        }

        private void PlainTextConfigPage_Load(object sender, EventArgs e)
        {
            noevents = true;
            dirFirst.Checked = Conf().SortDirectoriesFirst;
            sortAsc.Checked = Conf().SortAscending;
            hidden.Checked = Conf().ShowHiddenFilesFolders;
            Conf().Folders.ForEach(f => dirList.Items.Add(f));
            noevents = false;
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            if (noevents)
                return;

            Conf().SortDirectoriesFirst = dirFirst.Checked;
            Conf().SortAscending = sortAsc.Checked;
            Conf().ShowHiddenFilesFolders = hidden.Checked;
        }


        private FileExplorerConfig Conf()
        {
            return (FileExplorerConfig)Config;
        }

        public IApp App { get; set; }

        public Config Config { get; set; }

        private void PopulateDirs()
        {
            var c = Conf();
            c.Folders.Clear();
            dirList.Items.OfType<FileExplorerConfig.FavoriteFolder>()
                .ForEach(i => c.Folders.Add(i));
        }

        private void remove_Click(object sender, EventArgs e)
        {
            dirList.Items.RemoveAt(dirList.SelectedIndex);
            PopulateDirs();
        }

        private void add_Click(object sender, EventArgs e)
        {
            var dlg = new AddFolderDialog();

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                var it = new FileExplorerConfig.FavoriteFolder
                {
                    Directory = dlg.Path,
                    Name = dlg.FolderName,
                    Mask = dlg.Mask
                };
                dirList.Items.Add(it);
                PopulateDirs();
            }
        }

        private void edit_Click(object sender, EventArgs e)
        {
            var selItem = (FileExplorerConfig.FavoriteFolder)dirList.SelectedItem;

            var dlg = new AddFolderDialog
            {
                Path = selItem.Directory,
                FolderName = selItem.Name,
                Mask = selItem.Mask
            };

            if (dlg.ShowDialog(this) == DialogResult.OK)
            {
                selItem.Directory = dlg.Path;
                selItem.Name = dlg.FolderName;
                selItem.Mask = dlg.Mask;
                PopulateDirs();
            }
        }

        private void dirList_SelectedIndexChanged(object sender, EventArgs e)
        {
            edit.Enabled = remove.Enabled = dirList.SelectedIndex != -1;
        }
    }
}
