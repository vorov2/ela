using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
	public class ElaList : ElaObject, IEnumerable<ElaValue>
	{		
        public static readonly ElaList Empty = ElaNilList.Instance;

        private sealed class ElaNilList : ElaList
		{
			internal static readonly ElaNilList Instance = new ElaNilList();

			internal ElaNilList() : base(null, new ElaValue(ElaUnit.Instance)) { }

			protected internal override ElaValue Tail(ExecutionContext ctx)
			{
				ctx.Fail("prelude: Empty list.");
				return Default();
			}
		}
        
		public ElaList(ElaList next, object value) : this(next, ElaValue.FromObject(value))
		{

		}
        
		public ElaList(ElaList next, ElaValue value) : base(ElaTypeCode.List)
		{
			InternalNext = next;
			InternalValue = value;
		}		

		internal int GetLength()
		{
            ElaList xs = this;
            var count = 0;

            while (xs != Empty)
            {
                count++;

                var tl = xs.Tail().Ref;
                xs = tl as ElaList;

                if (xs == null)
                    throw InvalidDefinition();
            }

            return count;
		}
        
		protected internal override ElaValue Tail(ExecutionContext ctx)
		{
			return new ElaValue(InternalNext);
		}

        internal override ElaValue Cons(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(new ElaList(this, value));
        }
                
		protected internal override ElaValue GenerateFinalize(ExecutionContext ctx)
		{
            return new ElaValue(Reverse());
		}
        
        public override string ToString(string format, IFormatProvider formatProvider)
        {
            var sb = new StringBuilder();
            sb.Append('[');
            var c = 0;

            foreach (var v in this)
            {
                if (c++ > 0)
                    sb.Append(',');

                sb.Append(v);
            }

            sb.Append(']');
            return sb.ToString();
        }

        public static ElaList FromEnumerable(IEnumerable seq)
		{
			var list = ElaList.Empty;

			foreach (var e in seq)
				list = new ElaList(list, ElaValue.FromObject(e));

			return list.Reverse();
		}
        
		public static ElaList FromEnumerable(IEnumerable<ElaValue> seq)
		{
			var list = ElaList.Empty;

			foreach (var e in seq)
				list = new ElaList(list, e);

			return list.Reverse();
		}

		public virtual ElaList Reverse()
		{
            var newLst = ElaList.Empty;
			var lst = this;

            while (lst != Empty)
            {
				newLst = new ElaList(newLst, lst.Value);
				lst = lst.InternalNext;
			}

			return newLst;
		}

        public virtual ElaValue Tail()
        {
            return new ElaValue(InternalNext);
        }

        public IEnumerator<ElaValue> GetEnumerator()
		{
			ElaList xs = this;

			while (xs != Empty)
			{
				yield return xs.InternalValue;

                var tl = xs.Tail().Ref;
				xs = tl as ElaList;

				if (xs == null)
					throw InvalidDefinition();
			}
		}
        
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
        
		protected virtual Exception InvalidDefinition()
		{
            return new ElaException("Invalid list definition.");
		}

		protected internal ElaValue InternalValue;
		protected ElaList InternalNext;

		public virtual ElaList Next
		{
			get
			{
                if (InternalNext == null)
                    return null;
				else
					return InternalNext;
			}
		}

		public virtual ElaValue Value
		{
			get { return InternalValue; }
		}

		public virtual int Length
		{
			get { return GetLength(); }
		}
	}
}
