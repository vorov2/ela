using System;
using System.Linq;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Elide.Forms
{
    public sealed class FontPicker : ComboBox
    {
        private DefaultItem defaultItem;
        
        public FontPicker()
        {
            defaultItem = new DefaultItem();
            DropDownStyle = ComboBoxStyle.DropDownList;
            DropDownHeight = 500;
            DrawMode = DrawMode.OwnerDrawVariable;
            Font = Fonts.ControlText;
        }

        public void Populate()
        {
            Items.Clear();

            foreach (var f in FontFamily.Families)
                if (!String.IsNullOrEmpty(f.Name))
                    Items.Add(f.Name);
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

            using (var boldFont = new Font(Font, FontStyle.Bold))
            {
                var textBrush = SystemBrushes.ControlText;

                if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                {
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                    textBrush = SystemBrushes.HighlightText;
                }
                else
                    e.Graphics.FillRectangle(SystemBrushes.Window, e.Bounds);

                var itemValue = this.Items[e.Index].ToString();
                var mono = FontHelper.MonospaceFonts.Contains(itemValue);
                e.Graphics.DrawString(itemValue.ToString(), mono ? boldFont : Font, textBrush, new Point(e.Bounds.X, e.Bounds.Y));
            }
        }

        public string SelectedFont
        {
            get { return SelectedItem is String ? SelectedItem.ToString() : null; }
            set { Text = value != null ? value.ToString() : String.Empty; }
        }

        public object DefaultItem
        {
            get { return defaultItem; }
        }
    }
}