using System;
using System.IO;
using Ela.Compilation;

namespace Ela.Linking
{
	public sealed class ObjectFileWriter : ObjectFile
	{
		public ObjectFileWriter(FileInfo file) : base(file)
		{

		}
		
        public void Write(CodeFrame frame)
		{
			using (var bw = new BinaryWriter(File.OpenWrite()))
				Write(frame, bw);
		}
        
		private void Write(CodeFrame frame, BinaryWriter bw)
		{
			bw.Write(Version);
            
            var v = new Version(Const.Version);
            bw.Write(v.Major);
            bw.Write(v.Minor);
            bw.Write(v.Build);
            bw.Write(v.Revision);

            var d = DateTime.Now.ToUniversalTime().Ticks;
            bw.Write(d);

            bw.Write(frame.References.Count);

			foreach (var kv in frame.References)
			{
				bw.Write(kv.Key);
				bw.Write(kv.Value.ModuleName);
				bw.Write(kv.Value.DllName ?? String.Empty);
                bw.Write(kv.Value.RequireQuailified);
                bw.Write(kv.Value.Path.Length);
                
				foreach (var p in kv.Value.Path)
					bw.Write(p);

                bw.Write(kv.Value.LogicalHandle);
			}

			bw.Write(frame.GlobalScope.Locals.Count);

			foreach (var kv in frame.GlobalScope.Locals)
			{
				bw.Write(kv.Key);
				bw.Write((Int32)kv.Value.Flags);
				bw.Write(kv.Value.Address);
				bw.Write(kv.Value.Data);
			}

            bw.Write(frame.LateBounds.Count);

            foreach (var u in frame.LateBounds)
            {
                bw.Write(u.Name);
                bw.Write(u.Address);
                bw.Write(u.Data);
                bw.Write(u.Flags);
                bw.Write(u.Line);
                bw.Write(u.Column);
            }

			bw.Write(frame.Layouts.Count);

			for (var i = 0; i < frame.Layouts.Count; i++)
			{
				var l = frame.Layouts[i];
				bw.Write(l.Size);
				bw.Write(l.StackSize);
				bw.Write(l.Address);
			}

			bw.Write(frame.Strings.Count);

			for (var i = 0; i < frame.Strings.Count; i++)
				bw.Write(frame.Strings[i]);

			var ops = frame.Ops;
			bw.Write(ops.Count);

			for (var i = 0; i < ops.Count; i++)
			{
				var op = ops[i];
				bw.Write((Byte)op);

				if (OpSizeHelper.OpSize[(Int32)op] > 1)
					bw.Write(frame.OpData[i]);
			}

            var types = frame.InternalTypes;
            bw.Write(types.Count);

            foreach (var kv in types)
            {
                bw.Write(kv.Key);

                if (kv.Value <= SysConst.MAX_TYP)
                    bw.Write(kv.Value);
                else
                    bw.Write(-1);
            }

            var classes = frame.InternalClasses;
            bw.Write(classes.Count);

            foreach (var kv in classes)
            {
                bw.Write(kv.Key);
                bw.Write(kv.Value.Members.Length);

                for (var i = 0; i < kv.Value.Members.Length; i++)
                {
                    var m = kv.Value.Members[i];
                    bw.Write(m.Components);
                    bw.Write(m.Mask);
                    bw.Write(m.Name);
                }
            }

            var inst = frame.InternalInstances;
            bw.Write(inst.Count);

            for (var i = 0; i < inst.Count; i++)
            {
                var it = inst[i];
                bw.Write(it.Type);
                bw.Write(it.Class);
                bw.Write(it.TypeModuleId);
                bw.Write(it.ClassModuleId);
                bw.Write(it.Line);
                bw.Write(it.Column);
            }

            var ctors = frame.InternalConstructors;
            bw.Write(ctors.Count);

            foreach (var ct in frame.InternalConstructors)
            {
                bw.Write(ct.Name);
                bw.Write(ct.TypeName);
                bw.Write(ct.TypeModuleId);

                if (ct.Parameters != null)
                {
                    bw.Write(ct.Parameters.Count);

                    for (var j = 0; j < ct.Parameters.Count; j++)
                        bw.Write(ct.Parameters[j]);
                }
                else
                    bw.Write(0);
            }

            var di = frame.Symbols != null;
            bw.Write(di); //Contains debug info

            if (di)
            {
                var sym = frame.Symbols;

                bw.Write(sym.Lines.Count);

                for (var i = 0; i < sym.Lines.Count; i++)
                {
                    var ln = sym.Lines[i];
                    bw.Write(ln.Offset);
                    bw.Write(ln.Line);
                    bw.Write(ln.Column);
                }

                bw.Write(sym.Functions.Count);

                for (var i = 0; i < sym.Functions.Count; i++)
                {
                    var fn = sym.Functions[i];
                    bw.Write(fn.Name ?? String.Empty);
                    bw.Write(fn.StartOffset);
                    bw.Write(fn.Parameters);
                    bw.Write(fn.Handle);
                    bw.Write(fn.EndOffset);
                }
            }            
		}
	}
}
