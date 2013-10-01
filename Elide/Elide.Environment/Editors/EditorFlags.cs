using System;

namespace Elide.Environment.Editors
{
    [Flags]
    public enum EditorFlags
    {
        None = 0,

        Hidden = 0x02,

        Default = 0x04,

        Main = 0x08,

        HiddenEverywhere = 0x10
    }
}
