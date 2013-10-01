using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum LineEndings
    {
        Windows = Sci.SC_EOL_CRLF,

        Unix = Sci.SC_EOL_LF,

        Mac = Sci.SC_EOL_CR
    }
}
