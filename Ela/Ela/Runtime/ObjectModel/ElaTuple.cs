using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaTuple : ElaObject, IEnumerable<ElaValue>
	{
        public ElaTuple(params object[] args) : base(ElaTypeCode.Tuple)
		{
			Values = new ElaValue[args.Length];

            for (var i = 0; i < args.Length; i++)
                Values[i] = ElaValue.FromObject(args[i]);
		}

		public ElaTuple(params ElaValue[] args) : base(ElaTypeCode.Tuple)
		{
			Values = args;
		}

		internal ElaTuple(int size) : base(ElaTypeCode.Tuple)
		{
            Values = new ElaValue[size];
		}

        public static ElaTuple Concat(ElaTuple left, ElaTuple right)
        {
            var arr1 = left.Values;
            var arr2 = right.Values;
            var res = new ElaValue[arr1.Length + arr2.Length];
            arr1.CopyTo(res, 0);
            arr2.CopyTo(res, arr1.Length);
            return new ElaTuple(res);
        }
        
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append('(');
            var c = 0;

            foreach (var v in this)
            {
                if (c++ > 0)
                    sb.Append(',');

                sb.Append(v);
            }

            if (Length == 1)
                sb.Append(',');

            sb.Append(')');
            return sb.ToString();
        }
                
		public IEnumerator<ElaValue> GetEnumerator()
		{
			for (var i = 0; i < Values.Length; i++)
                yield return Values[i];
		}
        
        IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		internal void InternalSetValue(int index, ElaValue value)
		{
            Values[index] = value;
		}

        internal ElaValue[] Values;

		public ElaValue this[int index]
		{
			get
			{
				if (index < Length && index > -1)
					return Values[index];
				else
					throw new IndexOutOfRangeException();
			}
		}
        
		public int Length 
		{ 
			get { return Values.Length; } 
		}
	}
}
