using System;
using System.Drawing;
using Elide.Environment.Configuration;
using Elide.Scintilla.ObjectModel;

namespace Elide.Workbench.Configuration
{
    [Serializable]
    public sealed class OutputConfig : Config
    {
        public OutputConfig()
        {
            Styling = true;
            SelectionBackColor = KnownColor.Gray;
            SelectionForeColor = KnownColor.Black;
            SelectionTransparency = 100;
        }

        public bool Styling { get; set; }
        
        public KnownColor SelectionBackColor { get; set; }

        public KnownColor SelectionForeColor { get; set; }

        public bool UseSelectionColor { get; set; }

        public int SelectionTransparency { get; set; }
    }
}
