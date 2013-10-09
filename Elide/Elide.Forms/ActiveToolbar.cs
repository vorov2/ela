using System;
using System.Linq;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.Forms
{
    public class ActiveToolbar : Control
    {
        private const int HEIGHT = 20;
        private List<ToolbarItem> items;
        private ToolbarItem selItem;
        private ToolbarItem hoverItem;
        private static readonly Font boldFont = new Font(Fonts.Menu, FontStyle.Bold);

        private sealed class ToolbarItem
        {
            public string Text { get; set; }
            public object Tag { get; set; }
            public int Left { get; set; }
            public int Right { get; set; }
            public bool NewContent { get; set; }
            public string ContentDescription { get; set; }
        }

        public ActiveToolbar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
            items = new List<ToolbarItem>();
        }

        public Action<Boolean,String> AddItem(string text, object tag)
        {
            items = items ?? new List<ToolbarItem>();
            var it = new ToolbarItem { Text = text, Tag = tag };
            items.Add(it);
            Refresh();
            return (b,dsc) => {
                it.NewContent = b;
                it.ContentDescription = b ? dsc : null;
                this.Invoke(Refresh);
            };
        }

        public IEnumerable<Object> EnumerateTags()
        {
            return items == null ? null : items.Select(i => i.Tag);
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(UserBrushes.Menu, ClientRectangle);
            var shift = Dpi.ScaleX(11);

            foreach (var i in items)
            {
                var w = DrawItem(g, i, shift, i == selItem || i == hoverItem);
                i.Left = shift;
                i.Right = shift + w;
                shift += w + Dpi.ScaleX(10);
            }

            g.DrawLine(UserPens.Border, new Point(0, 0), new Point(ClientSize.Width, 0));            
            g.DrawLine(UserPens.Border, new Point(shift, 0), new Point(shift, ClientSize.Height));

            var extraShift = Dpi.ScaleX(10);

            if (HighlightStatusString)
            {
                g.FillRectangle(UserBrushes.Window, new Rectangle(shift + Dpi.ScaleX(1), Dpi.ScaleY(1), ClientSize.Width - shift, ClientSize.Height));
                
                if (StatusImage != null)
                    g.DrawImage(StatusImage, new Rectangle(shift + Dpi.ScaleX(10), Dpi.ScaleY(4), Dpi.ScaleX(StatusImage.Width), Dpi.ScaleY(StatusImage.Height)));

                extraShift += Dpi.ScaleX(15);
            }

            var c = HighlightStatusString ? UserColors.Text : UserColors.Disabled;
            TextRenderer.DrawText(g, StatusString, Fonts.Text, new Rectangle(shift + extraShift, Dpi.ScaleY(3), ClientSize.Width - shift - Dpi.ScaleX(20), ClientSize.Height - Dpi.ScaleY(2)),
                c, TextFormatFlags.EndEllipsis);
        }
        
        private int DrawItem(Graphics g, ToolbarItem item, int pos, bool sel)
        {
            var text = item.NewContent && !String.IsNullOrEmpty(item.ContentDescription) ? 
                String.Format("{0} ({1})", item.Text, item.ContentDescription) : item.Text;
            var color = sel ? UserColors.HighlightText : UserColors.Text;
            var font = item.NewContent ? boldFont : Fonts.Menu;
            var tsize = TextRenderer.MeasureText(text, font);
            var width = tsize.Width ;

            if (sel)
                g.FillRectangle(UserBrushes.Selection, new Rectangle(pos - Dpi.ScaleX(5), Dpi.ScaleY(3), width + Dpi.ScaleX(10), tsize.Height + Dpi.ScaleY(2)));
            
            TextRenderer.DrawText(g, text, font, new Rectangle(pos, Dpi.ScaleY(3), width, ClientSize.Height), color, TextFormatFlags.Left);
            return width;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            var item = GetSelected(e.X);

            if (item == selItem)
            {
                hoverItem = null;
                Refresh();
            }
            else if (item != null && item != hoverItem)
            {
                hoverItem = item;
                Refresh();
            }
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (hoverItem != null)
            {
                hoverItem = null;
                Refresh();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            var sel = GetSelected(e.X);

            if (sel != null)
            {
                selItem = selItem == sel ? null : sel;
                Refresh();
                OnSelectedIndexChanged();
            }
        }
        
        protected override void OnResize(EventArgs e)
        {
            Height = Dpi.ScaleY(HEIGHT);
            base.OnResize(e);
        }

        private ToolbarItem GetSelected(int x)
        {
            return items.FirstOrDefault(i => x >= i.Left && x <= i.Right);
        }
        
        public int SelectedIndex
        {
            get { return selItem == null ? -1 : items.IndexOf(selItem); }
            set
            {
                if (items != null && items.Count > 0 && value < items.Count)
                {
                    selItem = value == -1 ? null : items[value];
                    this.Invoke(Refresh);
                    this.Invoke(OnSelectedIndexChanged);
                }
            }
        }

        public object SelectedTag
        {
            get { return selItem != null ? selItem.Tag : null; }
        }

        public bool HighlightStatusString { get; set; }

        public Image StatusImage { get; set; }

        private string _statusString;
        public string StatusString
        {
            get { return _statusString ?? "Ready"; }
            set
            {
                _statusString = value;
                Refresh();
            }
        }
        
        public event EventHandler SelectedIndexChanged;
        private void OnSelectedIndexChanged()
        {
            var h = SelectedIndexChanged;

            if (h != null)
                h(this, EventArgs.Empty);
        }
    }
}
