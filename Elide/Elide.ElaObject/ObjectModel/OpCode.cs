using System;
using Ela.Compilation;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class OpCode
    {
        internal OpCode(int offset, Op op, int? arg)
        {
            Offset = offset;
            Op = op;
            Argument = arg;
        }

        public int Offset { get; private set; }

        public Op Op { get; private set; }

        public int? Argument { get; private set; }
    }
}
