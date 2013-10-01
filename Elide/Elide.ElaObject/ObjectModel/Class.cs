using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Class
    {
        internal Class(string name, IEnumerable<ElaClassMember> members)
        {
            Name = name;
            Members = members;
        }

        public string Name { get; private set; }

        public IEnumerable<ElaClassMember> Members { get; private set; }
    }
}
