using System;
using System.Collections.Generic;
using Ela.CodeModel;
using Ela.Compilation;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Linking
{
	public abstract class ForeignModule
	{
		#region Construction
		private FastList<ElaValue> locals;
        private Scope scope;

		protected ForeignModule()
		{
			locals = new FastList<ElaValue>();
			scope = new Scope(false, null);
		}
		#endregion


		#region Methods
		public abstract void Initialize();


		public virtual void Close()
		{

		}


		internal IntrinsicFrame Compile()
		{
			var frame = new IntrinsicFrame(locals.ToArray());
			frame.Layouts.Add(new MemoryLayout(locals.Count, 0, 0));
			frame.GlobalScope = scope;
			return frame;
		}


        internal bool IsRegistered(string name)
        {
            return scope.Locals.ContainsKey(name);
        }


		protected void Add(string name, int val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, long val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, float val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, double val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, char val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, string val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, bool val)
		{
			Add(name, new ElaValue(val));
		}


		protected void Add(string name, ElaObject obj)
		{
			Add(name, new ElaValue(obj));
		}


        protected void Add<T1>(string name, ElaFun<T1> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1>(name, fun)));
		}
		

		protected void Add<T1,T2>(string name, ElaFun<T1,T2> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2>(name, fun)));
		}


		protected void Add<T1,T2,T3>(string name, ElaFun<T1,T2,T3> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3>(name, fun)));
		}


		protected void Add<T1,T2,T3,T4>(string name, ElaFun<T1,T2,T3,T4> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4>(name, fun)));
		}


		protected void Add<T1,T2,T3,T4,T5>(string name, ElaFun<T1,T2,T3,T4,T5> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4,T5>(name, fun)));
		}

        protected void Add<T1,T2,T3,T4,T5,T6>(string name, ElaFun<T1,T2,T3,T4,T5,T6> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4,T5,T6>(name, fun)));
		}

        protected void Add<T1,T2,T3,T4,T5,T6,T7>(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7>(name, fun)));
		}

        protected void Add<T1,T2,T3,T4,T5,T6,T7,T8>(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7,T8> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8>(name, fun)));
		}

        protected void Add<T1,T2,T3,T4,T5,T6,T7,T8,T9>(string name, ElaFun<T1,T2,T3,T4,T5,T6,T7,T8,T9> fun)
		{
			Add(name, new ElaValue(new DelegateFunction<T1,T2,T3,T4,T5,T6,T7,T8,T9>(name, fun)));
		}

		protected void Add(string name, ElaValue value)
		{
			scope.Locals.Remove(name);
			scope.Locals.Add(name, new ScopeVar(ElaVariableFlags.None, locals.Count, -1));
			locals.Add(value);
        }
		#endregion
    }
}
