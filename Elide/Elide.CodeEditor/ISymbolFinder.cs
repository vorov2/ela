using System;
using System.Collections.Generic;

namespace Elide.CodeEditor
{
    public interface ISymbolFinder
    {
        IEnumerable<SymbolLocation> FindSymbols(string symbol, bool allFiles);
    }
}
