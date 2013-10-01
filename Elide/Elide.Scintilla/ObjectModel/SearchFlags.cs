using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    [Flags]
    public enum SearchFlags
    {
        None = 0,

        MatchCase = Sci.SCFIND_MATCHCASE,

        WholeWord = Sci.SCFIND_WHOLEWORD,

        WordStart = Sci.SCFIND_WORDSTART,
        
        Regex = Sci.SCFIND_REGEXP,
        
        Posix = Sci.SCFIND_POSIX
    }
}
