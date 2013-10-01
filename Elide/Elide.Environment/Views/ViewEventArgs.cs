using System;

namespace Elide.Environment.Views
{
    public sealed class ViewEventArgs : EventArgs
    {
        public static readonly ViewEventArgs None = new ViewEventArgs(false, null);

        public ViewEventArgs(bool newContent, string contentDescription)
        {
            NewContent = newContent;
            ContentDescription = contentDescription;
        }

        public bool NewContent { get; private set; }

        public string ContentDescription { get; private set; }
    }
}
