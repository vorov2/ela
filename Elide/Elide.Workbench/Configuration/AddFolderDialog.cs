using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Elide.Workbench.Configuration
{
    public partial class AddFolderDialog : Form
    {
        public AddFolderDialog()
        {
            InitializeComponent();
        }

        public string Mask
        {
            get { return maskTextBox.Text; }
            set { maskTextBox.Text = value; }
        }

        public string FolderName
        {
            get { return nameTextBox.Text; }
            set { nameTextBox.Text = value; }
        }

        public string Path
        {
            get { return dirInputText.Text; }
            set { dirInputText.Text = value; }
        }
        
        private void folder_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog.ShowDialog(ParentForm) == DialogResult.OK)
                dirInputText.Text = folderBrowserDialog.SelectedPath;
        }
        
        private void dirInputText_TextChanged(object sender, EventArgs e)
        {
            errorProvider.SetError(dirInputText, null);
        }

        private void dirInputText_Validating(object sender, CancelEventArgs e)
        {
            try
            {
                var _ = new DirectoryInfo(dirInputText.Text);
                errorProvider.SetError(dirInputText, null);
            }
            catch
            {
                errorProvider.SetError(dirInputText, "Invalid directory path.");
                e.Cancel = true;
            }
        }

        private void AddFolderDialog_Load(object sender, EventArgs e)
        {
            Icon = WB.Form.Icon;
        }
    }
}
