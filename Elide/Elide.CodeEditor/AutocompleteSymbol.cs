using System;

namespace Elide.CodeEditor
{
    public sealed class AutocompleteSymbol : IComparable
    {
        public AutocompleteSymbol(string name, AutocompleteSymbolType type)
        {
            Name = name;
            Type = type;
        }

        public int CompareTo(object obj)
        {
            var ac = obj as AutocompleteSymbol;

            if (ac == null)
                return -1;

            return ac.Name.CompareTo(Name);
        }

        public string Name { get; private set; }

        public AutocompleteSymbolType Type { get; private set; }
    }
}
