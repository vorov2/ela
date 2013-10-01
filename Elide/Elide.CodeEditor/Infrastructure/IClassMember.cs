using System;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IClassMember
    {
        string Name { get; }

        int Components { get; }

        int Signature { get; }
    }
}
