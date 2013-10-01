using System;
using System.IO;
using Ela.CodeModel;
using Ela.Compilation;
using System.Collections.Generic;
using Ela.Debug;

namespace Ela.Linking
{
	public sealed class ObjectFileReader : ObjectFile
	{
		public ObjectFileReader(FileInfo file) : base(file)
		{

		}

		public CodeFrame Read()
		{
			using (var bw = new BinaryReader(File.OpenRead()))
				return Read(bw);
		}
        
		private CodeFrame Read(BinaryReader bw)
		{
			var frame = new CodeFrame();
			frame.GlobalScope = new Scope(false, null);
			var v = bw.ReadInt32();

			if (v != Version)
				throw new ElaLinkerException(Strings.GetMessage("InvalidObjectFile", Version), null);

            bw.ReadInt32(); //Version: Major
            bw.ReadInt32(); //Version: Minor
            bw.ReadInt32(); //Version: Build
            bw.ReadInt32(); //Version: Revision
            bw.ReadInt64(); //Version: Date stamp

            var c = bw.ReadInt32();

			for (var i = 0; i < c; i++)
			{
				var alias = bw.ReadString();
				var modName = bw.ReadString();
				var dllName = bw.ReadString();
                dllName = dllName.Length == 0 ? null : dllName;
                var qual = bw.ReadBoolean();
                var pl = bw.ReadInt32();
                var list = new string[pl];

				for (var j = 0; j < pl; j++)
					list[j] = bw.ReadString();

                var lh = bw.ReadInt32();
				frame.AddReference(alias, new ModuleReference(frame, modName, dllName, list, 0, 0, qual, lh));
			}

			c = bw.ReadInt32();

			for (var i = 0; i < c; i++)
				frame.GlobalScope.Locals.Add(bw.ReadString(),
					new ScopeVar((ElaVariableFlags)bw.ReadInt32(), bw.ReadInt32(), bw.ReadInt32()));

            c = bw.ReadInt32();

            for (var i = 0; i < c; i++)
                frame.LateBounds.Add(new LateBoundSymbol(
                    bw.ReadString(), bw.ReadInt32(), bw.ReadInt32(), bw.ReadInt32(), bw.ReadInt32(),
                    bw.ReadInt32()));

			c = bw.ReadInt32();

			for (var i = 0; i < c; i++)
			{
				var l = new MemoryLayout(bw.ReadInt32(), bw.ReadInt32(), bw.ReadInt32());
				frame.Layouts.Add(l);
			}

			c = bw.ReadInt32();

			for (var i = 0; i < c; i++)
				frame.Strings.Add(bw.ReadString());

			c = bw.ReadInt32();

			for (var i = 0; i < c; i++)
			{
				var opCode = (Op)bw.ReadByte();
				frame.Ops.Add(opCode);
				frame.OpData.Add(OpSizeHelper.OpSize[(Int32)opCode] > 1 ? bw.ReadInt32() : 0);
            }

            c = bw.ReadInt32();

            for (var i = 0; i < c; i++)
                frame.InternalTypes.Add(bw.ReadString(), bw.ReadInt32());

            c = bw.ReadInt32();

            for (var i = 0; i < c; i++)
            {
                var k = bw.ReadString();
                var cc = bw.ReadInt32();
                var mbr = new ElaClassMember[cc];

                for (var j = 0; j < cc; j++)
                {
                    var m = new ElaClassMember();
                    m.Components = bw.ReadInt32();
                    m.Mask = bw.ReadInt32();
                    m.Name = bw.ReadString();
                    mbr[j] = m;
                }

                frame.InternalClasses.Add(k, new ClassData(mbr));
            }

            c = bw.ReadInt32();

            for (var i = 0; i < c; i++)
                frame.InternalInstances.Add(new InstanceData(
                    bw.ReadString(), 
                    bw.ReadString(), 
                    bw.ReadInt32(), 
                    bw.ReadInt32(), 
                    bw.ReadInt32(), 
                    bw.ReadInt32()));

            c = bw.ReadInt32();

            for (var i = 0; i < c; i++)
            {
                var ct = new ConstructorData
                {
                    Code = -1,
                    Name = bw.ReadString(),
                    TypeName = bw.ReadString(),
                    TypeModuleId = bw.ReadInt32()
                };
                var cc = bw.ReadInt32();

                if (cc > 0)
                {
                    ct.Parameters = new List<String>();

                    for (var j = 0; j < cc; j++)
                        ct.Parameters.Add(bw.ReadString());
                }

                frame.InternalConstructors.Add(ct);
            }

            var di = bw.ReadBoolean();

            if (di)
            {
                var sym = new DebugInfo();

                c = bw.ReadInt32();

                for (var i = 0; i < c; i++)
                {
                    var ln = new LineSym(bw.ReadInt32(), bw.ReadInt32(), bw.ReadInt32());
                    sym.Lines.Add(ln);
                }

                c = bw.ReadInt32();

                for (var i = 0; i < c; i++)
                {
                    var fn = new FunSym(bw.ReadString(), bw.ReadInt32(), bw.ReadInt32());
                    fn.Handle = bw.ReadInt32();
                    fn.EndOffset = bw.ReadInt32();
                    sym.Functions.Add(fn);
                }

                frame.Symbols = sym;
            }

			return frame;
		}
	}
}