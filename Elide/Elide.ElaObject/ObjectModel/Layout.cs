using System;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Layout
    {
        internal Layout(int size, int stackSize, int address)
        {
            Size = size;
            StackSize = stackSize;
            Address = address;
        }

        public int Size { get; private set; }

        public int StackSize { get; private set; }

        public int Address { get; private set; }
    }
}
