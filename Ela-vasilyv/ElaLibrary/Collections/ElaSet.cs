using System;
using System.Collections;
using System.Collections.Generic;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
	public sealed class ElaSet : ElaObject, IEnumerable<ElaValue>
	{
		#region Construction
		public static readonly ElaSet Empty = new ElaSet(AvlTree.Empty);
		private const string TYPENAME = "set";
		
		internal ElaSet(AvlTree tree)
		{
			Tree = tree;
		}
		#endregion


		#region Methods
        public override ElaPatterns GetSupportedPatterns()
        {
            return ElaPatterns.HeadTail;
        }


        protected override string GetTypeName()
		{
			return TYPENAME;
		}


		public static ElaSet FromEnumerable(IEnumerable<ElaValue> seq)
		{
			var set = ElaSet.Empty;

			foreach (var v in seq)
			{
				if (set.Tree.Search(v).IsEmpty)
					set = new ElaSet(set.Tree.Add(v, default(ElaValue)));
			}

			return set;
		}


		public static ElaSet FromEnumerable(IEnumerable seq)
		{
			var set = ElaSet.Empty;

            foreach (var o in seq)
            {
                var v = ElaValue.FromObject(o);

                if (set.Tree.Search(v).IsEmpty)
                    set = new ElaSet(set.Tree.Add(v, default(ElaValue)));
            }

            return set;
		}


		public ElaSet Add(ElaValue value)
		{
			if (Tree.Search(value).IsEmpty)
				return new ElaSet(Tree.Add(value, default(ElaValue)));
			else
				return this;
		}


		public ElaSet Remove(ElaValue value)
		{
			if (!Tree.Search(value).IsEmpty)
				return new ElaSet(Tree.Remove(value));
			else
				return this;
		}


		public bool Contains(ElaValue value)
		{
			return !Tree.Search(value).IsEmpty;
		}


		public IEnumerator<ElaValue> GetEnumerator()
		{
			foreach (var kv in Tree.Enumerate())
                yield return kv.Key;
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<ElaValue>)this).GetEnumerator();
		}
		#endregion


		#region Operations
		protected override string Show(ElaValue @this, ShowInfo info, ExecutionContext ctx)
		{
			return "set[" + FormatHelper.FormatEnumerable(this, ctx, info) + "]";
		}


		protected override ElaValue GetLength(ExecutionContext ctx)
		{
			return new ElaValue(Length);
		}


		protected override ElaValue Head(ExecutionContext ctx)
		{
			return Tree.Key;
		}


		protected override ElaValue Tail(ExecutionContext ctx)
		{
			return new ElaValue(new ElaSet(Tree.Remove(Tree.Key)));
		}


		protected override bool IsNil(ExecutionContext ctx)
		{
			return Tree.IsEmpty;
		}


		protected override ElaValue Generate(ElaValue value, ExecutionContext ctx)
		{
			if (!Tree.Search(value).IsEmpty)
				return new ElaValue(this);
			else
				return new ElaValue(new ElaSet(Tree.Add(value, default(ElaValue))));
		}


		protected override ElaValue GenerateFinalize(ExecutionContext ctx)
		{
			return new ElaValue(this);
		}


		protected override ElaValue Cons(ElaObject instance, ElaValue value, ExecutionContext ctx)
		{
			var next = instance as ElaSet;

			if (next == null)
			{
				ctx.InvalidType(GetTypeName(), new ElaValue(instance));
				return Default();
			}

			return new ElaValue(next.Add(value));
		}


		protected override ElaValue Convert(ElaValue @this, ElaTypeInfo type, ExecutionContext ctx)
		{
			if (type.ReflectedTypeCode == ElaTypeCode.List)
				return new ElaValue(ElaList.FromEnumerable(this));

			ctx.ConversionFailed(@this, type.ReflectedTypeName);
			return Default();
		}


		protected override ElaValue Nil(ExecutionContext ctx)
		{
			return new ElaValue(Empty);
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
