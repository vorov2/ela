using System;
using System.Collections.Generic;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaRecord : ElaObject, IEnumerable<ElaRecordField>
	{
		internal readonly string[] keys;
        internal readonly ElaValue[] values;

		public ElaRecord(params ElaRecordField[] fields) : base(ElaTypeCode.Record)
		{
			keys = new string[fields.Length];
            values = new ElaValue[fields.Length];

			for (var i = 0; i < fields.Length; i++)
				SetField(i, fields[i].Field, fields[i].Value);
		}
        
		internal ElaRecord(int size) : base(ElaTypeCode.Record)
		{
			keys = new string[size];
			values = new ElaValue[size];
		}
		
        public static ElaRecord Concat(ElaRecord left, ElaRecord right)
        {
            var list = new List<ElaRecordField>();
            list.AddRange(left);
            list.AddRange(right);
            return new ElaRecord(list.ToArray());
        }

        internal ElaValue GetValue(ElaValue key, ExecutionContext ctx)
		{
            var idx = key.I4;

            if (key.TypeId != ElaMachine.INT)
            {
                ctx.InvalidIndexType(key);
                return Default();
            }
            
            if (idx == -1 || idx > values.Length - 1)
            {
                ctx.IndexOutOfRange(key, new ElaValue(this));
                return Default();
            }

			return values[idx];
        }

        internal ElaValue GetField(ElaValue key, ExecutionContext ctx)
        {
            if (key.TypeId != ElaMachine.STR)
            {
                ctx.InvalidType(TCF.GetShortForm(ElaTypeCode.String), key);
                return Default();
            }

            var idx = GetOrdinal(key.DirectGetString());

            if (idx == -1)
            {
                ctx.UnknownField(key.DirectGetString(), new ElaValue(this));
                return Default();
            }

            return values[idx];
        }

        internal bool HasField(ElaValue key, ExecutionContext ctx)
        {
            var idx = key.I4;

            if (key.TypeId != ElaMachine.STR)
            {
                ctx.InvalidType(TCF.GetShortForm(ElaTypeCode.String), key);
                return false;
            }

            return GetOrdinal(key.DirectGetString()) != -1;
        }

		public override string ToString(string format, IFormatProvider provider)
        {
			var sb = new StringBuilder();
			sb.Append('{');

			var c = 0;

			for (var i = 0; i < keys.Length; i++)
			{
                var k = keys[i];

				if (c++ > 0)
					sb.Append(",");

				sb.AppendFormat("{0}={1}", k, values[i]);
			}

			sb.Append('}');
			return sb.ToString();
		}

        public IEnumerator<ElaRecordField> GetEnumerator()
        {
            for (var i = 0; i < keys.Length; i++)
                yield return new ElaRecordField(keys[i], values[i]);
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

		public IEnumerable<String> GetKeys()
		{
			return keys;
		}
        
		internal void SetField(int index, string key, ElaValue value)
		{
			keys[index] = key;
			values[index] = value;
		}

        internal ElaValue GetKey(ElaValue index, ExecutionContext ctx)
        {
            if (index.TypeId != ElaMachine.INT)
            {
                ctx.InvalidIndexType(index);
                return Default();
            }

            if (index.I4 < 0 || index.I4 > keys.Length - 1)
            {
                ctx.IndexOutOfRange(index, new ElaValue(this));
                return Default();
            }

            return new ElaValue(keys[index.I4]);
        }

		private int GetOrdinal(string key)
		{
			for (var i = 0; i < keys.Length; i++)
				if (keys[i] == key)
					return i;

			return -1;
		}

        public int Length
        {
            get { return keys.Length; }
        }
        
		public ElaValue this[string key]
		{
			get
			{
				var index = GetOrdinal(key);

				if (index > -1 && index < values.Length)
					return values[index];
				else
					throw new IndexOutOfRangeException();
			}
		}
        
		public ElaValue this[int index]
		{
			get
			{
				if (index > -1 && index < values.Length)
					return values[index];
				else
					throw new IndexOutOfRangeException();
			}
		}
    }
}
