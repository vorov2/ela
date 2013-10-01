using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class CodeLexerInfo : ExtInfo
    {
        internal CodeLexerInfo(string key, Type type) : base(key)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
