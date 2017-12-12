using System;

namespace Ela.Parsing
{
    public abstract class SourceBuffer
    {
        public abstract string FileName { get; }

        protected internal abstract int Pos { get; set; }

        protected internal abstract string GetString(int start, int end);

        protected internal abstract int Peek();

        protected internal abstract int Read();
    }
}
