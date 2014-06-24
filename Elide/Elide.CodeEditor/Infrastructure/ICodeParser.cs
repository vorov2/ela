using System;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeParser<out T> where T : IAst
    {
        T Parse(string source, Document doc, IBuildLogger logger);

        IApp App { get; set; }
    }
}
