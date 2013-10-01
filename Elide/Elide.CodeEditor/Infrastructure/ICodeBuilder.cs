using System;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeBuilder<out T> where T : ICompiledAssembly
    {
        T Build(string source, Document doc, IBuildLogger logger, params ExtendedOption[] options);

        IApp App { get; set; }
    }
}
