using System;
using System.Collections;
using System.Collections.Generic;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class ElaQueue : ElaObject, IEnumerable<ElaValue>
    {
        #region Construction
        public static readonly ElaQueue Empty = new ElaQueue(ElaList.Empty, ElaList.Empty);
        private const string TYPENAME = "queue";
        private ElaList forward;
        private ElaList backward;

        internal ElaQueue(ElaList forward, ElaList backward)
        {
            this.forward = forward;
            this.backward = backward;
        }


        public ElaQueue(IEnumerable<ElaValue> seq) : this(ElaList.FromEnumerable(seq).Reverse(), ElaList.Empty)
        {

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


        protected override string Show(ElaValue @this, ShowInfo info, ExecutionContext ctx)
        {
            return "queue" + new ElaValue(ToList()).Show(info, ctx);
        }


        protected override ElaValue Head(ExecutionContext ctx)
        {
            return Peek();
        }


        protected override ElaValue Tail(ExecutionContext ctx)
        {
            return new ElaValue(Dequeue());
        }


        protected override bool IsNil(ExecutionContext ctx)
        {
            return this == Empty;
        }


        protected override ElaValue GetLength(ExecutionContext ctx)
        {
            return new ElaValue(Length);
        }


        protected override ElaValue Convert(ElaValue @this, ElaTypeInfo type, ExecutionContext ctx)
        {
            if (type.ReflectedTypeCode == ElaTypeCode.List)
                return new ElaValue(ToList());

            ctx.ConversionFailed(@this, type.ReflectedTypeName);
            return Default();
        }


		protected override ElaValue Cons(ElaObject instance, ElaValue value, ExecutionContext ctx)
		{
			var q = instance as ElaQueue;

			if (q == null)
			{
				ctx.InvalidType(GetTypeName(), new ElaValue(instance));
				return Default();
			}

			return new ElaValue(q.Enqueue(value));			
		}


		protected override ElaValue Nil(ExecutionContext ctx)
		{
			return new ElaValue(ElaQueue.Empty);
		}


        public ElaList ToList()
        {
            return forward.Concatenate(backward.Reverse());
        }


        public ElaValue Peek() 
        {
            return forward.Value;
        }


        public ElaQueue Enqueue(ElaValue value)
        {
            return new ElaQueue(forward, new ElaList(backward, value));
        }


        public ElaQueue Dequeue()
        {
            var f = forward.Next;

            if (!new ElaValue(f).IsNil(null))
                return new ElaQueue(f, backward);
            else if (new ElaValue(backward).IsNil(null))
                return ElaQueue.Empty;
            else
                return new ElaQueue(backward.Reverse(), ElaList.Empty);
        }


        public IEnumerator<ElaValue> GetEnumerator()
        {
            foreach (var t in forward) 
                yield return t;
            
            foreach (var t in backward.Reverse()) 
                yield return t;
        }
        
        
        IEnumerator IEnumerable.GetEnumerator() 
        { 
            return ((IEnumerable<ElaValue>)this).GetEnumerator(); 
        }
        #endregion


        #region Properties
        public int Length
        {
            get { return forward.Length + backward.Length; }
        }
        #endregion
    }
}
