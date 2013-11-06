using System;
using System.Collections.Generic;
using Ela.Linking;

namespace Ela.Runtime.ObjectModel
{
	public abstract class ElaObject : IFormattable
	{
        internal static readonly ExecutionContext DummyContext = new ExecutionContext();
        internal const string INVALID = "<INVALID>";

        internal sealed class ElaInvalidObject : ElaObject
        {
            internal static readonly ElaInvalidObject Instance = new ElaInvalidObject();

            internal ElaInvalidObject()
            {

            }

            public override string ToString(string format, IFormatProvider formatProvider)
            {
                return INVALID;
            }
        }

		protected ElaObject() : this(ElaTypeCode.Object)
		{
			
		}
		
		internal ElaObject(ElaTypeCode type)
		{
			TypeId = (Int32)type;
		}

        internal virtual int GetTypeId()
        {
            return TypeId;
        }
        
        internal virtual long AsLong()
        {
            return default(Int64);
        }

        internal virtual double AsDouble()
        {
            return default(Double);
        }

		public override string ToString()
		{
            return ToString(String.Empty, Culture.NumberFormat);
		}

        public virtual string ToString(string format, IFormatProvider formatProvider)
        {
            return "[" + GetTypeName() + ":" + TypeId + "]";
        }

        internal virtual bool CanTailCall()
        {
            return false;
        }
        
		protected ElaValue Default()
		{
			return new ElaValue(ElaInvalidObject.Instance);
		}
        
		protected internal virtual string GetTypeName()
		{
			return TCF.GetShortForm((ElaTypeCode)TypeId);
		}

        internal virtual bool True(ElaValue @this, ExecutionContext ctx)
        {
            ctx.NoOperator(@this, "true");
            return false;
        }

        internal virtual bool False(ElaValue @this, ExecutionContext ctx)
        {
            ctx.NoOperator(@this, "false");
            return false;
        }

        internal virtual ElaValue Quot(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            ctx.NoOperator(left, "quot");
            return Default();
        }
    	
		protected internal virtual ElaValue Tail(ExecutionContext ctx)
		{
			ctx.NoOperator(new ElaValue(this), "tail");
			return Default();
		}

        internal virtual ElaValue Cons(ElaValue val, ExecutionContext ctx)
        {
            ctx.NoOperator(new ElaValue(this), "cons");
            return Default();
        }
        
		protected internal virtual ElaValue GenerateFinalize(ExecutionContext ctx)
		{
			ctx.NoOperator(new ElaValue(this), "genfin");
			return Default();
		}
        
        internal virtual ElaValue Force(ElaValue @this, ExecutionContext ctx)
		{
			return @this;
		}
        
		internal virtual int GetTag()
		{
            return -1;
		}
        
		internal virtual ElaValue Untag(CodeAssembly asm, ExecutionContext ctx, int index)
        {
            ctx.Fail(new ElaError(ElaRuntimeError.InvalidTypeArgument, "<>", GetTypeName(), index + 1));
            return Default();
        }

        internal int TypeId { get; private set; }
    }
}
