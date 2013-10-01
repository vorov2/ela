using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

namespace Elide.Console
{
    [Serializable]
    public sealed class HistoryList<T> : IEnumerable<T>
    {
        private T[] buffer;
        private int position;

        public HistoryList(int size)
        {
            buffer = new T[size];
        }

        public HistoryList(int size, IEnumerable<T> items)
        {
            buffer = new T[size];
            items.Take(size).ForEachIndex((e,i) => buffer[i] = e);
        }
        
        public IEnumerator<T> GetEnumerator()
        {
            for (var i = 0; i < Size; i++)
                yield return buffer[i];
        }
        
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        
        public T Peek()
        {
            return buffer[position];
        }
        
        public T Next()
        {
            if (position == Size - 1)
                return buffer[position];
            else
                return buffer[++position];
        }
        
        public T Previous()
        {
            if (position == 0)
                return buffer[position];
            else
                return buffer[--position];
        }
        
        public void Add(T value)
        {
            Remove(value);

            if (Size == buffer.Length && Size > 0)
            {
                Array.Copy(buffer, 1, buffer, 0, buffer.Length - 1);
                buffer[Size - 1] = value;
            }
            else
                buffer[Size++] = value;

            position = Size - 1;
        }
        
        public void Remove(T value)
        {
            for (var i = 0; i < Size; i++)
            {
                var it = buffer[i];

                if (Object.Equals(it, value))
                {
                    var len = buffer.Length - 1;
                    var arr = new T[len < 10 ? 10 : len];

                    if (i == 0)
                        Array.Copy(buffer, 1, arr, 0, buffer.Length - 1);
                    else
                    {
                        Array.Copy(buffer, 0, arr, 0, i);
                        Array.Copy(buffer, i + 1, arr, i, buffer.Length - i - 1);
                    }

                    buffer = arr;
                    --Size;
                    break;
                }
            }
        }
        
        public void Clear()
        {
            buffer = new T[buffer.Length];
            Size = 0;
        }
       
        public int Size { get; private set; }
    }
}
