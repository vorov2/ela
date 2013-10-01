using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla
{
    public sealed class StandardIndicators
    {
        private EditorRef @ref;

        internal StandardIndicators(EditorRef @ref)
        {
            this.@ref = @ref;
            Hint = new Indicator(0, @ref);
            Information = new Indicator(1, @ref);
            Warning = new Indicator(2, @ref);
            Error = new Indicator(3, @ref);
            Important = new Indicator(4, @ref);
            FoundSymbol = new Indicator(5, @ref);
            FoundSymbol2 = new Indicator(6, @ref);
            Mark = new Indicator(7, @ref);
        }
        
        public Indicator Hint { get; private set; }

        public Indicator Information { get; private set; }

        public Indicator Warning { get; private set; }

        public Indicator Error { get; private set; }

        public Indicator Important { get; private set; }

        public Indicator FoundSymbol { get; private set; }

        public Indicator FoundSymbol2 { get; private set; }

        public Indicator Mark { get; private set; }
    }
}
