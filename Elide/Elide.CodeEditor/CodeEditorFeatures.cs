using System;

namespace Elide.CodeEditor
{
    public enum CodeEditorFeatures
    {
        None = 0,

        Tasks = 1 << 0,

        Outline = 1 << 1,
    }
}
