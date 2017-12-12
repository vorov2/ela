using System;
using System.IO;

namespace Ela.Parsing
{
    public sealed class StringBuffer : SourceBuffer
    {
        internal const int EOF = char.MaxValue + 1;

        private readonly char[] buffer;
        private int bufferPosition;
        private int bufferLen;

        public StringBuffer(string value)
        {
            this.buffer = value.ToCharArray();
            this.bufferLen = this.buffer.Length;
        }

        public StringBuffer(string value, string fileName) : this(value)
        {
            this.fileName = fileName;
        }

        public static SourceBuffer FromFile(string file)
        {
            using (var sr = new StreamReader(File.OpenRead(file)))
                return new StringBuffer(sr.ReadToEnd(), file);
        }

        protected internal override int Read()
        {
            if (this.bufferPosition < this.bufferLen)
                return this.buffer[this.bufferPosition++];
            else
                return EOF;
        }

        protected internal override int Peek()
        {
            var curPos = Pos;
            var ch = Read();
            Pos = curPos;
            return ch;
        }

        protected internal override string GetString(int start, int end)
        {
            var len = 0;
            var buf = new char[end - start];
            var oldPos = Pos;
            Pos = start;

            while (Pos < end)
                buf[len++] = (char)Read();

            Pos = oldPos;
            return new String(buf, 0, len);
        }

        protected internal override int Pos
        {
            get { return this.bufferPosition; }
            set
            {
                if (value < 0 || value > bufferLen)
                    throw new ElaParserException(string.Format("Выход за пределы диапазона в буфере парсера MScript, позиция: {0}.", value));

                this.bufferPosition = value;
            }
        }

        private string fileName;
        public override string FileName
        {
            get { return this.fileName; }
        }
    }
}
