using System;
using ST = System.Threading;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
	public sealed class ElaAsync : ElaObject
	{
		#region Construction
		private const string TYPENAME = "async";
		internal readonly object SyncRoot = new Object();

		internal ElaAsync(AsyncModule mod, ElaFunction fun)
		{
			Initialize(mod, fun);
		}
		#endregion


		#region Methods
		protected override string GetTypeName()
		{
			return TYPENAME;
		}


		private void Initialize(AsyncModule mod, ElaFunction fun)
		{
			Thread = new ST.Thread(() => Return = fun.Call());
			mod.Threads.Add(Thread);
		}


		internal void Run()
		{
			Thread.Start();
		}
		#endregion



		#region Properties
		internal ST.Thread Thread { get; set; }

		internal ElaValue Return { get; private set; }
		#endregion
	}
}
