using System;
using System.Collections.Generic;
using Ela.CodeModel;

namespace Ela.Compilation
{
	public sealed class Scope
	{
		#region Construction
		internal Scope(bool fun, Scope parent)
		{
			Function = fun;
			Parent = parent;
			Locals = new Dictionary<String,ScopeVar>();
		}
		#endregion


		#region Methods
		public ScopeVar GetVariable(string name)
		{
			var var = default(ScopeVar);

			if (!Locals.TryGetValue(name, out var))
				var = ScopeVar.Empty;

			return var;
		}


		public Scope Clone()
		{
			var ret = new Scope(Function, Parent);
			ret.Locals = new Dictionary<String,ScopeVar>(Locals);
			return ret;
		}


		public IEnumerable<String> EnumerateNames()
		{
			foreach (var kv in Locals)
				if ((kv.Value.Flags & ElaVariableFlags.SpecialName) != ElaVariableFlags.SpecialName &&
					((kv.Value.Flags & ElaVariableFlags.External) != ElaVariableFlags.External))
					yield return kv.Key;
		}

        
        public IEnumerable<KeyValuePair<String,ScopeVar>> EnumerateVars()
        {
            foreach (var kv in Locals)
                if ((kv.Value.Flags & ElaVariableFlags.SpecialName) != ElaVariableFlags.SpecialName &&
                    ((kv.Value.Flags & ElaVariableFlags.External) != ElaVariableFlags.External))
                    yield return kv;
        }

        internal bool LocalOrParent(string var)
        {
            if (Function)
                return Locals.ContainsKey(var);

            var s = this;

            do
            {
                if (s.Locals.ContainsKey(var))
                    return true;

                s = s.Parent;
            }
            while (s != null && !s.Function);

            return false;
        }

        internal void AddFlags(string name, ElaVariableFlags flags)
        {
            var sv = Locals[name];
            sv = new ScopeVar(sv.Flags | flags, sv.Address, sv.Data);
            Locals[name] = sv;
        }

        internal void AddFlagsAndData(string name, ElaVariableFlags flags, int data)
        {
            var sv = Locals[name];
            sv = new ScopeVar(sv.Flags | flags, sv.Address, data);
            Locals[name] = sv;
        }

        internal void RemoveFlags(string name, ElaVariableFlags flags)
        {
            var sv = Locals[name];
            sv = new ScopeVar(sv.Flags ^ flags, sv.Address, sv.Data);
            Locals[name] = sv;
        }

        internal int TryChangeVariable(string name)
        {
            var v = default(ScopeVar);

            if (Locals.TryGetValue(name, out v))
            {
                v = new ScopeVar(ElaVariableFlags.None, v.Address, -1);
                Locals[name] = v;
                return v.Address;
            }

            return -1;
        }
		#endregion


		#region Properties
		internal Scope Parent { get; set; }

		internal Dictionary<String,ScopeVar> Locals { get; private set; }

		internal bool Function { get; private set; }
		#endregion
	}
}
