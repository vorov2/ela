using System;
using System.Drawing;
using System.Windows.Forms;
using Elide.ElaObject.Images;
using Elide.Forms;

namespace Elide.ElaObject
{
    public partial class ObjectFileViewer : UserControl
    {
        public ObjectFileViewer()
        {
            InitializeComponent();

            ImageList = new ImageList();
            ImageList.ColorDepth = ColorDepth.Depth32Bit;
            ImageList.ImageSize = new Size(16, 16);
            ImageList.TransparentColor = Color.Magenta;
            ImageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            ImageList.Images.Add("Literal", Bitmaps.Load<NS>("Literal"));
            ImageList.Images.Add("Module", Bitmaps.Load<NS>("Module"));
            ImageList.Images.Add("Variable", Bitmaps.Load<NS>("Variable"));
            ImageList.Images.Add("PrivateVariable", Bitmaps.Load<NS>("PrivateVariable"));
            ImageList.Images.Add("Symbol", Bitmaps.Load<NS>("Symbol"));
            ImageList.Images.Add("Layout", Bitmaps.Load<NS>("Layout"));
            ImageList.Images.Add("Op", Bitmaps.Load<NS>("Op"));
            ImageList.Images.Add("Member", Bitmaps.Load<NS>("Member"));
            ImageList.Images.Add("Interface", Bitmaps.Load<NS>("Interface"));
            ImageList.Images.Add("Instance", Bitmaps.Load<NS>("Instance"));
            ImageList.Images.Add("Type", Bitmaps.Load<NS>("Type"));

        }
        
        private void ObjectFileViewer_Load(object sender, EventArgs e)
        {
            
        }

        public void AddHostedControl(TreeView c)
        {
            panel.Controls.Clear();
            panel.Controls.Add(c);
        }

        public ImageList ImageList { get; private set; }

        public TreeView HostedTreeView
        {
            get { return panel.Controls.Count > 0 ? panel.Controls[0] as TreeView : null; }
        }

        public bool CaptionVisible
        {
            get { return header.Visible; }
            set { header.Visible = value; }
        }

        public string Caption
        {
            get { return header.Text; }
            set 
            { 
                header.Text = value;
                header.Refresh();
            }
        }
    }
}
