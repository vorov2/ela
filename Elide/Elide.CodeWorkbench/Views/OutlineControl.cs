using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Elide.Forms;
using Elide.CodeWorkbench.Images;
using Elide.Scintilla;
using Elide.Core;
using System.IO;
using Elide.CodeEditor;
using Elide.CodeEditor.Infrastructure;
using Elide.Environment;
using Elide.TextEditor;
using Ela.CodeModel;

namespace Elide.CodeWorkbench.Views
{
    public partial class OutlineControl : UserControl
    {
        public OutlineControl()
        {
            InitializeComponent();
            treeView.Font = Fonts.ControlText;
            var imageList = new ImageList();
            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(16, 16);
            imageList.TransparentColor = Color.Magenta;

            imageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            imageList.Images.Add("References", Bitmaps.Load<NS>("References"));
            imageList.Images.Add("Reference", Bitmaps.Load<NS>("Reference"));
            imageList.Images.Add("Variable", Bitmaps.Load<NS>("Variable"));
            imageList.Images.Add("PrivateVariable", Bitmaps.Load<NS>("PrivateVariable"));
            imageList.Images.Add("Module", Bitmaps.Load<NS>("Module"));
            imageList.Images.Add("Interface", Bitmaps.Load<NS>("Interface"));
            imageList.Images.Add("Type", Bitmaps.Load<NS>("Type"));
            imageList.Images.Add("Instance", Bitmaps.Load<NS>("Instance"));
            imageList.Images.Add("Member", Bitmaps.Load<NS>("Member"));
            treeView.ImageList = imageList;
        }

        public void Clear()
        {
            treeView.BeginUpdate();
            treeView.Nodes.Clear();            
            treeView.EndUpdate();
        }

        private void treeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is ILocationBounded)
            {
                var doc = default(CodeDocument);
                var n = e.Node.Parent;

                while (doc == null && n != null)
                {
                    doc = n.Tag as CodeDocument;
                    n = n.Parent;
                }
                
                App.GetService<IDocumentService>().SetActiveDocument(doc);

                var lb = (ILocationBounded)e.Node.Tag;
                App.GetService<IDocumentNavigatorService>().Navigate(doc, lb.Location.Line - 1, lb.Location.Column - 1, 0, true);
            }
        }

        private void treeView_BeforeExpand(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;

            if (node.Text == "Bindings")
            {
                var doc = (CodeDocument)node.Parent.Tag;

                treeView.BeginUpdate();
                node.Nodes.Clear();

                if (doc.Unit != null)
                {
                    foreach (var v in doc.Unit.Globals)
                    {
                        var flags = (ElaVariableFlags)v.Flags;

                        if (!flags.Set(ElaVariableFlags.ClassFun))
                        {
                            var tn = new TreeNode(v.Name)
                            {
                                ImageKey = flags.Set(ElaVariableFlags.Private) ? "PrivateVariable" : "Variable",
                                SelectedImageKey = "Variable"
                            };
                            tn.Tag = v;
                            node.Nodes.Add(tn);
                        }
                    }
                }

                treeView.EndUpdate();
            }
            else if (node.Text == "Classes")
            {
                var doc = (CodeDocument)node.Parent.Tag;

                treeView.BeginUpdate();
                node.Nodes.Clear();

                if (doc.Unit != null)
                {
                    foreach (var v in doc.Unit.Classes)
                    {
                        var tn = new TreeNode(v.Name) { ImageKey = "Interface", SelectedImageKey = "Interface" };
                        tn.Tag = v;
                        node.Nodes.Add(tn);

                        foreach (var m in v.Members)
                        {
                            var cn = new TreeNode(m.ToString()) { ImageKey = "Member", SelectedImageKey = "Member" };
                            cn.Tag = doc.Unit.Globals.FirstOrDefault(c => c.Name == m.Name);                            
                            tn.Nodes.Add(cn);
                        }
                    }
                }

                treeView.EndUpdate();
            }
            else if (node.Text == "Instances")
            {
                var doc = (CodeDocument)node.Parent.Tag;

                treeView.BeginUpdate();
                node.Nodes.Clear();

                if (doc.Unit != null)
                {
                    foreach (var v in doc.Unit.Instances)
                    {
                        var tn = new TreeNode(v.Class + " " + v.Type) { ImageKey = "Instance", SelectedImageKey = "Instance" };
                        tn.Tag = v;
                        node.Nodes.Add(tn);
                    }
                }

                treeView.EndUpdate();
            }
            else if (node.Text == "Types")
            {
                var doc = (CodeDocument)node.Parent.Tag;

                treeView.BeginUpdate();
                node.Nodes.Clear();

                if (doc.Unit != null)
                {
                    foreach (var v in doc.Unit.Types)
                    {
                        var tn = new TreeNode(v.Name) { ImageKey = "Type", SelectedImageKey = "Type" };
                        tn.Tag = v;
                        node.Nodes.Add(tn);
                    }
                }

                treeView.EndUpdate();
            }
            else if (node.Text == "References")
            {
                var doc = (CodeDocument)node.Parent.Tag;

                treeView.BeginUpdate();
                node.Nodes.Clear();

                if (doc.Unit != null)
                {
                    foreach (var mr in doc.Unit.References)
                    {
                        var tn = new TreeNode(mr.ToString()) { ImageKey = "Reference", SelectedImageKey = "Reference" };
                        tn.Tag = mr;
                        tn.Nodes.Add(new TreeNode());
                        node.Nodes.Add(tn);
                    }
                }

                treeView.EndUpdate();
            }

            else if (node.Tag is IReference)
            {
                treeView.BeginUpdate();
                node.Nodes.Clear();

                var unit = App.GetService<IReferenceResolverService>().Resolve((IReference)node.Tag);
                
                if (unit != null)
                {
                    foreach (var v in unit.Globals)
                    {
                        var tn = new TreeNode(v.Name) { ImageKey = "Variable", SelectedImageKey = "Variable" };
                        node.Nodes.Add(tn);
                    }
                }

                treeView.EndUpdate();
            }
        }

        public TreeNode AddDocumentNode(CodeDocument doc)
        {
            var tn = new TreeNode(doc.FileInfo != null ? doc.FileInfo.ShortName() : new FileInfo(doc.Title).ShortName());
            tn.ImageKey = "Module";
            tn.SelectedImageKey = "Module";
            tn.Tag = doc;
            AddDocumentNodes(tn);
            treeView.Nodes.Add(tn);
            return tn;
        }

        public void RemoveDocumentNode(CodeDocument doc)
        {
            var node = treeView.Nodes.OfType<TreeNode>().FirstOrDefault(tn => tn.Tag == doc);

            if (node != null)
            {
                node.Tag = null;
                treeView.Nodes.Remove(node);
            }
        }

        public void CollapseNode(CodeDocument doc)
        {
            var node = treeView.Nodes.OfType<TreeNode>().FirstOrDefault(tn => tn.Tag == doc);

            if (node != null)
            {
                if (node.IsExpanded)
                {
                    node.Nodes.Clear();
                    AddDocumentNodes(node);
                }
            }
        }

        private void AddDocumentNodes(TreeNode tn)
        {
            var refs = new TreeNode("References");
            refs.ImageKey = "References"; 
            refs.SelectedImageKey = "References";
            refs.Nodes.Add(new TreeNode());
            tn.Nodes.Add(refs);

            var binds = new TreeNode("Bindings");
            binds.ImageKey = "Folder";
            binds.SelectedImageKey = "Folder";
            binds.Nodes.Add(new TreeNode());
            tn.Nodes.Add(binds);

            var classes = new TreeNode("Classes");
            classes.ImageKey = "Folder";
            classes.SelectedImageKey = "Folder";
            classes.Nodes.Add(new TreeNode());
            tn.Nodes.Add(classes);

            var inst = new TreeNode("Instances");
            inst.ImageKey = "Folder";
            inst.SelectedImageKey = "Folder";
            inst.Nodes.Add(new TreeNode());
            tn.Nodes.Add(inst);

            var types = new TreeNode("Types");
            types.ImageKey = "Folder";
            types.SelectedImageKey = "Folder";
            types.Nodes.Add(new TreeNode());
            tn.Nodes.Add(types);
        }

        public TreeView TreeView
        {
            get { return treeView; }
        }
        
        internal IApp App { get; set; }
    }
}
