using System;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class UserType : IType
    {
        internal UserType(string name, Location loc)
        {
            Name = name;
            Location = loc;
        }

        public string Name { get; private set; }

        public Location Location { get; private set;  }
    }
}
