using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeRunnerService : IExtService
    {
        bool RunCode<T>(T compiled, ExecOptions options, params ExtendedOption[] extOptions) where T : ICompiledAssembly;

        bool AbortExecution();

        bool IsRunning();
    }
}
