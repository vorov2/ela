using System;
using System.Drawing;
using System.Windows.Forms;
using Elide.Forms;

namespace Elide.ElaObject
{
    public class ObjectFileHeaderControl : Control
    {
        public ObjectFileHeaderControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;

            var s = 5;
            var rect = new Rectangle(e.ClipRectangle.X + s, e.ClipRectangle.Y + s, e.ClipRectangle.Width - s * 2, e.ClipRectangle.Height - s * 2);

            g.FillRectangle(UserBrushes.Menu, rect);
            g.DrawRectangle(UserPens.Border, rect);

            g.DrawString(Text, Fonts.Caption, UserBrushes.Text, e.ClipRectangle.X + 7, e.ClipRectangle.Y + 7);
            base.OnPaint(e);
        }
    }
}
