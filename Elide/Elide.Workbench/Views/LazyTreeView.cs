using System;
using System.Windows.Forms;
using Elide.Forms;

namespace Elide.Workbench.Views
{
    public class LazyTreeView : BufferedTreeView
    {
        private static readonly object dummy = new Object();

        public LazyTreeView()
        {

        }
        
        public void AddLazyNode(TreeNode node)
        {
            AddLazyNode(null, node);
        }
        
        public void AddLazyNode(TreeNode parent, TreeNode node)
        {
            node.Nodes.Add(GetLoadingNode());

            if (parent == null)
                Nodes.Add(node);
            else
                parent.Nodes.Add(node);

            node.Collapse();
        }
        
        public void RefreshNode(TreeNode node)
        {
            node.Nodes.Clear();
            node.Nodes.Add(GetLoadingNode());
            node.Collapse();
        }
        
        private TreeNode GetLoadingNode()
        {
            return new TreeNode("Loading...") { Tag = dummy };
        }
        
        protected override void OnBeforeExpand(TreeViewCancelEventArgs e)
        {
            var n = e.Node;

            if (n.FirstNode != null && n.FirstNode.Tag == dummy)
            {
                n.Nodes.Clear();
                OnNodesNeeded(new TreeViewEventArgs(n));
            }

            base.OnBeforeExpand(e);
        }

        public event TreeViewEventHandler NodesNeeded;
        private void OnNodesNeeded(TreeViewEventArgs e)
        {
            var h = NodesNeeded;

            if (h != null)
                h(this, e);
        }
    }
}
