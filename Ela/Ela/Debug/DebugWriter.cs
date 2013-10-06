using System;
using System.Collections.Generic;
using System.Text;

namespace Ela.Debug
{
	internal sealed class DebugWriter
	{
		#region Construction
		private FastStack<ScopeSym> scopes;
		private FastStack<FunSym> funs;
        private int scopeCount;
		
		internal DebugWriter()
		{
			Symbols = new DebugInfo();
			scopes = new FastStack<ScopeSym>();
			funs = new FastStack<FunSym>();
			var glob = new ScopeSym(0, 0, 0, 0, 0) { EndOffset = Int32.MaxValue };
			scopes.Push(glob);
			Symbols.Scopes.Add(glob);
		}
		#endregion


		#region Methods
		internal void StartFunction(string name, int offset, int pars)
		{
			funs.Push(new FunSym(name, offset, pars));
		}


		internal void EndFunction(int handle, int offset)
		{
			var f = funs.Pop();
			f.Handle = handle;
			f.EndOffset = offset;
			Symbols.Functions.Add(f);
		}


		internal void StartScope(int offset, int line, int col)
		{
            var index = ++scopeCount;
			scopes.Push(new ScopeSym(index, scopes.Peek().Index, offset, line, col));
		}


		internal void EndScope(int offset, int line, int col)
		{
			var s = scopes.Pop();
			s.EndOffset = offset;
            s.EndLine = line;
            s.EndColumn = col;
			Symbols.Scopes.Add(s);
		}


		internal void AddVarSym(string name, int address, int offset, int flags, int data)
		{
			Symbols.Vars.Add(LastVarSym = new VarSym(name, address, offset, scopes.Peek().Index, flags, data));
		}


		internal void AddLineSym(int offset, int line, int col)
		{
			Symbols.Lines.Add(new LineSym(offset, line, col));
		}
		#endregion


		#region Properties
		internal DebugInfo Symbols { get; private set; }

        internal VarSym LastVarSym { get; private set; }
		#endregion
	}
}
