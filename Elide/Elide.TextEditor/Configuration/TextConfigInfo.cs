using System;
using Elide.Core;
using Elide.Scintilla.ObjectModel;

namespace Elide.TextEditor.Configuration
{
    public sealed class TextConfigInfo : ExtInfo
    {
        public TextConfigInfo(string key, string display, TextConfigOptions options) : base(key)
        {
            Display = display;
            Options = options;
        }

        public override string ToString()
        {
            return Display;
        }

        public TextConfigOptions Options { get; private set; }

        public string Display { get; private set; }

        public bool? UseTabs { get; set; }

        public IndentMode? IndentMode { get; set; }

        public int? TabSize { get; set; }

        public int? IndentSize { get; set; }
    }
}
