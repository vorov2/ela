using System;
using System.Windows.Forms;
using Elide.Forms;
using Elide.Forms.State;

namespace Elide.CodeWorkbench.CodeSamples
{
    public partial class SamplesControl : StateUserControl
    {
        public SamplesControl()
        {
            InitializeComponent();
        }

        private void SamplesControl_Load(object sender, EventArgs e)
        {
            split.BackColor = UserColors.Border;
            split.SplitterWidth = 1;
        }

        public TreeView TreeView
        {
            get { return treeView; }
        }

        public string Description
        {
            get { return desc.Text; }
            set { desc.Text = value; }
        }

        //[StateItem]
        //public int SplitterDistance
        //{
        //    get { return split.SplitterDistance; }
        //    set { split.SplitterDistance = value; }
        //}
    }
}
