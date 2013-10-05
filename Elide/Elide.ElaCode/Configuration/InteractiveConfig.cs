using System;
using System.Drawing;
using Elide.Environment.Configuration;
using Elide.Scintilla.ObjectModel;

namespace Elide.ElaCode.Configuration
{
    [Serializable]
    public sealed class InteractiveConfig : Config
    {
        public InteractiveConfig()
        {
            CaretStyle = CaretStyle.Line;
            CaretWidth = CaretWidth.Thin;
            SelectionBackColor = KnownColor.Gray;
            SelectionForeColor = KnownColor.Black;
            SelectionTransparency = 100;
        }
        
        public CaretStyle CaretStyle { get; set; }

        public CaretWidth CaretWidth { get; set; }
        
        public KnownColor SelectionBackColor { get; set; }

        public KnownColor SelectionForeColor { get; set; }

        public bool UseSelectionColor { get; set; }

        public int SelectionTransparency { get; set; }
    }
}
