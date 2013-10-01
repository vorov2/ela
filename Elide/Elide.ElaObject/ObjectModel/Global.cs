using System;
using Ela.CodeModel;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Global
    {
        internal Global(string name, ElaVariableFlags flags, int address, int data)
        {
            Name = name;
            Address = address;
            Data = data;
            Flags = flags;
        }

        public string Name { get; private set; }

        public int Address { get; private set; }

        public int Data { get; private set; }

        public ElaVariableFlags Flags { get; private set; }
    }
}
