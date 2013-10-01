using System;

namespace Elide.Scintilla.ObjectModel
{
    public sealed class DwellEventArgs : EventArgs
    {
        internal DwellEventArgs(int x, int y, int position)
        {
            X = x;
            Y = y;
            Position = position;
        }

        public int X { get; private set; }

        public int Y { get; private set; }

        public int Position { get; private set; }
    }
}
