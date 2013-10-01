using System;
using System.Windows.Forms;
using Elide.Forms;
using Elide.Workbench.Images;
using Elide.Forms.State;
using System.Collections.Generic;

namespace Elide.Workbench.Views
{
    public partial class OpenFilesControl : StateUserControl
    {
        public OpenFilesControl()
        {
            InitializeComponent();
            treeView.ImageList = imageList;
            treeView.ImageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            treeView.ImageList.Images.Add("Flag", Bitmaps.Load<NS>("Flag"));
            FlaggedDocuments = new List<String>();
        }

        public TreeView TreeView
        {
            get { return treeView; }
        }

        [StateItem]
        public List<String> FlaggedDocuments { get; set; }
    }
}
