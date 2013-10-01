using System;
using System.Collections.Generic;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICompiledAssembly
    {
        ICompiledUnit MainUnit { get; }

        IEnumerable<ICompiledUnit> Units { get; }
    }
}
