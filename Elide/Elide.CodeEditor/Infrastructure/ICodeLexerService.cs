using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeLexerService : IExtService
    {
        ICodeLexer GetLexer(string editorKey);
    }
}
