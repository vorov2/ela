using System;
using System.Collections.Generic;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IClass : ILocationBounded
    {
        string Name { get; }

        IEnumerable<IClassMember> Members { get; }
    }
}
