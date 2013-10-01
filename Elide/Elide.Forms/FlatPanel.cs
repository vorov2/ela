using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public class FlatPanel : Panel
    {
        public FlatPanel()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            base.Padding = new Padding(2);
        }

        public void SetPadding(Padding pad)
        {
            base.Padding = pad;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rec = WideRendering ?
                new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1) :
                new Rectangle(1, 0, ClientSize.Width - 2, ClientSize.Height - 2);

            using (var b = new SolidBrush(UserColors.Window))
                e.Graphics.FillRectangle(b, rec);

            e.Graphics.DrawRectangle(UserPens.Border, rec);
            base.OnPaint(e);
        }
        
        public bool WideRendering { get; set; }

        public new Padding Padding
        {
            get { return base.Padding; }
            set { }
        }
    }
}
