using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public class MenuRenderer : ToolStripRenderer
    {
        private const int MENUHEIGHT = 18;
        private const int ITEMHEIGHT = 16;
        private const int SEPAHEIGHT = 5;
        private readonly KeysConverter conv = new KeysConverter();

        public MenuRenderer()
        {
            
        }

        public static Size MeasureDropDown(ToolStripDropDown dropDown)
        {
            return InternalMeasureDropDown(dropDown.Items, false);
        }

        public static void ApplySkin(ToolStripDropDown dropDown)
        {
            dropDown.Renderer = new MenuRenderer();
            var size = MeasureDropDown(dropDown);
            dropDown.Width = size.Width;
            dropDown.Height = size.Height + 5;
        }

        private static Size InternalMeasureDropDown(ToolStripItemCollection items, bool root)
        {
            var conv = new KeysConverter();
            var height = 0;
            var maxWidth = 0;
            var cm = items.Count > 0 && items[0].Owner is ContextMenuStrip;

            foreach (ToolStripItem it in items)
            {
                it.Font = Fonts.Menu;
                it.AutoSize = false;
                var sep = it is ToolStripSeparator;
                it.Height = sep ? SEPAHEIGHT : ITEMHEIGHT;

                var sc = String.Empty;

                var mi = it as ToolStripMenuItem;

                if (mi != null && mi.ShortcutKeys != Keys.None)
                    sc = conv.ConvertToString(mi.ShortcutKeys) + "     ";

                var width = sep ? 10 : TextRenderer.MeasureText(it.Text + sc, it.Font).Width +
                    (IsTopLevel(it) && !cm ? 10 : 20);

                if ((mi != null && !root && mi.DropDownItems.Count > 0) || cm)
                    width += 30;

                it.Width = width;

                if (width > maxWidth)
                    maxWidth = width;

                if (mi != null)
                {
                    mi.DropDown.AutoSize = false;
                    var size = InternalMeasureDropDown(mi.DropDownItems, false);
                    mi.DropDown.Height = size.Height + 5;
                    mi.DropDown.Width = size.Width;
                }

                height += it.Height;
            }

            if (!root || cm)
                foreach (ToolStripItem it in items)
                    it.Width = maxWidth;

            return new Size(maxWidth, height);
        }

        protected override void Initialize(ToolStrip toolStrip)
        {
            toolStrip.AutoSize = false;
            toolStrip.Height = MENUHEIGHT;
            InitializeSubMenus(toolStrip.Items, true);
        }

        protected Size InitializeSubMenus(ToolStripItemCollection items, bool root)
        {
            return InternalMeasureDropDown(items, root);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (e.Item.Selected)
            {
                var rect = new Rectangle(1, 1, e.Item.Bounds.Width - 2, e.Item.Bounds.Height);
                e.Graphics.FillRectangle(UserBrushes.Selection, rect);
            }
            else if (e.Item is ToolStripMenuItem && ((ToolStripMenuItem)e.Item).DropDown.Visible)
            {
                var rect = new Rectangle(1, 1, e.Item.Bounds.Width - 2, e.Item.Bounds.Height);
                e.Graphics.FillRectangle(UserBrushes.Selection, rect);
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(UserBrushes.Menu, e.AffectedBounds);

            if (!(e.ToolStrip is MenuStrip))
                g.DrawRectangle(UserPens.Border, new Rectangle(0, 0, e.AffectedBounds.Width - 1, e.AffectedBounds.Height - 1));
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            e.Graphics.DrawLine(UserPens.Border, 0, 3, e.Item.Bounds.Width, 3);
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            using (var bmp = Bitmaps.Load("Arrow"))
                e.Graphics.DrawImage(bmp, new Point(e.Item.Bounds.Width - 10, e.ArrowRectangle.Y + 7));
        }

        protected override void OnRenderItemCheck(ToolStripItemImageRenderEventArgs e)
        {
            using (var bmp = Bitmaps.Load("Check"))
                e.Graphics.DrawImage(bmp, new Point(e.ImageRectangle.X + 1, e.ImageRectangle.Y + 5));
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            var g = e.Graphics;
            var cm = e.ToolStrip is ContextMenuStrip;

            var x = IsTopLevel(e.Item) && !cm ? 0 : 15;
            var y = IsTopLevel(e.Item) && !cm ? 1 : e.TextRectangle.Y;

            if (e.Item.Text != e.Text)
                x = e.ToolStrip.Width - TextRenderer.MeasureText(e.Text, e.TextFont).Width - 2;

            e.TextRectangle = new Rectangle(x, y, e.Item.Width - x, 16);
            var flag = IsTopLevel(e.Item) && !cm ? TextFormatFlags.Top | TextFormatFlags.HorizontalCenter : TextFormatFlags.Default;
            var color = !e.Item.Enabled ? UserColors.Disabled :
                (e.Item.Selected || e.Item.Pressed ? UserColors.HighlightText : UserColors.Text);

            TextRenderer.DrawText(e.Graphics, e.Text, e.TextFont, e.TextRectangle, color,
                TextFormatFlags.Left | TextFormatFlags.EndEllipsis | flag);
        }

        private static bool IsTopLevel(ToolStripItem e)
        {
            return e.OwnerItem == null;
        }
    }
}
