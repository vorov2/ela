using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public class SingleBorderPanel : Panel
    {
        public SingleBorderPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            base.Padding = new Padding(0, 1, 0, 0);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.DrawLine(UserPens.Border, new Point(0, 0), new Point(ClientSize.Width, 0));

            if (BottomBorder)
                e.Graphics.DrawLine(UserPens.Border, new Point(0, ClientSize.Height - 1), new Point(ClientSize.Width, ClientSize.Height - 1));

            if (LeftBorder)
                e.Graphics.DrawLine(UserPens.Border, new Point(0, 0), new Point(0, ClientSize.Height - 1));
            
            if (RightBorder)
                e.Graphics.DrawLine(UserPens.Border, new Point(ClientSize.Width - 1, 0), new Point(ClientSize.Width - 1, ClientSize.Height - 1));

            base.OnPaint(e);
        }

        public bool BottomBorder { get; set; }

        public bool LeftBorder { get; set; }

        public bool RightBorder { get; set; }

        public new Padding Padding
        {
            get { return base.Padding; }
            set { }
        }
    }
}
