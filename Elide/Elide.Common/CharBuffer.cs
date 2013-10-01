using System;

namespace Elide
{
    public sealed class CharBuffer
    {
        private char[] buffer;
        private int anchor;

        public CharBuffer(string source) : this(source.ToCharArray())
        {

        }

        public CharBuffer(char[] buffer)
        {
            this.buffer = buffer;
            Offset = -1;
        }
        
        public string GetString(int startPos, int length)
        {
            return new String(buffer, startPos, length);
        }

        public char GetCurrent()
        {
            return SafeGet(Offset);
        }

        public char GetNext()
        {
            return SafeGet(Offset + 1);
        }

        public char Get(int lookup)
        {
            return SafeGet(Offset + lookup);
        }

        public char GetPrevious()
        {
            return SafeGet(Offset - 1);
        }

        public bool MoveNext()
        {
            var ret = ++Offset < buffer.Length;

            if (!ret)
                Offset = Length - 1;

            return ret;
        }

        public int SetAnchor()
        {
            return anchor = Offset;
        }

        public void BackToAnchor()
        {
            Offset = anchor;
        }

        public bool Skip(int chars)
        {
            Offset += chars;
            var ret = Offset < buffer.Length && Offset > 0;

            if (!ret)
                Offset -= chars;

            return ret;
        }

        private char SafeGet(int index)
        {
            return index < 0 || index >= buffer.Length ?
                default(Char) : buffer[index];
        }
        
        public int Offset { get; private set; }
        
        public bool Eof
        {
            get { return Offset == buffer.Length - 1; }
        }
        
        public int Length
        {
            get { return buffer.Length; }
        }
    }
}