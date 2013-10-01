using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elide.Workbench.ExceptionHandling
{
    public partial class ExceptionDialog : Form
    {
        public ExceptionDialog()
        {
            InitializeComponent();
        }


        public string ExceptionMessage
        {
            get { return messageTextLabel.Text; }
            set { messageTextLabel.Text = value; }
        }


        public string ExceptionDetails
        {
            get { return detailsTextBox.Text; }
            set { detailsTextBox.Text = value; }
        }


        public bool SendMail
        {
            get { return emailCheck.Checked; }
            set { emailCheck.Checked = value; }
        }

        private void ExceptionDialog_Load(object sender, EventArgs e)
        {
            Icon = WB.Form.Icon;
        }
    }
}
