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
            toolStrip.Padding = new Padding(0, 0, 0, 5);
            toolStrip.Width = Width;
            toolStrip.Height = toolStrip.Items.Count * HEIGHT + 5;
            toolStrip.AutoSize = false;
        }
        
        protected override void InitializeItem(ToolStripItem item)
        {
            item.AutoSize = false;
            item.Height = HEIGHT;
            item.Width = Width;
        }
        
        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                var g = e.Graphics;
                var rect = new Rectangle(1, 1, e.Item.Bounds.Width - 2, e.Item.Bounds.Height);
                g.FillRectangle(Brushes.LightGray, rect);
            }
        }
        
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            e.TextRectangle = new Rectangle(15, e.TextRectangle.Y, Width - 18, 16);
            TextRenderer.DrawText(e.Graphics, e.Text, Fonts.Menu, e.TextRectangle, Color.Black, TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }
        
        protected abstract int Width { get; }
    }
}
