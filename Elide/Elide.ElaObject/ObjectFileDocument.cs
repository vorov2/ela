using System;
using System.IO;
using System.Windows.Forms;
using Elide.ElaObject.ObjectModel;
using Elide.Environment;
using Elide.Forms;

namespace Elide.ElaObject
{
    internal sealed class ObjectFileDocument : Document
    {
        private readonly TreeView treeView;

        internal ObjectFileDocument(FileInfo fileInfo, ElaObjectFile objectFile) : base(fileInfo)
        {
            ObjectFile = objectFile;

            treeView = new BufferedTreeView();
            treeView.Font = Fonts.Menu;
            treeView.ShowLines = false;
            treeView.BackColor = UserColors.Window;
            treeView.Dock = DockStyle.Fill;
            treeView.BorderStyle = BorderStyle.None;
        }

        public override bool IsDirty
        {
            get { return false; }
            set { }
        }

        internal ElaObjectFile ObjectFile { get; private set; }

        internal TreeView Presentation
        {
            get { return treeView; }
        }
    }
}
