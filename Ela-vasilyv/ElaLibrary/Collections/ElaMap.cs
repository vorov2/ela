using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class ElaMap : ElaObject, IEnumerable<ElaValue>
    {
        #region Construction
        public static readonly ElaMap Empty = new ElaMap(AvlTree.Empty);
		private const string TYPENAME = "map";
		
        internal ElaMap(AvlTree tree)
        {
            Tree = tree;
        }
        #endregion


        #region Methods
        public override ElaPatterns GetSupportedPatterns()
        {
            return ElaPatterns.None;
        }


        protected override string GetTypeName()
		{
			return TYPENAME;
		}


		public ElaMap Add(ElaValue key, ElaValue value)
        {
            return new ElaMap(Tree.Add(key, value));
        }


        public ElaMap Remove(ElaValue key)
        {
            return new ElaMap(Tree.Remove(key));
        }


        public bool Contains(ElaValue key)
        {
            return !Tree.Search(key).IsEmpty;
        }


        public IEnumerator<ElaValue> GetEnumerator()
        {
            foreach (var kv in Tree.Enumerate())
                yield return kv.Value;
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable<ElaValue>)this).GetEnumerator();
        }

        protected override string Show(ElaValue @this, ShowInfo info, ExecutionContext ctx)
        {
            var sb = new StringBuilder();
            sb.Append("map");
            sb.Append('{');
            var c = 0;

            foreach (var e in Tree.Enumerate())
            {
                if (c++ > 0)
                    sb.Append(',');

                sb.Append(e.Key.Show(info, ctx));
                sb.Append('=');
                sb.Append(e.Value.Show(info, ctx));
            }

            sb.Append('}');
            return sb.ToString();
        }


        protected override ElaValue GetValue(ElaValue index, ExecutionContext ctx)
        {
            var res = Tree.Search(index);

            if (res.IsEmpty)
            {
                ctx.IndexOutOfRange(index, new ElaValue(this));
                return Default();
            }

            return res.Value;
        }


        protected override ElaValue GetLength(ExecutionContext ctx)
        {
            return new ElaValue(Length);
        }


        protected override ElaValue Convert(ElaValue @this, ElaTypeInfo type, ExecutionContext ctx)
        {
            if (type.ReflectedTypeCode == ElaTypeCode.Record)
                return new ElaValue(ConvertToRecord());

            ctx.ConversionFailed(@this, type.ReflectedTypeName);
            return Default();
        }


        private ElaRecord ConvertToRecord()
        {
            var fields = new ElaRecordField[Length];
            var c = 0;

            foreach (var kv in Tree.Enumerate())
                fields[c++] = new ElaRecordField(kv.Key.ToString(), kv.Value);

            return new ElaRecord(fields);
        }
        #endregion


        #region Properties
        internal AvlTree Tree { get; private set; }

        public int Length
        {
            get
            {
                var c = 0;

                foreach (var _ in Tree.Enumerate())
                    c++;

                return c;
            }
        }
        #endregion
    }		
}
