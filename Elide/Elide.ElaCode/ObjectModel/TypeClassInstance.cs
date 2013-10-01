using System;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class TypeClassInstance : IClassInstance
    {
        internal TypeClassInstance(string @class, string type, Location loc)
        {
            Class = @class;
            Type = type;
            Location = loc;
        }

        public string Class { get; private set; }

        public string Type { get; private set; }

        public Location Location { get; private set; }
    }
}
