using System;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    public sealed class TaskProviderInfo : ExtInfo
    {
        internal TaskProviderInfo(string key, string editorKey, Type type) : base(key)
        {
            EditorKey = editorKey;
            Type = type;
        }

        public string EditorKey { get; private set; }

        public Type Type { get; private set; }
    }
}
