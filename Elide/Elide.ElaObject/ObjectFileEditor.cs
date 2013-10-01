using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using Elide.ElaObject.Configuration;
using Elide.ElaObject.Images;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Editors;
using Elide.Forms;

namespace Elide.ElaObject
{
    public sealed class ObjectFileEditor : IEditor
    {
        private ObjectFileViewer viewer;
        private ContextMenuStrip contextMenu;

        public ObjectFileEditor()
        {

        }
        
        public void Initialize(IApp app)
        {
            viewer = new ObjectFileViewer();
            App = app;
            app.GetService<IConfigService>().ConfigUpdated += ConfigUpdated;
            contextMenu = BuildContextMenu();
        }

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {
            if (e.Config is ElaObjectConfig)
            {
                var doc = App.GetService<IDocumentService>().GetActiveDocument() as ObjectFileDocument;

                if (doc != null)
                {
                    BuildTree(doc);
                    DisplayCaption(doc);

                    if (((ElaObjectConfig)e.Config).ExpandAllNodes)
                        doc.Presentation.Nodes.OfType<TreeNode>().ForEach(n => n.Expand());
                }
            }
        }

        public bool TestDocumentType(FileInfo fileInfo)
        {
            return fileInfo != null && fileInfo.HasExtension("elaobj");
        }
        
        public Document CreateDocument(string title)
        {
            throw new NotSupportedException();
        }

        public Document OpenDocument(FileInfo fileInfo)
        {
            using (var br = new BinaryReader(fileInfo.OpenRead()))
            {
                var reader = new ObjectFileReader();
                var elaObjectFile = reader.Read(br);
                var doc = new ObjectFileDocument(fileInfo, elaObjectFile);
                doc.Presentation.ImageList = viewer.ImageList;
                BuildTree(doc);
                return doc;
            }            
        }

        public void OpenDocument(Document doc)
        {
            var objDoc = (ObjectFileDocument)doc;
            viewer.Invoke(() =>
            {
                DisplayCaption(objDoc);
                viewer.AddHostedControl(objDoc.Presentation);
                objDoc.Presentation.ContextMenuStrip = contextMenu;

                if (App.Config<ElaObjectConfig>().ExpandAllNodes)
                    objDoc.Presentation.Nodes.OfType<TreeNode>().ForEach(n => n.Expand());
            });
        }

        public void ReloadDocument(Document doc, bool silent)
        {
            OpenDocument(doc);
        }

        public void Save(Document doc, FileInfo fileInfo)
        {
            
        }

        public void CloseDocument(Document doc)
        {
            doc.Dispose();
        }

        private void BuildTree(ObjectFileDocument doc)
        {
            var tb = new TreeBuilder(doc.ObjectFile, App.Config<ElaObjectConfig>());
            tb.Build(doc.Presentation);
        }

        private void DisplayCaption(ObjectFileDocument doc)
        {
            var c = App.Config<ElaObjectConfig>();
            viewer.CaptionVisible = c.DisplayHeader;

            if (c.UseCustomHeaderFormat)
            {
                try
                {
                    viewer.Caption = c.CustomHeaderFormat
                        .Replace("%name%", doc.FileInfo.Name.Replace(doc.FileInfo.Extension, String.Empty))
                        .Replace("%version%", doc.ObjectFile.Header.Version.ToString())
                        .Replace("%ela%", doc.ObjectFile.Header.ElaVersion.ToString())
                        .Replace("%date%", doc.ObjectFile.Header.Date.ToString());
                }
                catch
                {
                    DisplayDefaultCaption(doc);
                }
            }
            else
                DisplayDefaultCaption(doc);
        }

        private void DisplayDefaultCaption(ObjectFileDocument doc)
        {
            viewer.Caption = String.Format("Module {0}, object file format version {1}. Created by Ela {2}", 
                doc.FileInfo.Name.Replace(doc.FileInfo.Extension, String.Empty),
                doc.ObjectFile.Header.Version,
                doc.ObjectFile.Header.ElaVersion);
        }

        private ContextMenuStrip BuildContextMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            contextMenu = builder
                .Item("&Copy Item Value", "Ctrl+C", CopyNodeData, AllowCopyNodeData)
                .Finish();
            return contextMenu;
        }

        private MenuStrip BuildMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<MenuStrip>();
            return builder
                .Menu("&Object File")
                    .Item("&Copy Item Value", "Ctrl+C", CopyNodeData, AllowCopyNodeData)
                    .CloseMenu()
                .Finish();
        }

        private void CopyNodeData()
        {
            if (AllowCopyNodeData())
            {
                var tag = viewer.HostedTreeView.SelectedNode.Tag.ToString();

                try
                {
                    Clipboard.SetText(tag);
                }
                catch { }
            }
        }

        private bool AllowCopyNodeData()
        {
            return viewer.HostedTreeView != null && viewer.HostedTreeView.SelectedNode != null && viewer.HostedTreeView.SelectedNode.Tag != null;
        }

        public object Control
        {
            get { return viewer; }
        }

        public object Menu
        {
            get { return BuildMenu(); }
        }

        public Image DocumentIcon
        {
            get { return Bitmaps.Load<NS>("Icon"); }
        }

        public IApp App { get; private set; }
    }
}
