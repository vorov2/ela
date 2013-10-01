using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class CodeRunnerInfo : ExtInfo
    {
        internal CodeRunnerInfo(string key, Type type) : base(key)
        {
            Type = type;
        }

        public Type Type { get; private set; }
    }
}
