using System;

namespace Elide.Forms
{
    public sealed class DocumentItem
    {
        private readonly Func<String> captionGet;

        internal DocumentItem(Func<String> captionGet, object tag)
        {
            this.captionGet = captionGet;
            Tag = tag;
        }

        internal bool IsCaptionChanged()
        {
            return _caption != captionGet();
        }

        private string _caption;
        internal string Caption
        {
            get 
            { 
                _caption = captionGet();
                return _caption;
            }
        }

        public object Tag { get; private set; }
    }
}