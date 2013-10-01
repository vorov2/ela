using System;

namespace Elide.Scintilla.ObjectModel
{
    [Flags]
    public enum SelectionFlags
    {
        None,

        ScrollToCaret = 0x02,

        MakeOnlySelection = 0x04
    }
}
