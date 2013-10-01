using System;
using System.Drawing;

namespace Elide.Environment.Configuration
{
    [Serializable]
    public sealed class StyleItemConfig : Config
    {
        public StyleItemConfig()
        {
            FontName = null;
            FontSize = 0;
            ForeColor = null;
            BackColor = null;
            Bold = null;
            Italic = null;
            Underline = null;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public string Type { get; set; }

        public string DisplayName { get; set; }

        public string FontName { get; set; }

        public int FontSize { get; set; }

        public KnownColor? ForeColor { get; set; }

        public KnownColor? BackColor { get; set; }

        public bool? Bold { get; set; }

        public bool? Italic { get; set; }

        public bool? Underline { get; set; }
    }
}
