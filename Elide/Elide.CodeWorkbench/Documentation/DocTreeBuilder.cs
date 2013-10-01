using System;
using System.Windows.Forms;
using Elide.CodeWorkbench.Images;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;

namespace Elide.CodeWorkbench.Documentation
{
    internal sealed class DocTreeBuilder
    {
        private readonly static object stub = new Object();
        private readonly TreeView treeView;
        private readonly DocControl control;
        private readonly IApp app;

        internal DocTreeBuilder(IApp app, DocControl control)
        {
            this.app = app;
            this.control = control;
            this.treeView = control.TreeView;
            this.treeView.ImageList.Images.Add("Book", Bitmaps.Load<NS>("Book"));
            this.treeView.ImageList.Images.Add("Article", Bitmaps.Load<NS>("Article"));
            this.treeView.BeforeExpand += BeforeExpand;
            this.treeView.NodeMouseDoubleClick += NodeMouseDoubleClick;
        }

        public void BuildTree(DocFolder root)
        {
            root.Items.ForEach(i => AddNode(i, null));
        }

        private void AddNode(DocItem item, TreeNode parent)
        {
            if (item is DocFolder)
                AddFolderNode((DocFolder)item, parent);
            else if (item is DocFile)
                AddFileNode((DocFile)item, parent);
        }

        private void AddFolderNode(DocFolder folder, TreeNode parent)
        {
            var tn = new TreeNode(folder.Name);
            tn.ImageKey = tn.SelectedImageKey = "Book";
            tn.Tag = folder;
            tn.Nodes.Add(new TreeNode { Tag = stub });

            if (parent != null)
                parent.Nodes.Add(tn);
            else
                treeView.Nodes.Add(tn);
        }

        private void AddFileNode(DocFile file, TreeNode parent)
        {
            var tn = new TreeNode(file.Name);            
            var inf = app.EditorInfo(file.File);
            tn.ImageKey = tn.SelectedImageKey = "Article";
            tn.Tag = file;

            if (parent != null)
                parent.Nodes.Add(tn);
            else
                treeView.Nodes.Add(tn);
        }

        private void BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var tag = e.Node.Tag as DocFolder;

            if (tag != null && e.Node.FirstNode != null && e.Node.FirstNode.Tag == stub)
            {
                treeView.BeginUpdate();
                e.Node.Nodes.Clear();
                tag.Items.ForEach(i => AddNode(i, e.Node));
                treeView.EndUpdate();
            }
        }

        private void NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var cs = e.Node.Tag as DocFile;

            if (cs != null)
                app.GetService<IFileService>().OpenFile(cs.File);
        }
    }
}
