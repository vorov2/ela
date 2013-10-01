using System;

namespace Elide.TextEditor.Configuration
{
    [Flags]
    public enum TextConfigOptions
    {
        None = 0,

        RestrictTabs = 0x02,

        RestrictWordWrap = 0x04
    }
}
