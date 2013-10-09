using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public abstract class ToolMenuRenderer : ToolStripRenderer
    {
        private const int HEIGHT = 16;

        protected ToolMenuRenderer()
        {

        }
    
        protected override void Initialize(ToolStrip toolStrip)
        {
            toolStrip.Padding = new Padding(0, 0, 0, Dpi.ScaleY(5));
            toolStrip.Width = Width;
            toolStrip.Height = Dpi.ScaleY(toolStrip.Items.Count * HEIGHT + 5);
            toolStrip.AutoSize = false;
        }
        
        protected override void InitializeItem(ToolStripItem item)
        {
            item.AutoSize = false;
            item.Height = Dpi.ScaleY(HEIGHT);
            item.Width = Width;
        }
        
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                var g = e.Graphics;
                var rect = new Rectangle(Dpi.ScaleX(1), Dpi.ScaleY(1), e.Item.Bounds.Width - Dpi.ScaleX(2), e.Item.Bounds.Height);
                g.FillRectangle(UserBrushes.Selection, rect);
            }
        }
                
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            var color = UserColors.Text;

            if (e.Item.Selected)
                color = UserColors.HighlightText;

            e.TextRectangle = new Rectangle(Dpi.ScaleX(15), e.TextRectangle.Y, Width - Dpi.ScaleX(18), Dpi.ScaleY(16));
            TextRenderer.DrawText(e.Graphics, e.Text, Fonts.Menu, e.TextRectangle, color, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
        
        protected abstract int Width { get; }
    }
}
