using System;
using System.Collections.Generic;
using Ela.CodeModel;
using Ela.Compilation;

namespace Ela.Runtime.ObjectModel
{
	public sealed class ElaModule : ElaObject
	{
		private const string ADDRESS = "address";
        private const string VARNAME = "name";
        private const string ISPRIVATE = "isPrivate";
        private const string VALUE = "value";
        private const string MODULENAME = "moduleName";
        private const string DLLNAME = "dllName";
        private const string ALIAS = "alias";
        private const string PATH = "path";
        private const string MODULE = "[module:{0}:{1}]";
		private ElaMachine vm;

		public ElaModule(int handle, ElaMachine vm) : base(ElaTypeCode.Module)
		{
			Handle = handle;
			this.vm = vm;
		}

        public override string ToString(string format, IFormatProvider provider)
        {
            return String.Format(MODULE, vm != null ? vm.Assembly.GetModuleName(Handle) : String.Empty, Handle);
		}
        
        internal int GetVariableCount(ExecutionContext ctx)
        {
            if (vm != null)
            {
                var frame = vm.Assembly.GetModule(Handle);
                var c = 0;

                foreach (var sv in frame.GlobalScope.Locals)
                    if ((sv.Value.Flags & ElaVariableFlags.Private) != ElaVariableFlags.Private)
                        c++;

                return c;
            }

            ctx.Fail(ElaRuntimeError.Unknown, "VM is non present");
            return 0;
        }

        internal bool HasField(ElaValue index, ExecutionContext ctx)
        {
            if (vm != null)
            {
                if (index.TypeId != ElaMachine.STR)
                {
                    ctx.InvalidType(TCF.GetShortForm(ElaTypeCode.String), index);
                    return false;
                }

                var frame = vm.Assembly.GetModule(Handle);
                return frame.GlobalScope.Locals.ContainsKey(index.DirectGetString());
            }

            ctx.Fail(ElaRuntimeError.Unknown, "VM is non present");
            return false;
        }

        internal ElaValue GetField(ElaValue index, ExecutionContext ctx)
		{
			if (vm != null)
			{
                if (index.TypeId != ElaMachine.STR)
                {
                    ctx.InvalidType(TCF.GetShortForm(ElaTypeCode.String), index);
                    return Default();
                }

                var field = index.DirectGetString();
				var frame = vm.Assembly.GetModule(Handle);
				ScopeVar sc;

				if (!frame.GlobalScope.Locals.TryGetValue(field, out sc))
				{
					ctx.UnknownField(index.DirectGetString(), new ElaValue(this));
					return Default();
				}

				if ((sc.Flags & ElaVariableFlags.Private) == ElaVariableFlags.Private)
				{
					ctx.Fail(new ElaError(ElaRuntimeError.PrivateVariable, field));
					return Default();
				}

				return vm.modules[Handle][sc.Address];
			}

            ctx.Fail(ElaRuntimeError.Unknown, "VM is non present");
			return Default();
		}

        public IEnumerable<ElaRecord> GetVariables()
        {
            if (vm == null)
                throw new ElaException("VM is non present");

            var frame = vm.Assembly.GetModule(Handle);            

            foreach (var v in frame.GlobalScope.EnumerateNames())
            {
				var sv = frame.GlobalScope.GetVariable(v);

				if ((sv.Flags & ElaVariableFlags.Private) != ElaVariableFlags.Private)
				{
                    var val = vm.GetVariableByHandle(Handle, sv.Address);

					if (val.Ref != null)
						yield return new ElaRecord(
							new ElaRecordField(ADDRESS, sv.Address),
							new ElaRecordField(VARNAME, v),
							new ElaRecordField(VALUE, val),
							new ElaRecordField(ISPRIVATE, (sv.Flags & ElaVariableFlags.Private) == ElaVariableFlags.Private));
				}
            }
        }

        public IEnumerable<ElaRecord> GetReferences()
        {
            if (vm == null)
                throw new ElaException("VM is non present");

            var frame = vm.Assembly.GetModule(Handle);
            
            foreach (var kv in frame.References)
                yield return new ElaRecord(
                    new ElaRecordField(MODULENAME, kv.Value.ModuleName),
                    new ElaRecordField(DLLNAME, kv.Value.DllName),
                    new ElaRecordField(ALIAS, kv.Key),
                    new ElaRecordField(PATH, String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), kv.Value.Path)));
        }
        
        public ElaMachine GetCurrentMachine()
        {
            return vm;
        }

        public string GetModuleName()
        {
            if (vm == null)
                throw new ElaException("VM is non present");

            return vm.Assembly.GetModuleName(Handle);
        }

		public int Handle { get; private set; }
	}
}
