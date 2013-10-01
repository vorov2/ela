using System;

namespace Elide.Forms
{
    public sealed class DefaultItem
    {
        public string Text { get; set; }

        public override string ToString()
        {
            return "(Default) " + Text;
        }
    }
}
