using System;
using System.Drawing;
using Elide.Scintilla.Internal;
using Elide.Scintilla.ObjectModel;

namespace Elide.Scintilla
{
    public sealed class Indicator
    {
        private EditorRef @ref;

        internal Indicator(int num, EditorRef @ref)
        {
            Number = num;
            this.@ref = @ref;
        }
        
        public int Number { get; private set; }
        
        public IndicatorStyle Style
        {
            get { return (IndicatorStyle)@ref.Send(Sci.SCI_INDICGETSTYLE, Number); }
            set { @ref.Send(Sci.SCI_INDICSETSTYLE, Number, (Int32)value); }
        }
        
        public Color ForeColor
        {
            get { return SciColor.FromScintillaColor(@ref.Send(Sci.SCI_INDICGETFORE, Number)); }
            set { @ref.Send(Sci.SCI_INDICSETFORE, Number, value.ToScintillaColor()); }
        }
    }
}
