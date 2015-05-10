using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Elide.Core;

namespace Elide.Forms
{
    public sealed class ColorPicker : ComboBox
    {
        public ColorPicker()
        {
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            base.DropDownHeight = 320;
            base.Height = Dpi.ScaleY(21);
            base.MinimumSize = new System.Drawing.Size(Dpi.ScaleX(10), Dpi.ScaleY(21));
            DrawMode = DrawMode.OwnerDrawVariable;
            SetStyle(ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint | 
                ControlStyles.OptimizedDoubleBuffer, true);
        }

        public void Populate()
        {
            Items.AddRange(new object[] { KnownColor.Black, KnownColor.White, KnownColor.Maroon, KnownColor.Green, KnownColor.Olive, 
                KnownColor.Navy, KnownColor.Purple, KnownColor.Teal, KnownColor.Silver, KnownColor.Gray, KnownColor.Red, KnownColor.Lime, KnownColor.Yellow, KnownColor.Blue,
                KnownColor.Magenta, KnownColor.Cyan, KnownColor.Pink, KnownColor.Cornsilk });
        }
        
        protected override void OnMeasureItem(System.Windows.Forms.MeasureItemEventArgs e)
        {
            e.ItemHeight = Dpi.ScaleY(16);
        }
        
        protected override void OnDrawItem(System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index == -1 || DesignMode)
                return;

            var colorRect = new Rectangle(e.Bounds.X + Dpi.ScaleX(2), e.Bounds.Y + Dpi.ScaleY(3), Dpi.ScaleX(12), Dpi.ScaleY(10));
            var textBrush = Enabled ? UserBrushes.Text : UserBrushes.Disabled;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                e.Graphics.FillRectangle(UserBrushes.Selection, e.Bounds);
                textBrush = UserBrushes.HighlightText;
            }
            else
                e.Graphics.FillRectangle(UserBrushes.Window, e.Bounds);

            var itemValue = this.Items[e.Index].ToString();

            using (var colorBrush = new SolidBrush(itemValue == "Default" ? Color.FromKnownColor(DefaultColor) : Color.FromName(itemValue)))
            {
                if (Enabled)
                    e.Graphics.FillRectangle(colorBrush, colorRect);

                e.Graphics.DrawRectangle(Enabled ? Pens.Black : Pens.Gray, colorRect);
                e.Graphics.DrawString(itemValue, Font, textBrush, new Point(e.Bounds.X + Dpi.ScaleX(18), e.Bounds.Y));
            }
        }

        private KnownColor _defaultColor;
        public KnownColor DefaultColor
        {
            get { return _defaultColor; }
            set
            {
                _defaultColor = value;
                RefreshItems();
            }
        }

        public KnownColor? SelectedColor
        {
            get
            {
                var si = SelectedItem;

                if (si != null && !DesignMode && si.ToString() != "Default")
                    return (KnownColor)Enum.Parse(typeof(KnownColor), si.ToString());

                return null;
            }
            set { Text = value.ToString(); }
        }

        public new int DropDownHeight
        {
            get { return base.DropDownHeight; }
            set { }
        }

        public void SetShowDefault(bool show)
        {
            Items.Remove("Default");

            if (show)
                Items.Insert(0, "Default");

            Refresh();
        }

        public new int Height
        {
            get { return base.Height; }
            set { }
        }

        public new Size MinimumSize
        {
            get { return base.MinimumSize; }
            set { }
        }
    }
}