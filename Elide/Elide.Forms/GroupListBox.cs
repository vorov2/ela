using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace Elide.Forms
{
    public sealed class GroupListBox : ListBox
    {
        public GroupListBox()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
            base.DrawMode = DrawMode.OwnerDrawVariable;
        }
        
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            if (SelectedIndex == -1)
                return;

            var item = Items[SelectedIndex];

            if (item is ListGroup)
            {
                CollapseAll();
                ((ListGroup)item).Expanded = true;

                if (SelectedIndex < Items.Count - 1)
                    SelectedIndex++;

                RefreshItems();
            }
            else if (item is ListItem)
            {
                var g = FindListGroup(SelectedIndex);

                if (!g.Expanded)
                {
                    CollapseAll();
                    g.Expanded = true;
                    RefreshItems();
                }
            }
        }
        
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            if (IsItemVisible(e.Index))
            {
                e.ItemWidth = ClientSize.Width;
                e.ItemHeight = 18;
            }
            else
                e.ItemHeight = 0;

            base.OnMeasureItem(e);
        }

        private void CollapseAll()
        {
            foreach (var o in Items)
            {
                var g = o as ListGroup;

                if (g != null)
                    g.Expanded = false;
            }
        }

        private bool IsItemVisible(int index)
        {
            var g = FindListGroup(index);
            return g == null || g.Expanded;
        }

        private ListGroup FindListGroup(int index)
        {
            if (Items.Count == 0 || index == -1 || Items[index] is ListGroup)
                return null;

            for (var i = index; i > -1; i--)
            {
                var g = Items[i] as ListGroup;

                if (g != null)
                    return g;
            }

            return null;
        }
        
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            if (e.Index != -1 && Items.Count > 0 && IsItemVisible(e.Index))
            {
                if (Items[e.Index] is ListGroup)
                    DrawHeader((ListGroup)Items[e.Index], e);
                else if (Items[e.Index] is ListItem)
                    DrawNormalItem((ListItem)Items[e.Index], e);
            }

            base.OnDrawItem(e);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            e.Handled =
                SelectedIndex == -1 ||
                SelectedIndex == 0 && e.KeyValue == 38 ||
                SelectedIndex == Items.Count - 1 && e.KeyValue == 40 ||
                e.KeyValue == 38 && Items[SelectedIndex - 1] is ListGroup ||
                e.KeyValue == 40 && Items[SelectedIndex + 1] is ListGroup;                
        }

        private void DrawNormalItem(ListItem item, DrawItemEventArgs e)
        {
            var caption = item.Text;
            var brush = default(Brush);

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                brush = UserBrushes.HighlightText;
                e.Graphics.FillRectangle(UserBrushes.Selection,
                    new Rectangle(e.Bounds.X, e.Bounds.Y + 0, e.Bounds.Width, e.Bounds.Height - 0));
            }
            else
            {
                brush = UserBrushes.Text;
                e.Graphics.FillRectangle(UserBrushes.Window, e.Bounds);
            }

            var captionSize = e.Graphics.MeasureString(caption, Font);
            e.Graphics.DrawString(caption, e.Font, brush, e.Bounds.X + 15, e.Bounds.Y + (e.Bounds.Height - captionSize.Height) / 2);
        }

        private void DrawHeader(ListGroup group, DrawItemEventArgs e)
        {
            var caption = group.Title;
            e.Graphics.FillRectangle(UserBrushes.Window, e.Bounds);

            using (var font = new Font(e.Font, FontStyle.Bold))
            {
                if (group.Expanded)
                    e.Graphics.DrawImage(Bitmaps.Load("ArrowDownGray"), e.Bounds.X + 5, e.Bounds.Y + 7);
                else
                    e.Graphics.DrawImage(Bitmaps.Load("ArrowGray"), e.Bounds.X + 7, e.Bounds.Y + 5);

                var captionSize = e.Graphics.MeasureString(caption, font);
                e.Graphics.DrawString(caption, font, SystemBrushes.ControlText,
                    e.Bounds.X + 14, e.Bounds.Y + (e.Bounds.Height - captionSize.Height) / 2);
            }
        }

        [Browsable(false)]
        public override DrawMode DrawMode
        {
            get { return DrawMode.OwnerDrawVariable; }
            set { }
        }
    }
}