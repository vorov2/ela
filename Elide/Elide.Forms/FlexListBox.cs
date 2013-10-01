using System;
using System.Drawing;
using System.Windows.Forms;

namespace Elide.Forms
{
    public sealed class FlexListBox : ListBox
    {
        public FlexListBox()
        {
            DrawMode = DrawMode.OwnerDrawVariable;
            Font = Fonts.ControlText;
        }

        protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemHeight = ItemHeight;
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (e.Index == -1 || Items.Count == 0)
                return;

            e.Graphics.SetClip(e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(0, e.BackColor.R, e.BackColor.G, e.BackColor.B)), e.Bounds);
            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(255, e.BackColor.R, e.BackColor.G, e.BackColor.B)), e.Bounds);

            var textBrush = UserBrushes.Text;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(UserBrushes.Selection, e.Bounds);
                textBrush = UserBrushes.HighlightText;
            }
            else
                e.Graphics.FillRectangle(UserBrushes.Window, e.Bounds);

            var itemValue = this.Items[e.Index].ToString();
            e.Graphics.DrawString(itemValue.ToString(), Font, textBrush, new Point(e.Bounds.X, e.Bounds.Y));
        }
    }
}