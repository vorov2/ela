using System;
using System.Collections;
using System.Collections.Generic;

namespace Ela.Runtime.ObjectModel
{
	public class ElaString : ElaObject, IEnumerable<ElaValue>
	{
		public static readonly ElaString Empty = new ElaString(String.Empty);
		
		public ElaString(string value) : base(ElaTypeCode.String)
		{
			this.Value = value ?? String.Empty;
		}
		
        public IEnumerator<ElaValue> GetEnumerator()
		{
			foreach (var c in Value)
				yield return new ElaValue(c);
		}
        
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
        
        public override string ToString(string format, IFormatProvider provider)
        {
            return Value;
        }
        
        public ElaList ToList()
        {
            var str = Value;
            var xs = ElaList.Empty;

            for (var i = str.Length - 1; i > -1; i--)
                xs = new ElaList(xs, new ElaValue(str[i]));

            return xs;
        }
		
		internal ElaValue GetValue(ElaValue key, ExecutionContext ctx)
		{
			if (key.TypeId != ElaMachine.INT)
			{
				ctx.InvalidIndexType(key);
				return Default();
			}

			if (key.I4 >= Value.Length || key.I4 < 0)
			{
				ctx.IndexOutOfRange(key, new ElaValue(this));
				return Default();
			}

			return new ElaValue(Value[key.I4]);
		}

		protected internal override ElaValue Tail(ExecutionContext ctx)
		{
            return new ElaValue(new ElaString(Value.Substring(1)));
		}
        
        public string Value { get; private set; }
    }
}
