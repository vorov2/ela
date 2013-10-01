using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public enum WordWrapMode
    {
        None = Sci.SC_WRAP_NONE,

        Char = Sci.SC_WRAP_CHAR,

        Word = Sci.SC_WRAP_WORD
    }
}