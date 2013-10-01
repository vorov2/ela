using System;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class CodeName : ILocationBounded
    {
        public CodeName(string name, int flags, int line, int column)
        {
            Name = name;
            Flags = flags;
            Location = new Location(line, column);
        }

        public string Name { get; private set; }

        public int Flags { get; private set; }

        public Location Location { get; private set; }
    }
}
