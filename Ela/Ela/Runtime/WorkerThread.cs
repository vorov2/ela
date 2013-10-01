using System;
using Ela.Compilation;
using Ela.Linking;

namespace Ela.Runtime
{
	public class WorkerThread
	{
		private WorkerThread()
		{
			
		}
		
		internal WorkerThread(CodeAssembly asm)
		{
			Assembly = asm;
			Module = asm.GetRootModule();
            CallStack = new CallStack();
			Context = new ExecutionContext();
		}
		
        internal void SwitchModule(int handle)
		{
			ModuleHandle = handle;
			Module = Assembly.GetModule(handle);
		}
        
		internal WorkerThread Clone()
		{
			var ret = new WorkerThread();
			ret.Assembly = Assembly;
			ret.CallStack = new CallStack();
			ret.Module = Assembly.GetRootModule();
			ret.Context = new ExecutionContext();
			ret.Offset = 1;
			return ret;
		}

        internal CodeAssembly Assembly { get; private set; }

		internal CallStack CallStack { get; private set; }

        internal int Offset;

		internal CodeFrame Module { get; private set; }

		internal int ModuleHandle { get; private set; }

		internal ExecutionContext Context { get; private set; }

        internal Exception LastException { get; set; }
	}
}