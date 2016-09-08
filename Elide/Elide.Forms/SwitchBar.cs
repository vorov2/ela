using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Elide.Forms
{
    public sealed class SwitchBar : Control
    {
        private ContextMenuStrip contextMenu;
        private int hoverItem = -1;
        private int selItem = -1;
        private int currentWidth = 0;

        public SwitchBar()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            _items = new List<SwitchBarItem>();
            contextMenu = new ContextMenuStrip();
            contextMenu.Closed += (o, e) => { hoverItem = -1; Refresh(); };
        }		       

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.FillRectangle(UserBrushes.Window, ClientRectangle);
            var width = 0;
            var textColor = UserColors.Text;
            var corner = "Corner";
            var cross = "Cross";

            if (SelectedItem != null)
                currentWidth = width = (Int32)g.MeasureString(SelectedItem.Caption, Fonts.Caption).Width;

            if (hoverItem == 0 || contextMenu.Visible)
            {
                g.FillRectangle(UserBrushes.Selection, new Rectangle(Dpi.ScaleX(3), Dpi.ScaleY(4), Dpi.ScaleX(12) + width, Dpi.ScaleY(14)));
                textColor = UserColors.HighlightText;
                corner = "CornerWhite";
            }
            else if (hoverItem == 1)
            {
                g.FillRectangle(UserBrushes.Selection, new Rectangle(ClientSize.Width - Dpi.ScaleX(17), Dpi.ScaleY(4), Dpi.ScaleX(14), Dpi.ScaleY(14)));
                cross = "CrossWhite";
            }

            using (var bmp = Bitmaps.Load(corner))
                g.DrawImage(bmp, new Rectangle(Dpi.ScaleX(5), Dpi.ScaleY(9), Dpi.ScaleX(bmp.Width), Dpi.ScaleY(bmp.Height)));

            using (var bmp = Bitmaps.Load(cross))
                g.DrawImage(bmp, new Rectangle(ClientSize.Width - Dpi.ScaleX(15), Dpi.ScaleY(7), Dpi.ScaleX(bmp.Width), Dpi.ScaleY(bmp.Height)));

            g.DrawRectangle(UserPens.Border, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - Dpi.ScaleX(1), ClientRectangle.Height));

            if (SelectedItem != null)
                TextRenderer.DrawText(g, SelectedItem.Caption, Fonts.Caption, new Rectangle(Dpi.ScaleX(14), 0, width + Dpi.ScaleY(20), ClientSize.Height), textColor,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (e.X > ClientSize.Width - Dpi.ScaleX(17))
            {
                if (hoverItem != 1)
                {
                    hoverItem = 1;
                    Refresh();
                }
            }
            else if (e.X <= Dpi.ScaleX(12) + currentWidth)
            {
                if (hoverItem != 0)
                {
                    hoverItem = 0;
                    Refresh();
                }
            }
            else if (hoverItem != -1)
            {
                hoverItem = -1;
                Refresh();
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (hoverItem != -1)
            {
                hoverItem = -1;
                Refresh();
            }

            base.OnMouseLeave(e);
        }        
        
        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.X > ClientSize.Width - Dpi.ScaleX(17))
            {
                OnCloseRequested(EventArgs.Empty);
                return;
            }
            else if (e.X <= Dpi.ScaleX(12) + currentWidth)
            {
                contextMenu.Items.Clear();

                for (var i = 0; i < Items.Count; i++)
                {
                    var it = Items[i];

                    if (it != SelectedItem)
                    {
                        var mi = new ToolStripMenuItem { Text = it.Caption, Tag = it };
                        mi.Click += (o, a) =>
                        {
                            var itm = (SwitchBarItem)((ToolStripMenuItem)o).Tag;
                            SelectedIndex = Items.IndexOf(itm);
                        };

                        contextMenu.Items.Add(mi);
                    }
                }

                contextMenu.Renderer = new DocumentMenuRenderer(ClientSize.Width - Dpi.ScaleX(20));
                contextMenu.Show(this, Dpi.ScaleX(3), Dpi.ScaleY(18));
            }
        }
                
        public event EventHandler<SwitchBarEventArgs> SelectedIndexChanged;
        private void OnSelectedIndexChanged(SwitchBarEventArgs e)
        {
            var h = SelectedIndexChanged;

            if (h != null)
                h(this, e);
        }

        public event EventHandler CloseRequested;
        private void OnCloseRequested(EventArgs e)
        {
            var h = CloseRequested;

            if (h != null)
                h(this, e);
        }
                
        private List<SwitchBarItem> _items;
        public List<SwitchBarItem> Items
        {
            get { return _items; }
        }

        public SwitchBarItem SelectedItem
        {
            get { return selItem == -1 ? null : Items[selItem]; }
        }
        
        public int SelectedIndex
        {
            get { return selItem; }
            set
            {
                if (value >= Items.Count)
                    throw new IndexOutOfRangeException();

                if (selItem != value)
                {
                    selItem = value;

                    Refresh();
                    OnSelectedIndexChanged(new SwitchBarEventArgs(SelectedItem));
                }
            }
        }
    }
}