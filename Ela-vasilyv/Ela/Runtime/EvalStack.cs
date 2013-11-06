using System;
using System.Collections;
using System.Collections.Generic;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime
{
	internal class EvalStack : IEnumerable<ElaValue>
	{
		private ElaValue[] array;
		private int size;
		private int initialSize;
        private static readonly ElaValue[] bigArray = new ElaValue[100];
        internal readonly bool tail;

		internal EvalStack(int size, bool tail)
		{
			this.initialSize = size;
            this.tail = tail;

            //It DOESN'T create a memory leak, we can safely reuse an already allocated
            //array here. This is because eval stack *after* execution of a function is 
            //should be always empty.
            array = tail && size <= 100 ? bigArray : new ElaValue[size];            
		}

		public IEnumerator<ElaValue> GetEnumerator()
		{
			for (var i = 0; i < size; i++)
				yield return array[i];
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
        
		internal void Clear(int offset)
		{
			if (size > 0)
			{
				var newArr = new ElaValue[initialSize];
				Array.Copy(array, 0, newArr, 0, offset);
				array = newArr;
				size = offset;
			}
			else
			{
				array = new ElaValue[initialSize];
				size = 0;
			}
		}

		internal void PopVoid()
		{
			--size;

			if (array[size].Ref.TypeId > 5)
				array[size].Ref = null;
		}
        
		internal ElaValue PopFast()
		{
			return array[--size];
		}

        internal ElaValue Pop()
		{
            --size;
            var ret = array[size];

            if (ret.Ref.TypeId > 5)
                array[size].Ref = null;

            return ret;
		}
        
		internal ElaValue Peek()
		{
			return array[size - 1];
		}
        
		internal void Push(ElaValue val)
		{
			array[size++] = val;
        }

        internal void Push(ElaObject val)
        {
            array[size++] = new ElaValue(val);
        }

        internal void Dup()
        {
            array[size++] = array[size - 2];
        }

        private ElaValue emptyInt = new ElaValue(ElaInteger.Instance);
		internal void Push(int val)
		{
            emptyInt.I4 = val;
            array[size++] = emptyInt;
		}
        
        private ElaValue emptyBool = new ElaValue(ElaBoolean.Instance);
        internal void Push(bool val)
		{
            emptyBool.I4 = val ? 1 :0;
            array[size++] = emptyBool;
		}
        
		internal void Replace(ElaValue val)
		{
			array[size - 1] = val;
        }

        internal void Replace(ElaObject val)
        {
            array[size - 1] = new ElaValue(val);
        }
        
		internal void Replace(int val)
		{
            emptyInt.I4 = val;
            array[size - 1] = emptyInt;
		}

        internal void Replace(bool val)
		{
            emptyBool.I4 = val ? 1 : 0;
            array[size - 1] = emptyBool;
		}

		internal int Count
		{
			get { return size; }
		}
        
		internal ElaValue this[int index]
		{
			get { return array[index]; }
		}
	}
}