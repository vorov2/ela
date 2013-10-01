using System;

namespace Elide.Forms
{
    public sealed class ListItem
    {
        public ListItem(string text, object value)
        {
            Text = text;
            Value = value;
        }

        public string Text { get; private set; }

        public object Value { get; private set; }
    }
}
