using System;
using System.Collections.Generic;
using System.IO;
using Ela.Linking;
using Ela.Runtime;
using Ela.Compilation;

namespace Ela.Debug
{
	public sealed class ElaDebugger
	{
		#region Construction
		private const string FUNC = "<function>";
		private const string FUNC_PARS = "<function@{0}>";
		
		internal ElaDebugger(CodeAssembly asm)
		{
			Assembly = asm;
		}
		#endregion
		

		#region Methods
		public CallStack BuildCallStack(int currentOffset, CodeFrame errModule, FileInfo moduleFile, Stack<StackPoint> callChain)
		{
			var syms = new DebugReader(errModule.Symbols);
			var frames = new List<CallFrame>();
			var lp = syms.FindLineSym(currentOffset - 1);			
			var retval = new CallStack(
					moduleFile,
					lp != null ? lp.Line : 0,
					lp != null ? lp.Column : 0,
					frames
				);

			if (callChain == null || callChain.Count == 0)
				return retval;

			var mem = default(StackPoint);
			var first = true;
			var offset = 0;

			do
			{
				mem = callChain.Pop();
				var mod = Assembly.GetModuleName(mem.ModuleHandle);
				syms = new DebugReader(Assembly.GetModule(mem.ModuleHandle).Symbols);
				offset = first ? currentOffset - 1 : mem.BreakAddress - 1;
				var glob = callChain.Count == 0 || offset < 0;
				var funSym = !glob ? syms.FindFunSym(offset) : null;
				var line = syms != null && offset > 0 ? syms.FindLineSym(offset) : null;
				frames.Add(new CallFrame(glob, mod,
					funSym != null ? 
						funSym.Name != null ? funSym.Name : String.Format(FUNC_PARS, funSym.Parameters) :
						glob ? null : FUNC,
					offset, line));
				first = false;
			}
			while (callChain.Count > 0 && offset > 0);

			return retval;
		}
		#endregion


		#region Properties
		public CodeAssembly Assembly { get; private set; }
		#endregion
	}
}
