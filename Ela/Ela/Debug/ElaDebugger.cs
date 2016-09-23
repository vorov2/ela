using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
		public CallStack BuildCallStack(int currentOffset, CodeFrame errModule, ModuleFileInfo moduleFile, Stack<StackPoint> callChain)
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
        
        internal IEnumerable<TraceVar> ObtainTraceVars(WorkerThread thread, Ela.Runtime.CallStack stack)
        {
            var sym = thread.Module.Symbols;
            var mem = stack.Peek();
            var locals = mem.Locals;
            var captures = mem.Captures;

            var off = thread.Offset;
            var dr = new DebugReader(sym);
            var scope = dr.FindScopeSym(off);
            var vars = dr.FindVarSyms(off, scope).ToList();

            var alls = new FastList<ElaValue[]>();
            alls.AddRange(captures);
            alls.Add(locals);
            var count = alls.Count - 1;
            var locs = alls[count];
            var varcount = 0;

            do
            {
                for (var i = 0; i < vars.Count; i++)
                {
                    var v = vars[i];

                    if ((Ela.CodeModel.ElaVariableFlags)v.Flags != Ela.CodeModel.ElaVariableFlags.Context)
                    {
                        var val = locs[v.Address];
                        varcount++;
                        yield return new TraceVar(v.Name, val, locs == locals);

                        if (varcount == locs.Length)
                        {
                            locs = alls[--count];
                            varcount = 0;
                        }
                    }
                }

                var ns = dr.GetScopeSymByIndex(scope.ParentIndex);

                if (ns.Index != scope.Index)
                {
                    scope = ns;
                    vars = dr.FindVarSyms(off, scope).ToList();
                }
                else
                    scope = null;
            }
            while (scope != null);
        }
		#endregion


		#region Properties
		public CodeAssembly Assembly { get; private set; }
		#endregion
	}
}
