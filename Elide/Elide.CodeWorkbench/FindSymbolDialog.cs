using Elide.Forms.State;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Elide.CodeWorkbench
{
    public partial class FindSymbolDialog : Form
    {
        public FindSymbolDialog()
        {
            InitializeComponent();
        }

        [StateItem]
        public string Symbol
        {
            get { return textBox.Text; }
            set 
            { 
                if (String.IsNullOrWhiteSpace(textBox.Text))
                    textBox.Text = value; 
            }
        }
        
        [StateItem]
        public bool AllFiles
        {
            get { return allFiles.Checked; }
            set { allFiles.Checked = value; }
        }
    }
}
