using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Elide.Forms;
using Elide.ElaCode.Images;
using Elide.Environment;
using Elide.Core;

namespace Elide.ElaCode.Views
{
    public partial class AstControl : UserControl
    {
        public AstControl()
        {
            InitializeComponent();
            treeView.Font = Fonts.Menu;
            treeView.ShowLines = false;
            treeView.BackColor = UserColors.Window;
            treeView.Dock = DockStyle.Fill;
            treeView.BorderStyle = BorderStyle.None;

            imageList.ColorDepth = ColorDepth.Depth32Bit;
            imageList.ImageSize = new Size(16, 16);
            imageList.TransparentColor = Color.Magenta;
            imageList.Images.Add("Folder", Bitmaps.Load<NS>("Folder"));
            imageList.Images.Add("Literal", Bitmaps.Load<NS>("Literal"));
            imageList.Images.Add("Module", Bitmaps.Load<NS>("Module"));
            imageList.Images.Add("Functon", Bitmaps.Load<NS>("Function"));
            imageList.Images.Add("Field", Bitmaps.Load<NS>("Field"));
            imageList.Images.Add("Arrow", Bitmaps.Load<NS>("Arrow"));

            treeView.ImageList = imageList;
        }       

        public TreeView TreeView
        {
            get { return treeView; }
        }

        public void ShowWorking()
        {
            workLabel.Text = "Wait, while generating AST...";
            workLabel.Visible = true;
        }

        public void ShowNoData()
        {
            workLabel.Text = "No data to display";
            workLabel.Visible = true;
        }

        public void HideInfo()
        {
            workLabel.Visible = false;
        }
    }
}
