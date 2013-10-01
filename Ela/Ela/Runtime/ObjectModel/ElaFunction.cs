using System;
using System.Collections.Generic;
using System.Text;
using Ela.CodeModel;
using Ela.Debug;

namespace Ela.Runtime.ObjectModel
{
	public class ElaFunction : ElaObject
	{
		private ElaMachine vm;
		internal static readonly ElaValue[] defaultParams = new ElaValue[] { new ElaValue(ElaUnit.Instance) };
		internal static readonly ElaValue[] emptyParams = new ElaValue[0];
		private const string DEF_NAME = "<f>";
        internal bool table;
         
		protected ElaFunction() : this(1)
		{

		}		
		
		protected ElaFunction(int parCount) : base(ElaTypeCode.Function)
		{
			if (parCount == 0)
				parCount = 1;

			if (parCount > 1)
				Parameters = new ElaValue[parCount - 1];
			else
				Parameters = emptyParams;
		}
		
		internal ElaFunction(int handle, int module, int parCount, FastList<ElaValue[]> captures, ElaMachine vm) : base(ElaTypeCode.Function)
		{
			Handle = handle;
			ModuleHandle = module;
			Captures = captures;
			this.vm = vm;
			Parameters = parCount > 1 ? new ElaValue[parCount - 1] : emptyParams;
		}
        
		private ElaFunction(ElaValue[] pars) : base(ElaTypeCode.Function)
		{
			Parameters = pars;
        }

        #region Conversion
        public static ElaFunction Create<T1>(ElaFun<T1> fun)
        {
            return new DelegateFunction<T1>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2>(ElaFun<T1, T2> fun)
        {
            return new DelegateFunction<T1, T2>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3>(ElaFun<T1, T2, T3> fun)
        {
            return new DelegateFunction<T1, T2, T3>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4>(ElaFun<T1, T2, T3, T4> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5>(ElaFun<T1, T2, T3, T4, T5> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6>(ElaFun<T1, T2, T3, T4, T5, T6> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7>(ElaFun<T1, T2, T3, T4, T5, T6, T7> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7, T8>(ElaFun<T1, T2, T3, T4, T5, T6, T7, T8> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(ElaFun<T1, T2, T3, T4, T5, T6, T7, T8, T9> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(DEF_NAME, fun);
        }

        public static ElaFunction Create<T1>(string name, ElaFun<T1> fun)
        {
            return new DelegateFunction<T1>(name, fun);
        }

        public static ElaFunction Create<T1, T2>(string name, ElaFun<T1, T2> fun)
        {
            return new DelegateFunction<T1, T2>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3>(string name, ElaFun<T1, T2, T3> fun)
        {
            return new DelegateFunction<T1, T2, T3>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4>(string name, ElaFun<T1, T2, T3, T4> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5>(string name, ElaFun<T1, T2, T3, T4, T5> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6>(string name, ElaFun<T1, T2, T3, T4, T5, T6> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7>(string name, ElaFun<T1, T2, T3, T4, T5, T6, T7> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7, T8>(string name, ElaFun<T1, T2, T3, T4, T5, T6, T7, T8> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7, T8>(name, fun);
        }

        public static ElaFunction Create<T1, T2, T3, T4, T5, T6, T7, T8, T9>(string name, ElaFun<T1, T2, T3, T4, T5, T6, T7, T8, T9> fun)
        {
            return new DelegateFunction<T1, T2, T3, T4, T5, T6, T7, T8, T9>(name, fun);
        }

        public Delegate ToDelegate()
        {
            var len = Parameters.Length;

            if (len == 0)
                return ToDelegate<Object>();
            else if (len == 1)
                return ToDelegate<Object, Object>();
            else if (len == 2)
                return ToDelegate<Object, Object, Object>();
            else if (len == 3)
                return ToDelegate<Object, Object, Object, Object>();
            else if (len == 4)
                return ToDelegate<Object, Object, Object, Object, Object>();
            else if (len == 5)
                return ToDelegate<Object, Object, Object, Object, Object, Object>();
            else if (len == 6)
                return ToDelegate<Object, Object, Object, Object, Object, Object, Object>();
            else if (len == 7)
                return ToDelegate<Object, Object, Object, Object, Object, Object, Object, Object>();
            else if (len == 8)
                return ToDelegate<Object, Object, Object, Object, Object, Object, Object, Object, Object>();
            else
                throw new InvalidCastException();
        }

        public ElaFun<R> ToDelegate<R>()
        {
            return () =>
            {
                var ret = Call(new ElaValue(ElaUnit.Instance));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T, R> ToDelegate<T, R>()
        {
            return t =>
            {
                var ret = Call(ElaValue.FromObject(t));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, R> ToDelegate<T1, T2, R>()
        {
            return (t1, t2) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, R> ToDelegate<T1, T2, T3, R>()
        {
            return (t1, t2, t3) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, T4, R> ToDelegate<T1, T2, T3, T4, R>()
        {
            return (t1, t2, t3, t4) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3), ElaValue.FromObject(t4));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, T4, T5, R> ToDelegate<T1, T2, T3, T4, T5, R>()
        {
            return (t1, t2, t3, t4, t5) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3), ElaValue.FromObject(t4),
                    ElaValue.FromObject(t5));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, T4, T5, T6, R> ToDelegate<T1, T2, T3, T4, T5, T6, R>()
        {
            return (t1, t2, t3, t4, t5, t6) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3), ElaValue.FromObject(t4),
                    ElaValue.FromObject(t5), ElaValue.FromObject(t6));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, T4, T5, T6, T7, R> ToDelegate<T1, T2, T3, T4, T5, T6, T7, R>()
        {
            return (t1, t2, t3, t4, t5, t6, t7) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3), ElaValue.FromObject(t4),
                    ElaValue.FromObject(t5), ElaValue.FromObject(t6), ElaValue.FromObject(t7));
                return ret.Convert<R>();
            };
        }

        public ElaFun<T1, T2, T3, T4, T5, T6, T7, T8, R> ToDelegate<T1, T2, T3, T4, T5, T6, T7, T8, R>()
        {
            return (t1, t2, t3, t4, t5, t6, t7, t8) =>
            {
                var ret = Call(ElaValue.FromObject(t1), ElaValue.FromObject(t2), ElaValue.FromObject(t3), ElaValue.FromObject(t4),
                    ElaValue.FromObject(t5), ElaValue.FromObject(t6), ElaValue.FromObject(t7), ElaValue.FromObject(t8));
                return ret.Convert<R>();
            };
        }
        #endregion

        public IEnumerable<ElaValue> GetAppliedArguments()
        {
            for (var i = 0; i < AppliedParameters; i++)
                yield return Parameters[i];
        }

        public int GetArgumentNumber()
        {
            return Parameters.Length + 1;
        }

        public int GetAppliedArgumentNumber()
        {
            return AppliedParameters;
        }

		internal ElaValue CallWithThrow(ElaValue value)
		{
			return vm.CallPartial(this, value);
		}
        
        public override string ToString(string format, IFormatProvider provider)
        {
			var sb = new StringBuilder();
			sb.Append("*");

			for (var i = 0; i < Parameters.Length + 1 - AppliedParameters; i++)
			{
				sb.Append("->");
				sb.Append("*");
			}

			return GetFunctionName() + ":" + sb.ToString();
        }

        internal override bool CanTailCall()
        {
            return Captures != null;
        }

        internal ElaFunction CloneFast()
		{
            var pars = new ElaValue[Parameters.Length];

			if (AppliedParameters > 0) //This is faster than Array.Copy
				for (var i = 0; i < AppliedParameters; i++)
					pars[i] = Parameters[i];

			var ret = new ElaFunction(pars);
			ret.AppliedParameters = AppliedParameters;
			ret.Handle = Handle;
			ret.ModuleHandle = ModuleHandle;
			ret.vm = vm;
			ret.Captures = Captures;
			ret._flip = _flip;
			return ret;
		}

		protected ElaFunction CloneFast(ElaFunction newInstance)
		{
			var pars = new ElaValue[Parameters.Length];

			if (AppliedParameters > 0) //This is faster than Array.Copy
				for (var i = 0; i < AppliedParameters; i++)
					pars[i] = Parameters[i];

			newInstance.Parameters = pars;
			newInstance.AppliedParameters = AppliedParameters;
			newInstance.Handle = Handle;
			newInstance.ModuleHandle = ModuleHandle;
			newInstance.vm = vm;
			newInstance.Captures = Captures;
			newInstance._flip = _flip;
			return newInstance;
		}
        
		public virtual ElaFunction Clone()
		{
			var nf = (ElaFunction)MemberwiseClone();
			var pars = new ElaValue[Parameters.Length];

			if (AppliedParameters > 0) //This is faster than Array.Copy
				for (var i = 0; i < AppliedParameters; i++)
					pars[i] = Parameters[i];

			nf.Parameters = pars;
			return nf;
		}

		public virtual ElaValue Call(params ElaValue[] args)
		{
			if (args == null || args.Length == 0)
				args = defaultParams;

			return vm.Call(this, args);
		}
        
		public virtual string GetFunctionName()
		{
			var funName = DEF_NAME;

			if (vm != null)
			{
				var mod = vm.Assembly.GetModule(ModuleHandle);
				var syms = mod.Symbols;

				if (syms != null)
				{
					var dr = new DebugReader(syms);
					var fs = dr.GetFunSymByHandle(Handle);

					if (fs != null && fs.Name != null)
						funName = fs.Name;
				}
				else
				{
					foreach (var sv in mod.GlobalScope.Locals)
					{
						var v = vm.GetVariableByHandle(ModuleHandle, sv.Value.Address);

						if (v.TypeId == ElaMachine.FUN && ((ElaFunction)v.Ref).Handle == Handle)
						{
							funName = sv.Key;
							break;
						}
					}
				}
			}

			if (funName != DEF_NAME && Format.IsSymbolic(funName))
				funName = "(" + funName + ")";

			return funName;
		}

        public ElaModule GetFunctionModule()
        {
            return new ElaModule(ModuleHandle, vm);
        }

		internal ElaMachine Machine 
        { 
            get { return vm; }
            set { vm = value; }
        }

		public int Handle { get; private set; }

		public int ModuleHandle { get; private set; }

		internal FastList<ElaValue[]> Captures { get; private set; }
		
		internal int AppliedParameters { get; set; }

		internal ElaValue[] Parameters { get; set; }

		internal ElaValue LastParameter { get; set; }

		private bool _flip;
		internal bool Flip 
		{
			get { return _flip; }
			set
			{
				if (Parameters.Length > 0)
					_flip = value;
			}
		}
	}
}
