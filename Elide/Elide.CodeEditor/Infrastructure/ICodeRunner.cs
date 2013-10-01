using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeRunner<out T> where T : ICompiledAssembly
    {
        object Execute(ICompiledAssembly compiled, params ExtendedOption[] options);

        IApp App { get; set; }
    }
}
