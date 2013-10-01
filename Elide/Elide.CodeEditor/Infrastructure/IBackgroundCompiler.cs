using System;
using System.Collections.Generic;
using Elide.CodeEditor.Views;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IBackgroundCompiler
    {
        Tuple<ICompiledUnit,IEnumerable<MessageItem>> Compile(CodeDocument doc, string source);
    }
}
