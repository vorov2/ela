using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum SciModifier
    {
        None = Sci.SCMOD_NORM,

        Alt = Sci.SCMOD_ALT,

        Ctrl = Sci.SCMOD_CTRL,

        Shift = Sci.SCMOD_SHIFT
    }
}
