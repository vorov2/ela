using System;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IClassInstance : ILocationBounded
    {
        string Class { get; }

        string Type { get; }
    }
}
