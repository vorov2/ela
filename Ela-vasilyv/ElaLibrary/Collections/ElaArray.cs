using System;
using System.Collections;
using System.Collections.Generic;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
	public sealed class ElaArray : ElaObject, IEnumerable<ElaValue>
	{
		#region Construction
		private const int DEFAULT_SIZE = 4;
		private const string TYPENAME = "array";
		private int size;
		private ElaValue[] array;
		private int headIndex;

		public ElaArray(ElaValue[] arr)
		{
			if (arr == null)
				throw new ArgumentNullException("arr");

			array = new ElaValue[arr.Length == 0 ? DEFAULT_SIZE : arr.Length];

			if (arr.Length != 0)
			{
				Array.Copy(arr, array, arr.Length);
				size = array.Length;
			}
		}


		public ElaArray(object[] arr)
		{
			if (arr == null)
				throw new ArgumentNullException("arr");

			array = new ElaValue[arr.Length == 0 ? DEFAULT_SIZE : arr.Length];

			for (var i = 0; i < arr.Length; i++)
				array[i] = ElaValue.FromObject(arr[i]);

			if (arr.Length != 0)
				size = array.Length;
		}


		public ElaArray(int size)
		{
			array = new ElaValue[size == 0 ? DEFAULT_SIZE : size];
		}


		public ElaArray() : this(DEFAULT_SIZE)
		{

		}


		private ElaArray(ElaValue[] arr, int size, int headIndex)
		{
			array = arr;
			this.size = size;
			this.headIndex = headIndex;
		}
		#endregion
		

		#region Operations
		protected override ElaValue GetLength(ExecutionContext ctx)
		{
			return new ElaValue(size - headIndex);
		}


		protected override ElaValue GetValue(ElaValue index, ExecutionContext ctx)
		{
			if (index.TypeCode != ElaTypeCode.Integer)
			{
				ctx.InvalidIndexType(index);
				return Default();
			}

            var idx = index.AsInteger();

            if (idx >= Length || idx < 0)
			{
				ctx.IndexOutOfRange(index, new ElaValue(this));
				return Default();
			}

            return array[idx];
		}


        //protected override void SetValue(ElaValue index, ElaValue value, ExecutionContext ctx)
        //{
        //    if (index.TypeCode != ElaTypeCode.Integer)
        //    {
        //        ctx.InvalidIndexType(index);
        //        return;
        //    }

        //    var idx = index.AsInteger();

        //    if (idx < 0 || idx >= Length)
        //    {
        //        ctx.IndexOutOfRange(index, new ElaValue(this));
        //        return;
        //    }

        //    array[idx] = value;
        //}


		protected override ElaValue Head(ExecutionContext ctx)
		{
			return array[headIndex];
		}


		protected override ElaValue Tail(ExecutionContext ctx)
		{
			return new ElaValue(new ElaArray(array, size, headIndex + 1));
		}


		protected override bool IsNil(ExecutionContext ctx)
		{
			return headIndex == size;
		}


		protected override ElaValue Generate(ElaValue value, ExecutionContext ctx)
		{
			Add(value);
			return new ElaValue(this);			
		}


		protected override ElaValue GenerateFinalize(ExecutionContext ctx)
		{
			return new ElaValue(this);
		}


		protected override ElaValue Concatenate(ElaValue left, ElaValue right, ExecutionContext ctx)
		{
			if (left.Is<ElaArray>() && right.Is<ElaArray>())
			{
				var thisArr = left.As<ElaArray>();
				var otherArr = right.As<ElaArray>();
				var arr = new ElaValue[thisArr.Length + otherArr.Length];
				Array.Copy(thisArr.array, 0, arr, 0, thisArr.Length);
				Array.Copy(otherArr.array, 0, arr, thisArr.Length, otherArr.Length);
				return new ElaValue(new ElaArray(arr, arr.Length, 0));
			}
			else if (left.Is<ElaArray>())
				return right.Concatenate(left, right, ctx);
			
			ctx.InvalidLeftOperand(left, right, "concat");
			return Default();
		}


        protected override string Show(ElaValue @this, ShowInfo info, ExecutionContext ctx)
		{
			return "array[" + FormatHelper.FormatEnumerable((IEnumerable<ElaValue>)this, ctx, info) + "]";
		}


		protected override ElaValue Cons(ElaObject instance, ElaValue value, ExecutionContext ctx)
		{
			var arr = (ElaArray)instance;

			if (arr == null)
			{
				ctx.InvalidType(GetTypeName(), new ElaValue(instance));
				return Default();
			}

			if (arr.headIndex > 0)
			{
				var newArr = new ElaArray();
				arr.Copy(arr.headIndex, newArr);
				arr = newArr;
			}

			if (arr.Length == 0)
				arr.Add(value);
			else
				arr.Insert(0, value);

			return new ElaValue(arr);
		}


		protected override ElaValue Nil(ExecutionContext ctx)
		{
			return new ElaValue(new ElaArray());
		}
		#endregion


		#region Methods
        public override ElaPatterns GetSupportedPatterns()
        {
            return ElaPatterns.Tuple|ElaPatterns.HeadTail;
        }


		protected override string GetTypeName()
		{
			return TYPENAME;
		}


		public IEnumerator<ElaValue> GetEnumerator()
		{
			for (var i = 0; i < Length; i++)
				yield return array[i];
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		internal ElaValue FastGet(int index)
		{
			return index < Length ? array[index] : Default();
		}


		internal void FastSet(int index, ElaValue value)
		{
			array[index] = value;
		}


		public void Add(ElaValue value)
		{
			if (size == array.Length)
				EnsureSize(size + 1);

			array[size] = value;
			size++;
		}


		public bool Remove(int index)
		{
			index += headIndex;

			if (index < 0 || index >= size)
				return false;
			else
			{
				if (index < --size)
					Array.Copy(array, index + 1, array, index, size - index);

				array[size] = default(ElaValue);
				return true;
			}
		}


		public bool Insert(int index, ElaValue value)
		{
			index += headIndex;

			if (index < 0 || index >= size)
				return false;
			else
			{
				if (size == array.Length)
					EnsureSize(size + 1);

				if (index < size)
					Array.Copy(array, index, array, index + 1, size - index);

				array[index] = value;
				size++;
				return true;
			}
		}


		public void Clear()
		{
			if (size > 0)
			{
				Array.Clear(array, 0, size);
				size = 0;
				headIndex = 0;
			}
		}


		internal void Copy(int offset, ElaArray elaArr)
		{
			Array.Copy(elaArr.array, 0, array, offset, elaArr.Length);
			size += elaArr.size;
		}


		private void EnsureSize(int newSize)
		{
			if (array.Length < newSize)
			{
				var newArr = new ElaValue[array.Length == 0 ? DEFAULT_SIZE : array.Length * 2];
				Array.Copy(array, newArr, array.Length);
				array = newArr;
			}
		}
		#endregion


		#region Properties
		public int Length
		{
			get { return size - headIndex; }
		}


		public ElaValue this[int index]
		{
			get
			{
				if (index < Length && index > -1)
					return array[index];
				else
					throw new IndexOutOfRangeException();
			}
			set
			{
				if (index > -1 && index < Length)
					array[index] = value;
				else
					throw new IndexOutOfRangeException();
			}
		}
		#endregion
	}
}