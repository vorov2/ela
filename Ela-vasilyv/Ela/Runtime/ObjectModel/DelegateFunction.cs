using System;

namespace Ela.Runtime.ObjectModel
{
    public delegate R ElaFun<R>();
    public delegate R ElaFun<T,R>(T arg1);
    public delegate R ElaFun<T1,T2,R>(T1 arg1, T2 arg2);
    public delegate R ElaFun<T1,T2,T3,R>(T1 arg1, T2 arg2, T3 arg3);
    public delegate R ElaFun<T1,T2,T3,T4,R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    public delegate R ElaFun<T1,T2,T3,T4,T5,R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);
    public delegate R ElaFun<T1,T2,T3,T4,T5,T6,R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6);
    public delegate R ElaFun<T1,T2,T3,T4,T5,T6,T7,R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7);
    public delegate R ElaFun<T1,T2,T3,T4,T5,T6,T7,T8,R>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8);

	internal abstract class DelegateFunction : ElaFunction
	{
		protected DelegateFunction(int args, string name) : base(args)
		{
			Name = name;
		}

		public override string GetFunctionName()
		{
			return Name;
		}

		protected string Name { get; private set; }
	}

    internal sealed class DynamicDelegateFunction : DelegateFunction
    {
        private Delegate func;

        internal DynamicDelegateFunction(string name, Delegate func) : base(1, name)
        {
            this.func = func;
        }

        public override ElaValue Call(params ElaValue[] args)
        {
            var data = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
                data[i] = args[i].AsObject();

            return ElaValue.FromObject(func.DynamicInvoke(data));
        }

        public override ElaFunction Clone()
        {
            return CloneFast(new DynamicDelegateFunction(Name, func));
        }
    }

	internal sealed class DelegateFunction<T1> : DelegateFunction
	{
		private ElaFun<T1> func;

		internal DelegateFunction(string name, ElaFun<T1> func) : base(1, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			args[0].As<ElaUnit>();
			return ElaValue.FromObject(func());
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1>(Name, func));
		}
	}

	internal sealed class DelegateFunction<T1,T2> : DelegateFunction
	{
        private ElaFun<T1,T2> func;

		internal DelegateFunction(string name, ElaFun<T1,T2> func) : base(1, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2>(Name, func));
		}
	}

	internal sealed class DelegateFunction<T1,T2,T3> : DelegateFunction
	{
		private ElaFun<T1,T2,T3> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3> func) : base(2, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3>(Name, func));
		}
	}

	internal sealed class DelegateFunction<T1,T2,T3,T4> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4> func) : base(3, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4>(Name, func));
		}
	}

	internal sealed class DelegateFunction<T1,T2,T3,T4,T5> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4,T5> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4,T5> func) : base(4, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>(), args[3].Convert<T4>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4,T5>(Name, func));
		}
	}

	internal sealed class DelegateFunction<T1,T2,T3,T4,T5,T6> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4,T5,T6> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4,T5,T6> func) : base(5, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>(), args[3].Convert<T4>(), args[4].Convert<T5>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4,T5,T6>(Name, func));
		}
	}

    internal sealed class DelegateFunction<T1,T2,T3,T4,T5,T6,T7> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4,T5,T6,T7> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7> func) : base(6, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>(), args[3].Convert<T4>(), args[4].Convert<T5>(),
                args[5].Convert<T6>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7>(Name, func));
		}
	}

    internal sealed class DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4,T5,T6,T7,T8> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7,T8> func) : base(7, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>(), args[3].Convert<T4>(), args[4].Convert<T5>(),
                args[5].Convert<T6>(), args[6].Convert<T7>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8>(Name, func));
		}
	}

    internal sealed class DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8,T9> : DelegateFunction
	{
		private ElaFun<T1,T2,T3,T4,T5,T6,T7,T8,T9> func;

		internal DelegateFunction(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7,T8,T9> func) : base(8, name)
		{
			this.func = func;
		}

		public override ElaValue Call(params ElaValue[] args)
		{
			return ElaValue.FromObject(func(args[0].Convert<T1>(), args[1].Convert<T2>(), args[2].Convert<T3>(), args[3].Convert<T4>(), args[4].Convert<T5>(),
                args[5].Convert<T6>(), args[6].Convert<T7>(), args[7].Convert<T8>()));
		}

		public override ElaFunction Clone()
		{
			return CloneFast(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8,T9>(Name, func));
		}
	}
}
