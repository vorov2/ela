using System;
using System.Windows.Forms;

namespace Elide.Workbench.Views
{
    public partial class AddFilterDialog : Form
    {
        public AddFilterDialog()
        {
            InitializeComponent();
        }

        private void AddFilterDialog_Load(object sender, EventArgs e)
        {
            Icon = WB.Form.Icon;
        }

        public string Filter
        {
            get { return filterTextBox.Text; }
            set { filterTextBox.Text = value; }
        }
    }
}
