using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Environment.Views;
using Elide.Forms;

namespace Elide.Workbench.Views
{
    public sealed class OpenFilesView : AbstractView
    {
        private TreeNode mainFolder;
        private TreeNode otherFolder;
        private TreeNode flagFolder;
        private TreeNode lastNodeClick;
        private TreeNode lastNodeSel;
        private OpenFilesControl control;
        private ContextMenuStrip contextMenu;

        public OpenFilesView()
        {

        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Close file", null, (o,e) => CloseDocument(lastNodeClick));
            contextMenu.Items.Add("Toggle document flag", null, (o,e) => ToggleFlag(lastNodeClick));
            MenuRenderer.ApplySkin(contextMenu);

            control = new OpenFilesControl();
            mainFolder = new TreeNode(App.EditorInfo(EditorFlags.Main).DisplayName.Remove("&")) { ImageKey = "Folder", SelectedImageKey = "Folder" };
            otherFolder = new TreeNode("Other Files") { ImageKey = "Folder", SelectedImageKey = "Folder" };
            flagFolder = new TreeNode("Flagged Files") { ImageKey = "Flag", SelectedImageKey = "Flag" };
            control.TreeView.Nodes.Add(mainFolder);
            control.TreeView.Nodes.Add(otherFolder);
            control.TreeView.Nodes.Add(flagFolder);

            App.GetService<IDocumentService>().DocumentAdded += (o,e) => AddDocumentNode(e.Document);
            App.GetService<IDocumentService>().DocumentRemoved += (o,e) => RemoveDocumentNode(e.Document);
            App.GetService<IDocumentService>().ActiveDocumentChanged += (o,e) => Refresh();
            Application.Idle += (o, e) =>
            {
                var sel = App.Document();
                ProcessTitles(sel, mainFolder.Nodes.OfType<TreeNode>());
                ProcessTitles(sel, otherFolder.Nodes.OfType<TreeNode>());
            };
            control.TreeView.NodeMouseClick += (o, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    var d = e.Node.Tag as Document;

                    if (d != null)
                        App.GetService<IDocumentService>().SetActiveDocument(d);
                }
                else if (e.Button == MouseButtons.Right)
                    lastNodeClick = e.Node;
            };
            control.TreeView.GotFocus += (o, e) =>
            {
                var node = FindDocumentNode(App.Document());

                if (node != null)
                {
                    node.EnsureVisible();
                    control.TreeView.SelectedNode = node;
                }
            };
            control.TreeView.DrawNode += TreeView_DrawNode;
            control.TreeView.DrawMode = TreeViewDrawMode.OwnerDrawText;
            control.Load += (o, e) => WalkDocuments();
        }

        private void Refresh()
        {
            var node = FindDocumentNode(App.Document());

            if (node != null)
            {
                node.EnsureVisible();
                control.TreeView.Refresh();
            }
        }

        public override void Activate()
        {
            control.TreeView.Select();   
        }

        private void TreeView_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            var n = e.Node;
            var g = e.Graphics;
            var sel = App.Document();

            var textBrush = default(Brush);
            var backBrush = default(Brush);
            
            if (n.Tag == sel && n.Tag != null)
            {
                if (lastNodeSel != n && lastNodeSel != null)
                {
                    lastNodeSel.Text += " ";
                    lastNodeSel.Text = lastNodeSel.Text.TrimEnd(' ');
                }

                textBrush = UserBrushes.HighlightText;
                backBrush = UserBrushes.Selection;
                lastNodeSel = n;
            }
            else
            {
                textBrush = UserBrushes.Text;
                backBrush = UserBrushes.Window;
            }

            g.FillRectangle(backBrush, e.Bounds);
            g.DrawString(n.Text, control.TreeView.Font, textBrush, e.Bounds.X, e.Bounds.Y + 2);

            if (e.State.Set(TreeNodeStates.Focused) && n.Tag != null)
                ControlPaint.DrawFocusRectangle(g, e.Bounds);
        }

        private void WalkDocuments()
        {
            control.TreeView.BeginUpdate();
            flagFolder.Nodes.Clear();
            mainFolder.Nodes.Clear();
            otherFolder.Nodes.Clear();
            var flagExp = flagFolder.IsExpanded;
            var mainExp = mainFolder.IsExpanded;
            var otherExp = otherFolder.IsExpanded;
            App.GetService<IDocumentService>().EnumerateDocuments().ForEach(d => AddDocumentNode(d));
            if (flagExp) flagFolder.Expand();
            if (mainExp) mainFolder.Expand();
            if (otherExp) otherFolder.Expand();
            control.TreeView.EndUpdate();
        }

        private void AddDocumentNode(Document doc)
        {
            var inf = App.EditorInfo(doc);
            var par = control.FlaggedDocuments.Contains(doc.Title.ToUpper()) ? flagFolder :
                (inf.Flags & EditorFlags.Main) == EditorFlags.Main ? mainFolder : otherFolder;

            if (!control.TreeView.ImageList.Images.ContainsKey(inf.Key))
                control.TreeView.ImageList.Images.Add(inf.Key, inf.Instance.DocumentIcon);

            var tn = new TreeNode(new FileInfo(doc.Title).Name);
            tn.ImageKey = tn.SelectedImageKey = inf.Key;
            tn.Tag = doc;
            doc.FileChanged += (o,e) => tn.Text = new FileInfo(doc.Title).Name;
            par.Nodes.Add(tn);
            tn.ContextMenuStrip = contextMenu;

            if (!par.IsExpanded)
                par.Expand();
        }

        private void ToggleFlag(TreeNode tn)
        {
            if (tn != null && tn.Tag is Document)
            {
                var d = (Document)tn.Tag;
                var tit = d.Title.ToUpper();

                if (control.FlaggedDocuments.Contains(tit))
                    control.FlaggedDocuments.Remove(tit);
                else
                    control.FlaggedDocuments.Add(tit);

                WalkDocuments();
            }
        }

        private void CloseDocument(TreeNode tn)
        {
            if (tn != null && tn.Tag is Document)
                App.GetService<IFileService>().Close((Document)tn.Tag);
        }

        private void RemoveDocumentNode(Document doc)
        {
            var node = FindDocumentNode(doc);

            if (node != null)
                node.Remove();
        }

        private void ProcessTitles(Document sel, IEnumerable<TreeNode> nodes)
        {
            nodes.ForEach(n =>
            {
                var d = (Document)n.Tag;

                if (d.IsDirty && !n.Text.EndsWith("*"))
                    n.Text += "*";
                else if (!d.IsDirty && n.Text.EndsWith("*"))
                    n.Text = n.Text.TrimEnd('*');
            });
        }

        private TreeNode FindDocumentNode(Document doc)
        {
            var node = mainFolder.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Tag == doc);

            if (node != null)
                return node;
            else
            {
                node = otherFolder.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Tag == doc);

                if (node != null)
                    return node;
                else
                {
                    node = flagFolder.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Tag == doc);

                    if (node != null)
                        return node;
                }
            }

            return null;
        }

        public override object Control
        {
            get { return control; }
        }
    }
}
