using System;

namespace Elide.Forms
{
    public sealed class ListGroup
    {
        public ListGroup(string title)
        {
            Title = title;
        }

        public string Title { get; private set; }

        internal bool Expanded { get; set; }
    }
}
