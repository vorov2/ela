using System;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ILocationBounded
    {
        Location Location { get; }
    }
}
