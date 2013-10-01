using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.CodeEditor
{
    public interface IAutocompleteService : IService
    {
        void ShowAutocomplete(IEnumerable<AutocompleteSymbol> symbols);
    }
}
