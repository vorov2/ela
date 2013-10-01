using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Elide.TextWorkbench.Dialogs
{
    public partial class GotoDialog : Form
    {
        public GotoDialog()
        {
            InitializeComponent();
        }

        private void GotoDialog_Load(object sender, EventArgs e)
        {
            label.Text = String.Format(label.Text, Max);
            textBox.Select();
            textBox.SelectAll();
        }

        private void textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsNumber(e.KeyChar) && e.KeyChar != '\b')
            {
                errorProvider.SetError(textBox, "Only numbers are allowed.");
                e.Handled = true;
            }
            else
                errorProvider.SetError(textBox, String.Empty);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                var res = default(Int32);

                if (!Int32.TryParse(textBox.Text, out res))
                {
                    errorProvider.SetError(textBox, "Please specify a textToFind number.");
                    e.Cancel = true;
                }
            }
        }

        public int Max { get; set; }

        public int SelectedNumber
        {
            get { return Int32.Parse(textBox.Text); }
            set { textBox.Text = value.ToString(); }
        }
    }
}
