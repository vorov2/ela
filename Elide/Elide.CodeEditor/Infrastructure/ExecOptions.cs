using System;

namespace Elide.CodeEditor.Infrastructure
{
    public enum ExecOptions
    {
        None = 0x00,

        Annotation = 0x02,

        PrintResult = 0x04,

        //ReadLine = 0x08,

        Console = 0x10,

        ShowOutput = 0x20,

        LimitTime = 0x40,

        TipResult = 0x80,

        TipError = 0x100
    }
}
