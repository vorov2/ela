using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public sealed class DocumentMenuRenderer : ToolMenuRenderer
    {
        public DocumentMenuRenderer(int width)
        {
            _width = width;
        }
        
        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(UserBrushes.Window, e.AffectedBounds);
            g.DrawRectangle(UserPens.Border, new Rectangle(0, 0, e.AffectedBounds.Width - Dpi.ScaleX(1), e.AffectedBounds.Height - Dpi.ScaleY(1)));
        }

        private int _width;
        protected override int Width
        {
            get { return _width; }
        }
    }
}