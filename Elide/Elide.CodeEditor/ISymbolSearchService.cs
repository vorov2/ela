using System;
using Elide.Core;

namespace Elide.CodeEditor
{
    public interface ISymbolSearchService : IService
    {
        void SearchSymbol(string symbol, ISymbolFinder finder);
    }
}
