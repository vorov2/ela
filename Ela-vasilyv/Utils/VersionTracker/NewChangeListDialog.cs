using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace VersionTracker
{
    public partial class NewChangeListDialog : Form
    {
        public NewChangeListDialog()
        {
            InitializeComponent();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                fileTextBox.Text = saveFileDialog.FileName;
            }
        }

        public string FileName
        {
            get { return fileTextBox.Text; }
        }


        public string Version
        {
            get { return versionTextBox.Text; }
        }

		private void NewChangeListDialog_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (String.IsNullOrEmpty(versionTextBox.Text))
			{
				errorProvider.SetError(versionTextBox, "Please specify an initial version.");
				e.Cancel = true;
			}
			else if (!IsValidVersion(versionTextBox.Text))
			{
				errorProvider.SetError(versionTextBox, "Please enter a valid version that contains four revisions.");
				e.Cancel = true;
			}
			else
				errorProvider.SetError(versionTextBox, null);

			if (String.IsNullOrEmpty(fileTextBox.Text))
			{
				errorProvider.SetError(fileTextBox, "Please specify a full path to the file.");
				e.Cancel = true;
			}   
			else if (fileTextBox.Text.IndexOfAny(Path.GetInvalidPathChars()) != -1)
			{
				errorProvider.SetError(fileTextBox, "Please enter a valid path to the file.");
				e.Cancel = true;
			}
			else
				errorProvider.SetError(fileTextBox, null);
		}


		private bool IsValidVersion(string val)
		{
			var arr = val.Split('.');

			if (arr.Length == 4)
			{
				foreach (var s in arr)
					foreach (var c in s)
						if (!Char.IsNumber(c))
							return false;

				return true;
			}

			return false;
		}
    }
}
