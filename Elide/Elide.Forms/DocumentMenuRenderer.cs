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
            g.DrawRectangle(UserPens.Border, new Rectangle(0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
            g.DrawLine(UserPens.Window, 1, 0, e.AffectedBounds.Width - 2, 0);
        }
        
        private int _width;
        protected override int Width
        {
            get { return _width; }
        }
    }
}