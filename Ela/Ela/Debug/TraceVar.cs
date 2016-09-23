using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.Debug
{
    public sealed class TraceVar
    {
        internal TraceVar(string name, Ela.Runtime.ElaValue value, bool local)
        {
            Name = name;
            Value = value;
            Local = local;
        }

        public string Name { get; private set; }

        public Ela.Runtime.ElaValue Value { get; private set; }

        public bool Local { get; private set; }
    }
}
