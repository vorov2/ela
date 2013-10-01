using System;
using System.Collections.Generic;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICompiledUnit
    {
        string Name { get; }

        IEnumerable<CodeName> Globals { get; }

        IEnumerable<IType> Types { get; }

        IEnumerable<IClassInstance> Instances { get; }

        IEnumerable<IClass> Classes { get; }

        IEnumerable<IReference> References { get; }
    }
}
