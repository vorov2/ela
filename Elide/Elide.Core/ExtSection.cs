using System;

namespace Elide.Core
{
    public sealed class ExtSection
    {
        public ExtSection(string name, ExtList<ExtEntry> entries)
        {
            Name = name;
            Entries = entries;
        }

        public override string ToString()
        {
            return Name;
        }

        public string Name { get; private set; }

        public ExtList<ExtEntry> Entries { get; private set; }
    }
}
