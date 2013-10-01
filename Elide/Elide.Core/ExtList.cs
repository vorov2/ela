using System;
using System.Collections.Generic;

namespace Elide.Core
{
    public sealed class ExtList<T> : IEnumerable<T>
    {
        public static readonly ExtList<T> Empty = new ExtList<T>(new T[0]);
        private readonly List<T> elements;

        public ExtList(IEnumerable<T> elements)
        {
            this.elements = new List<T>(elements);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return elements.GetEnumerator();
        }

        public ExtList<T> Merge(ExtList<T> another)
        {
            var list = new List<T>();
            list.AddRange(elements);
            list.AddRange(another.elements);
            return new ExtList<T>(list);
        }

        public int Count
        {
            get { return elements.Count; }
        }

        public T this[int index]
        {
            get { return elements[index]; }
        }
    }
}
