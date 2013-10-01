using System;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public sealed class BackgroundCompilerInfo : ExtInfo
    {
        internal BackgroundCompilerInfo(string key, string editorKey, Type type) : base(key)
        {
            EditorKey = editorKey;
            Type = type;
        }

        public Type Type { get; private set; }

        public string EditorKey { get; private set; }
    }
}
