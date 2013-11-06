using System;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaLazy : ElaObject
	{
		internal ElaLazy(ElaFunction function) : base(ElaTypeCode.Lazy)
		{
			Function = function;
			_value = default(ElaValue);
        }

        internal override int GetTypeId()
        {
            return Value.Ref != null ? Value.Ref.TypeId : ElaMachine.LAZ;
        }
        
		internal ElaValue Force()
		{
            if (Value.Ref == null)
            {
                var fun = Function;

                while (fun != null)
                {
                    Value = fun.CallWithThrow(
                        fun.LastParameter.Ref != null ? fun.LastParameter :
                        new ElaValue(ElaUnit.Instance));

                    if (Value.Ref is ElaLazy)
                    {
                        var la = (ElaLazy)Value.Ref;

                        if (la.Value.Ref == null && la.Function != null)
                            fun = la.Function;
                        else
                        {
                            Value = la.Value;
                            fun = null;
                        }
                    }
                    else
                        fun = null;
                }
            }

            return Value;
		}
        
		internal override ElaValue Force(ElaValue @this, ExecutionContext ctx)
		{
			if (ctx == ElaObject.DummyContext)
				return Force();

			return Force(ctx);
		}
        
		internal ElaValue Force(ExecutionContext ctx)
		{
            var f = Function;

			if (f != null)
			{
                ctx.Failed = true;
                ctx.Thunk = this;
                return new ElaValue(this);
			}

            if (Value.Ref == this)
            {
                ctx.Fail(ElaRuntimeError.Cyclic);
                return Default();
            }

            if (Value.TypeId == ElaMachine.LAZ)
            {
                var val = Value;

                while (val.TypeId == ElaMachine.LAZ)
                {
                    var la = (ElaLazy)val.Ref;

                    if (la.Value.Ref != null)
                        val = la.Value;
                    else
                        return Value.Ref.Force(Value, ctx);
                }

                return Value = val;
            }

            return Value;
		}

        public override string ToString(string format, IFormatProvider provider)
        {
            if (Value.Ref != null)
                return Value.ToString(format, provider);
            else
                return "<thunk>";
        }

        internal override bool True(ElaValue @this, ExecutionContext ctx)
        {
            var ret = Force(ctx);

            if (ctx.Failed)
                return false;

            return ret.Ref.True(ret, ctx);
        }

        internal override bool False(ElaValue @this, ExecutionContext ctx)
        {
            var ret = Force(ctx);

            if (ctx.Failed)
                return false;

            return ret.Ref.False(ret, ctx);
        }

        internal override ElaValue Cons(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(new ElaLazyList(this, value));
        }

		protected internal override ElaValue GenerateFinalize(ExecutionContext ctx)
		{
			return new ElaValue(this);
		}

        public bool Evaled
        {
            get { return Function == null; }
        }

		internal ElaFunction Function;

		private ElaValue _value;
		internal ElaValue Value
		{
			get { return _value; }
			set
			{
                _value = value;
				Function = null;				
			}
		}
	}
}