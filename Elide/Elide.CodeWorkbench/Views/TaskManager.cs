using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.CodeEditor;
using Elide.CodeEditor.Views;
using Elide.CodeWorkbench.Images;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.CodeWorkbench.Views
{
    internal sealed class TaskManager
    {
        private TreeView treeView;
        private Dictionary<ScintillaControl,Object> sciMap;
        private Dictionary<CodeDocument,TreeNode> nodeMap;
        private IApp app;
        private TaskListService service;
        private static readonly object stub = new Object();

        public TaskManager(IApp app, TaskListService service)
        {
            this.app = app;
            this.service = service;

            this.sciMap = new Dictionary<ScintillaControl,Object>();
            this.nodeMap = new Dictionary<CodeDocument,TreeNode>();
            this.treeView = new BufferedTreeView();
            this.treeView.Font = Fonts.Text;
            this.treeView.BorderStyle = BorderStyle.None;
            this.treeView.ShowLines = false;
            this.treeView.BeforeExpand += TreeViewBeforeExpand;
            this.treeView.NodeMouseClick += NodeMouseClick;

            var img = new ImageList();
            img.ColorDepth = ColorDepth.Depth32Bit;
            img.TransparentColor = Color.Magenta;
            img.ImageSize = new Size(16, 16);
            img.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            img.Images.Add("Task", Bitmaps.Load<NS>("Task"));
            treeView.ImageList = img;

            var srv = app.GetService<IDocumentService>();
            srv.EnumerateDocuments().ForEach(d => AddDocument(d as CodeDocument));
            srv.DocumentAdded += DocumentAdded;
            srv.DocumentRemoved += DocumentRemoved;
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Tag is TaskItem)
            {
                var ti = (TaskItem)e.Node.Tag;
                var doc = (CodeDocument)e.Node.Parent.Tag;
                var svc = app.GetService<IDocumentNavigatorService>();
                svc.Navigate(doc, ti.Position, 0, true);
            }
        }

        private void TreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            e.Node.Nodes.Clear();
            var doc = e.Node.Tag as CodeDocument;

            if (doc != null)
            {
                var sciDoc = doc.GetSciDocument();

                using (var sci = new ScintillaControl())
                {
                    sci.AttachDocument(sciDoc);
                    var tp = service.GetTaskProvider(doc);

                    if (tp != null)
                    {
                        var tasks = tp.GetTasks(doc);
                        tasks.ForEach(t => AddTask(sci, e.Node, t));
                    }
                }
            }
        }

        private void DocumentRemoved(object sender, DocumentEventArgs e)
        {
            var node = GetNode(e.Document);

            if (node != null)
                treeView.Nodes.Remove(node);
        }

        private void DocumentAdded(object sender, DocumentEventArgs e)
        {
            var codeDoc = e.Document as CodeDocument;
            AddDocument(codeDoc);
        }

        private void AddDocument(CodeDocument codeDoc)
        {
            if (codeDoc != null && codeDoc.Features.Set(CodeEditorFeatures.Tasks))
            {
                var editor = app.Editor(codeDoc.GetType());
                var sci = editor.Control as ScintillaControl;

                if (sci != null)
                {
                    var node = new TreeNode();
                    codeDoc.FileChanged += (o,ev) => ChangeNodeText(node);
                    node.ImageKey = node.SelectedImageKey = "Folder";
                    node.Tag = codeDoc;
                    node.Nodes.Add(new TreeNode { Tag = stub });
                    ChangeNodeText(node);
                    nodeMap.Add(codeDoc, node);
                    treeView.Nodes.Add(node);

                    if (!sciMap.ContainsKey(sci))
                        sci.TextChanged += (o, ev) => ContentChanged();
                }
            }
        }

        private void ChangeNodeText(TreeNode node)
        {
            var fi = new FileInfo(((Document)node.Tag).Title);
            node.Text = fi.Name;
        }

        private void ContentChanged()
        {
            var doc = app.Document<CodeDocument>();
            var node = default(TreeNode);

            if (doc != null && nodeMap.TryGetValue(doc, out node) && (node.IsExpanded || node.Nodes.Count == 0))
            {
                if (node.Nodes.Count == 0)
                    node.Nodes.Add(new TreeNode { Tag = stub });

                node.Collapse();
            }
        }

        private TreeNode GetNode(Document doc)
        {
            return treeView.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Tag == doc);
        }

        private void AddTask(ScintillaControl sci, TreeNode parent, TaskItem task)
        {
            var txt = sci.GetTextRangeUnicode(task.Position, task.Position + task.Length).Trim(':', '-', '.', ' ', '\r', '\n');
            txt = !String.IsNullOrEmpty(txt) && txt.Length > 30 ? txt.Substring(0, 30) + "..." : txt;
            var tn = new TreeNode(String.Format("{0}: {1} ({2},{3})", task.Type.ToString().ToUpper(), txt, 
                sci.GetLineFromPosition(task.Position) + 1, sci.GetColumnFromPosition(task.Position) + 1));
            tn.ImageKey = tn.SelectedImageKey = "Task";
            tn.Tag = task;
            parent.Nodes.Add(tn);
        }

        public TreeView TreeView
        {
            get { return treeView; }
        }
    }
}
