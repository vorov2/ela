using System;
using System.Windows.Forms;
using System.Drawing;

namespace Elide.Forms
{
    public sealed class IntPicker : ComboBox
    {
        private DefaultItem defaultItem;

        public IntPicker()
        {
            defaultItem = new DefaultItem();
            DropDownStyle = ComboBoxStyle.DropDownList;
            DropDownHeight = 500;
            DrawMode = DrawMode.OwnerDrawVariable;
            Font = Fonts.ControlText;
        }

        public void Populate(int start, int end)
        {
            Items.Clear();

            for (var i = start; i < end + 1; i++)
                Items.Add(i);
        }

        public void RemoveDefault()
        {
            Items.Remove(defaultItem);
        }

        public void AddDefault(string text)
        {
            Items.Remove(defaultItem);

            defaultItem.Text = text;
            Items.Insert(0, defaultItem);
            RefreshItem(0);
        }

        protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemHeight = 16;
        }

        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if (e.Index == -1)
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

        public object DefaultItem
        {
            get { return defaultItem; }
        }
    }
}