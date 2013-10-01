using System;
using System.Windows.Forms;
using Elide.CodeWorkbench.Images;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;

namespace Elide.CodeWorkbench.CodeSamples
{
    internal sealed class CodeSampleTreeBuilder
    {
        private readonly static object stub = new Object();
        private readonly TreeView treeView;
        private readonly SamplesControl control;
        private readonly IApp app;

        internal CodeSampleTreeBuilder(IApp app, SamplesControl control)
        {
            this.app = app;
            this.control = control;
            this.treeView = control.TreeView;
            this.treeView.ImageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            this.treeView.BeforeExpand += BeforeExpand;
            this.treeView.NodeMouseClick += NodeMouseClick;
            this.treeView.NodeMouseDoubleClick += NodeMouseDoubleClick;
        }

        public void BuildTree(CodeSampleFolder root)
        {
            root.Items.ForEach(i => AddNode(i, null));
        }

        private void AddNode(CodeSampleItem item, TreeNode parent)
        {
            if (item is CodeSampleFolder)
                AddFolderNode((CodeSampleFolder)item, parent);
            else if (item is CodeSampleFile)
                AddFileNode((CodeSampleFile)item, parent);
        }

        private void AddFolderNode(CodeSampleFolder folder, TreeNode parent)
        {
            var tn = new TreeNode(folder.Name);
            tn.ImageKey = tn.SelectedImageKey = "Folder";
            tn.Tag = folder;
            tn.Nodes.Add(new TreeNode { Tag = stub });

            if (parent != null)
                parent.Nodes.Add(tn);
            else
                treeView.Nodes.Add(tn);
        }

        private void AddFileNode(CodeSampleFile file, TreeNode parent)
        {
            var tn = new TreeNode(file.Name);            
            var inf = app.EditorInfo(file.File);

            if (!treeView.ImageList.Images.ContainsKey(inf.Key))
                treeView.ImageList.Images.Add(inf.Key, inf.Instance.DocumentIcon);

            tn.ImageKey = tn.SelectedImageKey = inf.Key;
            tn.Tag = file;

            if (parent != null)
                parent.Nodes.Add(tn);
            else
                treeView.Nodes.Add(tn);
        }

        private void BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var tag = e.Node.Tag as CodeSampleFolder;

            if (tag != null && e.Node.FirstNode != null && e.Node.FirstNode.Tag == stub)
            {
                treeView.BeginUpdate();
                e.Node.Nodes.Clear();
                tag.Items.ForEach(i => AddNode(i, e.Node));
                treeView.EndUpdate();
            }
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var cs = e.Node.Tag as CodeSampleItem;

            if (cs != null)
                control.Description = cs.Description;       
        }

        private void NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var cs = e.Node.Tag as CodeSampleFile;

            if (cs != null)
                app.GetService<IFileService>().OpenFile(cs.File);
        }
    }
}
