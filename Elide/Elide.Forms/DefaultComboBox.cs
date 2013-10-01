using System;
using System.Windows.Forms;
using System.Drawing;

namespace Elide.Forms
{
    public sealed class DefaultComboBox : ComboBox
    {
        private DefaultItem defaultItem;

        public DefaultComboBox()
        {
            defaultItem = new DefaultItem();
            DropDownStyle = ComboBoxStyle.DropDownList;
            Font = Fonts.ControlText;
        }

        public void Populate(params object[] args)
        {
            Items.Clear();
            Items.AddRange(args);
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

        public object DefaultItem
        {
            get { return defaultItem; }
        }
    }
}