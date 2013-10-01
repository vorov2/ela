using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum CaretStyle
    {
        None = Sci.CARETSTYLE_INVISIBLE,

        Block = Sci.CARETSTYLE_BLOCK,

        Line = Sci.CARETSTYLE_LINE
    }
}
