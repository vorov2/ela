using System;
using System.Collections.Generic;
using Ela.Compilation;
using Ela.Runtime;
using Ela.Runtime.Classes;
using System.IO;

namespace Ela.Linking
{
	public sealed class CodeAssembly 
	{
		#region Construction
		private const string MAIN_NAME = "[Main]";
		private const int MAIN_HDL = 0;
		
		private Dictionary<String,Int32> moduleMap;
		private FastList<CodeFrame> modules;
        private FastList<Boolean> quals;
        private FastList<ForeignModule> foreignModules;
		
		public static readonly CodeAssembly Empty = new CodeAssembly(CodeFrame.Empty);

		public CodeAssembly(CodeFrame frame) : this()
		{
			moduleMap.Add(MAIN_NAME, modules.Count);
			modules.Add(frame);			
		}


		internal CodeAssembly()
		{
			modules = new FastList<CodeFrame>();
			foreignModules = new FastList<ForeignModule>();
			moduleMap = new Dictionary<String,Int32>();
            quals = new FastList<Boolean>();
            Instances = new Dictionary<Int64,Byte>();
            Constructors = new FastList<ConstructorData>();
            Types = new FastList<TypeData>
                {
                    new TypeData(ElaTypeCode.None),
                    new TypeData(ElaTypeCode.Integer),
                    new TypeData(ElaTypeCode.Long),
                    new TypeData(ElaTypeCode.Single),
                    new TypeData(ElaTypeCode.Double),
                    new TypeData(ElaTypeCode.Boolean),
                    new TypeData(ElaTypeCode.Char),
                    new TypeData(ElaTypeCode.String),
                    new TypeData(ElaTypeCode.Unit),
                    new TypeData(ElaTypeCode.List),
                    new TypeData(ElaTypeCode.__Reserved),
                    new TypeData(ElaTypeCode.Tuple),
                    new TypeData(ElaTypeCode.Record),
                    new TypeData(ElaTypeCode.Function),
                    new TypeData(ElaTypeCode.Object),
                    new TypeData(ElaTypeCode.Module),
                    new TypeData(ElaTypeCode.Lazy),
                    new TypeData(ElaTypeCode.__Reserved2),
                    new TypeData(ElaTypeCode.__Reserved3)
                };
            Cls = new FastList<Class>();
            Cls.AddRange(
                new Class[]
                {
                    new Class(), //ERR
                    new IntegerInstance(), //INT
                    new LongInstance(), //LNG
                    new SingleInstance(), //SNG
                    new DoubleInstance(), //DBL
                    new BooleanInstance(), //BYT
                    new CharInstance(), //CHR
                    new StringInstance(), //STR
                    new UnitInstance(), //UNI
                    new ListInstance(), //LST
                    new Class(), //TAB
                    new TupleInstance(), //TUP
                    new RecordInstance(), //REC
                    new FunctionInstance(), //FUN
                    new Class(), //OBJ
                    new ModuleInstance(), //MOD
                    new Class(), //LAZ
                    new Class(), //RES2
                    new Class(), //RES3
                });
		}
		#endregion


		#region Methods
        internal void RegisterForeignModule(ForeignModule module)
		{
			foreignModules.Add(module);
		}


		internal int AddModule(FileInfo fi, CodeFrame module, bool qual, int logicHandle)
		{
            var name = ObtainModuleName(fi);
            return AddModule(name, module, qual, logicHandle);
		}


        private string ObtainModuleName(FileInfo fi)
        {
            return !String.IsNullOrEmpty(fi.Extension) && !fi.Extension.Contains("#") ?
                fi.FullName.Replace(fi.Extension, String.Empty).ToUpper() :
                fi.FullName.ToUpper();
        }


        internal int AddModule(string name, CodeFrame module, bool qual, int logicHandle)
        {
            var hdl = 0;

            if (!moduleMap.TryGetValue(name, out hdl))
            {
                moduleMap.Add(name, hdl = modules.Count);
                modules.Add(module);
                quals.Add(qual);
            }
            else
            {
                modules[hdl] = module;
                quals[hdl] = qual;
            }

            return hdl;
        }


		public bool IsModuleRegistered(string name)
		{
			return moduleMap.ContainsKey(name);
		}


		public CodeFrame GetRootModule()
		{
			return modules[MAIN_HDL];
		}


        internal CodeFrame GetModule(FileInfo fi, out int hdl)
        {
            var name = ObtainModuleName(fi);
            return GetModule(name, out hdl);
        }

        internal CodeFrame GetModule(string name, out int hdl)
        {
            hdl = 0;

            if (moduleMap.TryGetValue(name, out hdl))
                return GetModule(hdl);
            else
                return null;
        }


		public CodeFrame GetModule(int handle)
		{
			return modules[handle];
		}


        internal int TryGetModuleHandle(string name)
        {
            var val = 0;

            if (!moduleMap.TryGetValue(name, out val))
                return -1;

            return val;
        }


		public int GetModuleHandle(string name)
		{
			return moduleMap[name];
		}


		public string GetModuleName(int handle)
		{
			foreach (var kv in moduleMap)
                if (kv.Value == handle)
                {
                    var fi = new FileInfo(kv.Key);
                    return fi.Extension.Length > 0 ?
                        fi.Name.Replace(fi.Extension, String.Empty).ToLower()
                        : fi.Name.ToLower();
                }

            return null;
		}


		public IEnumerable<ForeignModule> EnumerateForeignModules()
		{
			return foreignModules;
		}


		public IEnumerable<String> EnumerateModules()
		{
			return moduleMap.Keys;
		}


		internal void RefreshRootModule(CodeFrame frame)
		{
			if (frame != null)
				modules[0] = frame;
		}


        internal bool RequireQuailified(int moduleHandle)
        {
            return quals[moduleHandle];
        }
		#endregion


		#region Properties
		public int ModuleCount
		{
			get { return modules.Count; }
		}

        internal int ClassIndexer { get; set; }

        internal Dictionary<Int64,Byte> Instances { get; private set; }

        internal FastList<TypeData> Types { get; private set; }

        internal FastList<ConstructorData> Constructors { get; private set; }

        internal FastList<Class> Cls { get; private set; }
		#endregion
	}
}