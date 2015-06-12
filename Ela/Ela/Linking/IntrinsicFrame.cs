using System;
using Ela.Compilation;
using Ela.Runtime;

namespace Ela.Linking
{
    public sealed class IntrinsicFrame : CodeFrame
    {
        internal IntrinsicFrame(ElaValue[] mem)
        {
            Memory = mem;
        }

        internal ElaValue[] Memory { get; private set; }
    }
}
