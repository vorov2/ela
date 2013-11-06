using System;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using System.Threading;
using Ela.Runtime;
using System.Collections.Generic;

namespace Ela.Library.General
{
	public sealed class AsyncModule : ForeignModule
	{
		#region Construction
        private readonly object syncRoot = new Object();

		public AsyncModule()
		{
			Threads = new List<Thread>();
		}
		#endregion


		#region Methods
		public override void Close()
		{
			foreach (var t in Threads)
			{
				if (t.IsAlive)
				{
					try
					{
						t.Abort();
					}
					catch { }
				}
			}
		}


		public override void Initialize()
		{
			Add<ElaFunction,ElaAsync>("async", RunAsync);
			Add<ElaAsync,ElaValue>("get", GetValue);
			Add<ElaAsync,Boolean>("hasValue", HasValue);
			Add<Int32,ElaAsync,ElaUnit>("wait", Wait);
			Add<ElaFunction,ElaUnit>("sync", Sync);
            Add<Int32,ElaUnit>("sleep", Sleep);
		}


        public ElaUnit Sleep(int ms)
        {
            Thread.Sleep(ms);
            return ElaUnit.Instance;
        }


		public ElaAsync RunAsync(ElaFunction fun)
		{
			var ret = new ElaAsync(this, fun);
			ret.Run();
			return ret;
		}


		public ElaValue GetValue(ElaAsync obj)
		{
			if (HasValue(obj))
				return obj.Return;
			else
			{
				Wait(Int32.MaxValue, obj);

				if (!HasValue(obj))
					throw new ElaRuntimeException("AsyncNoValue", "Unable to obtain a value of an async computation.");

				return obj.Return;
			}
		}


		public bool HasValue(ElaAsync obj)
		{
			lock (obj.SyncRoot)
				return obj.Return.As<ElaObject>() != null;
		}


		public ElaUnit Wait(int timeout, ElaAsync obj)
		{
			var th = obj.Thread;

			if (th != null)
			{
                if (th.Join(timeout))
				{
                    lock (syncRoot)
                    {
                        Threads.Remove(th);
                        obj.Thread = null;
                    }
				}
			}

			return ElaUnit.Instance;
		}


		public ElaUnit Sync(ElaFunction fun)
		{
            lock (syncRoot)
				fun.Call();

			return ElaUnit.Instance;
		}
		#endregion


		#region Properties
		public List<Thread> Threads { get; private set; }
		#endregion
	}
}
