using System;
using System.Windows.Forms;
using Elide.Forms;
using Elide.Forms.State;

namespace Elide.CodeWorkbench.Documentation
{
    public partial class DocControl : StateUserControl
    {
        public DocControl()
        {
            InitializeComponent();
        }

        private void SamplesControl_Load(object sender, EventArgs e)
        {
            
        }

        public TreeView TreeView
        {
            get { return treeView; }
        }
        
        //[StateItem]
        //public int SplitterDistance
        //{
        //    get { return split.SplitterDistance; }
        //    set { split.SplitterDistance = value; }
        //}
    }
}
