using System;
using System.Collections.Generic;
using System.Linq;
using Ela.Compilation;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class TypeClass : IClass
    {
        internal TypeClass(string className, ClassData classData, Location loc)
        {
            Name = className;
            Members = classData.GetMembers().Select(m => new TypeClassMember(m.Name, m.Components, m.Mask)).ToList();
            Location = loc;
        }

        public string Name { get; private set; }

        public Location Location { get; private set; }

        public IEnumerable<IClassMember> Members { get; private set; }
    }
}
