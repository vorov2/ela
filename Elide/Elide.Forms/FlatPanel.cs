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
            base.Padding = new Padding(Dpi.ScaleX(2), Dpi.ScaleY(2), Dpi.ScaleX(2), Dpi.ScaleY(2));
        }

        public void SetPadding(Padding pad)
        {
            base.Padding = pad;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var rec = WideRendering ?
                new Rectangle(0, 0, ClientSize.Width - Dpi.ScaleX(1), ClientSize.Height - Dpi.ScaleX(1)) :
                new Rectangle(Dpi.ScaleX(1), 0, ClientSize.Width - Dpi.ScaleX(2), ClientSize.Height - Dpi.ScaleX(2));

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
