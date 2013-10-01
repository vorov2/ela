using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;
using Elide.TextWorkbench.Images;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
	internal sealed class BookmarkManager
	{
		private TreeView treeView;
        private TreeNode lastNodeClick;
        private Dictionary<ScintillaControl, Object> sciMap;
		private Dictionary<Document,TreeNode> nodeMap;
		private IApp app;
        private ContextMenuStrip contextMenu;
        private static readonly object stub = new Object();
		
		public BookmarkManager()
		{
			
		}
        		
		internal void Initialize(IApp app)
		{
			this.app = app;
            contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Remove Bookmark", null, (o, e) => RemoveBookmark(lastNodeClick));
            MenuRenderer.ApplySkin(contextMenu);

            this.sciMap = new Dictionary<ScintillaControl,Object>();
			this.nodeMap = new Dictionary<Document,TreeNode>();
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
			img.Images.Add("Bookmark", Bitmaps.Load<NS>("Bookmark"));
			treeView.ImageList = img;

			var srv = app.GetService<IDocumentService>();
            srv.EnumerateDocuments().ForEach(d => AddDocument(d as TextDocument));
			srv.DocumentAdded += DocumentAdded;
			srv.DocumentRemoved += DocumentRemoved;

            
		}

        private void RemoveBookmark(TreeNode n)
        {
            if (n != null && n.Tag is Int32)
            {
                var i = (Int32)n.Tag;
                var doc = n.Parent.Tag as TextDocument;

                if (doc != null)
                {
                    var sciDoc = doc.GetSciDocument();

                    using (var sciTemp = new ScintillaControl())
                    {
                        sciTemp.AttachDocument(sciDoc);
                        sciTemp.RemoveBookmark(i);
                    }

                    treeView.Nodes.Remove(n);
                }
            }
        }

        private void NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.Node.Tag is Int32)
            {
                var line = (Int32)e.Node.Tag;
                var doc = (Document)e.Node.Parent.Tag;
                var svc = app.GetService<IDocumentNavigatorService>();
                svc.Navigate(doc, line, 0, 0, false);
            }
            else if (e.Button == MouseButtons.Right)
                lastNodeClick = e.Node;
        }

		private void TreeViewBeforeExpand(object sender, TreeViewCancelEventArgs e)
		{
			e.Node.Nodes.Clear();

            var doc = e.Node.Tag as TextDocument;

            if (doc != null)
            {
                var sciDoc = doc.GetSciDocument();

                using (var sci = new ScintillaControl())
                {
                    sci.AttachDocument(sciDoc);
                    sci.GetAllBookmarks().ForEach(l => AddBookmark(sci, e.Node, l));
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
			var txtDoc = e.Document as TextDocument;
            AddDocument(txtDoc);			
		}

        private void AddDocument(TextDocument txtDoc)
        {
            if (txtDoc != null)
            {
                var editor = app.Editor(txtDoc.GetType());
                var sci = editor.Control as ScintillaControl;

                if (sci != null)
                {
                    var node = new TreeNode();
                    txtDoc.FileChanged += (o, ev) => ChangeNodeText(node);
                    node.ImageKey = node.SelectedImageKey = "Folder";
                    node.Tag = txtDoc;
                    node.Nodes.Add(new TreeNode { Tag = stub });
                    ChangeNodeText(node);
                    nodeMap.Add(txtDoc, node);
                    treeView.Nodes.Add(node);

                    if (!sciMap.ContainsKey(sci))
                    {
                        sci.TextChanged += (o, ev) => ContentChanged();
                        sci.BookmarkAdded += (o, ev) => ContentChanged();
                        sci.BookmarkRemoved += (o, ev) => ContentChanged();
                    }
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
			var doc = app.GetService<IDocumentService>().GetActiveDocument();
			var node = default(TreeNode);
			
			if (nodeMap.TryGetValue(doc, out node) && (node.IsExpanded || node.Nodes.Count  == 0))
			{
				if (node.Nodes.Count == 0)
					node.Nodes.Add(new TreeNode { Tag = stub });
				
				node.Collapse();
			}
		}

		private void BookmarkRemoved(object sender, BookmarkEventArgs e)
		{
			var par = GetNode(app.Document());
			
			if (par != null)
			{
				var cn = par.Nodes.OfType<TreeNode>().FirstOrDefault(n => (Int32)n.Tag == e.Line);
				
				if (cn != null)
					par.Nodes.Remove(cn);
			}
		}
		
		private TreeNode GetNode(Document doc)
		{
			return treeView.Nodes.OfType<TreeNode>().FirstOrDefault(n => n.Tag == doc);
		}
		
		private void AddBookmark(ScintillaControl sci, TreeNode parent, int line)
		{
			var txt = sci.GetLine(line).Text.TrimStart(' ', '\t');
            txt = !String.IsNullOrEmpty(txt) && txt.Length > 30 ? txt.Substring(0, 30) + "..." : txt;
			var tn = new TreeNode(String.Format("Line {0}: {1}", (line + 1), txt));
			tn.ImageKey = tn.SelectedImageKey = "Bookmark";
			tn.Tag = line;
            parent.Nodes.Add(tn);
            tn.ContextMenuStrip = contextMenu;
		}
		
		public TreeView TreeView
		{
			get { return treeView; }
		}
	}
}
