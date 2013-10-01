using System;
using Elide.Configuration;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.PlainText.Configuration
{
    [Serializable]
    public sealed class PlainTextConfig : Config
    {
        public PlainTextConfig()
        {
            HighlightHyperlinks = true;
            SingleClickNavigation = true;
        }

        public bool HighlightHyperlinks { get; set; }

        public bool HighlightBoldItalics { get; set; }

        public bool SingleClickNavigation { get; set; }
    }
}
