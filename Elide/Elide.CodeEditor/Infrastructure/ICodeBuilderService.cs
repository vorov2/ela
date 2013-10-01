using System;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeBuilderService : IExtService
    {
        T RunBuilder<T>(string source, Document doc, BuildOptions options, params ExtendedOption[] extOptions) where T : ICompiledAssembly;
    }
}
