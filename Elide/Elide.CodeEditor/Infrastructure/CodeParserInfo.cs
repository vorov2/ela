using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class CodeParserInfo : ExtInfo
    {
        internal CodeParserInfo(string key, Type type) : base(key)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
