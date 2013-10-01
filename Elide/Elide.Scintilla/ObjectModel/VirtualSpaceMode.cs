using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum VirtualSpaceMode
    {
        Disabled = Sci.SCVS_NONE,

        RectangularSelections = Sci.SCVS_RECTANGULARSELECTION,

        OtherSelections = Sci.SCVS_USERACCESSIBLE,

        AllSelections = Sci.SCVS_RECTANGULARSELECTION | Sci.SCVS_USERACCESSIBLE
    }
}
