using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Forms.State;

namespace Elide.Workbench.Views
{
    public partial class ExplorerControl : StateUserControl
    {
        public ExplorerControl()
        {
            InitializeComponent();
            FilteredFolders = new Dictionary<String,String>();
        }
        
        public void Refresh(bool force)
        {
            if (!force && treeView.SelectedNode != null && treeView.SelectedNode.IsExpanded)
            {
                var n = treeView.SelectedNode;
                treeView.RefreshNode(treeView.SelectedNode);
                n.Expand();
            }
            else
            {
                foreach (TreeNode n in treeView.Nodes)
                    treeView.RefreshNode(n);
            }
        }

        public LazyTreeView TreeView
        {
            get { return treeView; }
        }

        [StateItem]
        public Dictionary<String,String> FilteredFolders { get; set; }
    }
}
