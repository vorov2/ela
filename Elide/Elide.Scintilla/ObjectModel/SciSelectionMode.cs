using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum SciSelectionMode
    {
        Stream = Sci.SC_SEL_STREAM,

        Rectangular = Sci.SC_SEL_RECTANGLE,

        ByLines = Sci.SC_SEL_LINES,

        RectangularThin = Sci.SC_SEL_THIN
    }
}
