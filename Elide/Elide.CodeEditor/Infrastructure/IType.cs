using System;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IType : ILocationBounded
    {
        string Name { get; }
    }
}
