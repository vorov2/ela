using System;
using System.Collections;
using System.Collections.Generic;

namespace Ela.Runtime
{
    internal class CallStack : IEnumerable<CallPoint>
    {
        private const int DEFAULT_SIZE = 4;
        private CallPoint[] array;
        private int initialSize;

        internal CallStack() : this(DEFAULT_SIZE)
        {

        }

        internal CallStack(int size)
        {
            this.initialSize = size;
            array = new CallPoint[size];
        }

        public IEnumerator<CallPoint> GetEnumerator()
        {
            for (var i = 0; i < Count; i++)
                yield return array[i];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Clear()
        {
            Count = 0;
            array = new CallPoint[initialSize];
        }

        internal CallPoint Pop()
        {
            var ret = array[--Count];
            array[Count] = null;
            return ret;
        }

        internal CallPoint Peek()
        {
            return array[Count - 1];
        }

        internal void Push(CallPoint val)
        {
            if (Count == array.Length)
            {
                var dest = new CallPoint[array.Length * 2];
                
                for (var i = 0; i < Count; i++)
                    dest[i] = array[i];

                array = dest;
            }

            array[Count++] = val;
        }

        internal int Count;

        internal CallPoint this[int index]
        {
            get { return array[index]; }
            set { array[index] = value; }
        }
    }
}