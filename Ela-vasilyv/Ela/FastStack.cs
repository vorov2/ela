using System;
using System.Collections;
using System.Collections.Generic;

namespace Ela
{
	internal class FastStack<T> : IEnumerable<T>
	{
		private const int DEFAULT_SIZE = 4;
		private T[] array;
		private int size;
		private int initialSize;

		internal FastStack() : this(DEFAULT_SIZE)
		{

		}
        
		internal FastStack(int size)
		{
			this.initialSize = size;
			array = new T[size];
		}
		
        public IEnumerator<T> GetEnumerator()
		{
			for (var i = 0; i < size; i++)
				yield return array[i];
		}
        
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
        
		public void Clear(int offset)
		{
			var newArr = new T[array.Length];
			Array.Copy(array, 0, newArr, 0, offset);
			array = newArr;
		}
        
		internal FastStack<T> Clone()
		{
			var ret = (FastStack<T>)MemberwiseClone();
			ret.array = (T[])array.Clone();
			return ret;
		}
        
		internal void Clear()
		{
			size = 0;
			array = new T[initialSize];
		}
        
		internal T Pop()
		{
			var ret = array[--size];
			array[size] = default(T);
			return ret;
		}
        
		internal T Peek()
		{
			return array[size - 1];
		}
        
		internal void Push(T val)
		{
			if (size == array.Length)
			{
				var dest = new T[array.Length * 2];
				Array.Copy(array, 0, dest, 0, size);
				array = dest;
			}

			array[size++] = val;
		}
        
		internal void Trim(int num)
		{
			var iter = size - num;

			for (var i = size; i > iter; --i)
				array[i - 1] = default(T);

			size -= num;
		}
        
		internal void Replace(T val)
		{
			array[size - 1] = val;
		}
		
		internal int Count
		{
			get { return size; }
		}
        
		internal T this[int index]
		{
			get { return array[index]; }
			set { array[index] = value; }
		}
	}
}