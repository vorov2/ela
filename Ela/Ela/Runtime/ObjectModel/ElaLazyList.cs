using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
	public class ElaLazyList : ElaList
	{
		private ElaLazy thunk;
		
		public ElaLazyList(ElaLazy next, object value) : this(next, ElaValue.FromObject(value))
		{

		}
        
		public ElaLazyList(ElaLazy next, ElaValue value) : this((ElaLazyList)null, value)
		{
			this.thunk = next;
		}
        
		public ElaLazyList(ElaLazyList next, ElaValue value) : base(next, value)
		{
			
		}
		
        public override ElaValue Tail()
        {
            if (thunk != null)
            {
                InternalNext = thunk.Force().Ref as ElaList;
                thunk = null;
            }

            return new ElaValue(InternalNext);
        }

		protected internal override ElaValue Tail(ExecutionContext ctx)
		{
			if (thunk != null)
			{
                InternalNext = thunk.Force(ctx).Ref as ElaList;

                if (InternalNext == null)
                {
                    ctx.Fail("Invalid lazy list definition.");
                    return Default();
                }

				thunk = null;
			}

			return new ElaValue(InternalNext);
        }

        internal override ElaValue Cons(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(new ElaLazyList(this, value));
        }

		protected internal override ElaValue GenerateFinalize(ExecutionContext ctx)
		{
			return new ElaValue(this);
        }

        public override string ToString(string format, IFormatProvider formatProvider)
        {
            return "[lazy list]";
        }
		
		protected override Exception InvalidDefinition()
		{
            return new ElaException("Invalid lazy list definition.");
		}
		
		public override ElaList Next
		{
			get
			{
				if (thunk != null)
				{
					var xs = thunk.Force().Ref as ElaList;

					if (xs == null)
						throw InvalidDefinition();

					thunk = null;
					InternalNext = xs;
				}

				return InternalNext;
			}
		}
	}
}
