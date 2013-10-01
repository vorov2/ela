using System;
using System.Collections.Generic;
using Elide.Scintilla;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeLexer
    {
        IEnumerable<StyledToken> Parse(string source);
    }
}
