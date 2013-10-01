using System;
using System.Collections.Generic;
using System.IO;
using Ela.CodeModel;
using Ela.Compilation;
using Ela.Parsing;
using Ela.Runtime;
using Ela.Runtime.Classes;
using System.Diagnostics;

namespace Ela.Linking
{
	public sealed class ElaLinker : ElaLinker<ElaParser,ElaCompiler>
	{
		#region Construction
		public ElaLinker(LinkerOptions linkerOptions, CompilerOptions compOptions, FileInfo rootFile) :
			base(linkerOptions, compOptions, rootFile)
		{

		}
		#endregion
	}


	public class ElaLinker<P,C> where P : IElaParser, new() where C : IElaCompiler, new()
	{
		#region Construction
		private const string EXT = ".ela";
		private const string OBJEXT = ".elaobj";
		private const string DLLEXT = ".dll";
        internal const string ARG_MODULE = "$___ARGS___";

		internal const string MemoryFile = "memory";
        private static readonly ModuleReference argModuleRef = new ModuleReference("$Args");
		private Dictionary<String,Dictionary<String,ElaModuleAttribute>> foreignModules;
        private Dictionary<String,FileInfo> foreignModulesFiles;
		private FastList<DirectoryInfo> dirs;
		private ArgumentModule argModule;
        private CodeFrame argModuleFrame;
		
		public ElaLinker(LinkerOptions linkerOptions, CompilerOptions compOptions, FileInfo rootFile)
		{
			dirs = new FastList<DirectoryInfo>();
            
			if (linkerOptions.CodeBase.LookupStartupDirectory && rootFile != null)
				dirs.Add(rootFile.Directory);

			dirs.AddRange(linkerOptions.CodeBase.Directories);
			LinkerOptions = linkerOptions;
			CompilerOptions = compOptions;
			RootFile = rootFile;
			Messages = new List<ElaMessage>();
			Assembly = new CodeAssembly();
			Success = true;
			foreignModules = new Dictionary<String,Dictionary<String,ElaModuleAttribute>>();
            foreignModulesFiles = new Dictionary<String,FileInfo>();
		}
		#endregion


		#region Methods
        public virtual void AddArgument(string name, object value)
        {
            if (argModule == null)
                argModule = new ArgumentModule();

			argModule.AddArgument(name, value);
        }


		public virtual LinkerResult Build()
		{
			var mod = new ModuleReference(Path.GetFileNameWithoutExtension(SafeRootFileName));
            var frame = Build(null, mod, SafeRootFile);
			RegisterFrame(new ModuleReference(
                Path.GetFileNameWithoutExtension(SafeRootFileName)), frame, SafeRootFile, false, -1);
			return new LinkerResult(Assembly, Success, Messages);
		}


        public virtual LinkerResult Build(string source)
		{
			var mod = new ModuleReference(Path.GetFileNameWithoutExtension(SafeRootFileName));
            var frame = Build(null, mod, SafeRootFile, source, null, null);
			RegisterFrame(new ModuleReference(
                Path.GetFileNameWithoutExtension(SafeRootFileName)), frame, SafeRootFile, false, -1);
			return new LinkerResult(Assembly, Success, Messages);
		}

		
        //Modules that are being processed (but are not completely processed yet, used to detect cyclic references).
        private Dictionary<String,String> inProc = new Dictionary<String,String>();
		internal CodeFrame ResolveModule(FileInfo parentFile, ModuleReference mod, ExportVars exportVars)
		{
            var frame = default(CodeFrame);
            var hdl = -1;

            if (mod.ModuleName == "lang" && mod.Path.Length == 0 && mod.DllName == null)
            {
                if ((frame = Assembly.GetModule("LANG", out hdl)) == null)
                {
                    var lm = new LangModule();
                    lm.Initialize();
                    frame = lm.Compile();
                    hdl = Assembly.AddModule("LANG", frame, false, mod.LogicalHandle);
                }
            }
            else if (mod.ModuleName == ARG_MODULE)
            {
                if ((frame = Assembly.GetModule("ARGS", out hdl)) == null)
                {
                    if (argModule == null)
                        argModule = new ArgumentModule();

                    if (argModuleFrame == null)
                        argModuleFrame = argModule.Compile();

                    frame = argModuleFrame;
                    hdl = Assembly.AddModule("ARGS", argModuleFrame, false, mod.LogicalHandle);
                }
            }
            else
            {
                if (mod.DllName != null)
                    frame = ResolveDll(mod, out hdl);
                else
                {
                    var ela = String.Concat(mod.ModuleName, EXT);
                    var obj = LinkerOptions.ForceRecompile ? null : String.Concat(mod.ModuleName, OBJEXT);
                    bool bin;
                    var fi = FindModule(mod, ela, obj, out bin);
                                            
                    if (fi != null)
                    {
                        var kv = fi.FullName.ToUpper();

                        if (inProc.ContainsKey(kv))
                            AddError(ElaLinkerError.CyclicReference, parentFile, mod.Line, mod.Column, fi.FullName);
                        else
                        {
                            inProc.Add(kv, null);

                            if ((frame = Assembly.GetModule(fi, out hdl)) != null)
                            {
                                //Do nothing
                            }
                            else if (!bin)
                            {
                                frame = Build(parentFile, mod, fi);
                                hdl = RegisterFrame(mod, frame, fi, false, mod.LogicalHandle);
                            }
                            else
                            {
                                frame = ReadObjectFile(parentFile, mod, fi);
                                hdl = RegisterFrame(mod, frame, fi, false, mod.LogicalHandle);
                            }

                            inProc.Remove(kv);
                        }
                    }
                    else
                    {
                        frame = TryResolveModule(mod);
                        hdl = RegisterFrame(mod, frame, fi, false, mod.LogicalHandle);
                    }
                }
            }
            
            if (mod.Parent != null)
                mod.Parent.HandleMap[mod.LogicalHandle] = hdl;

            return frame != null && !mod.RequireQuailified ? FillExports(frame, exportVars, mod.LogicalHandle) : frame;
		}


        private void ProcessTypeAndClasses(CodeFrame frame, bool reload, int hdl)
        {
            foreach (var k in new List<String>(frame.InternalTypes.Keys))
            {
                if (frame.InternalTypes[k] == -1)
                {
                    var c = Assembly.Types.Count;
                    frame.InternalTypes[k] = c;
                    Assembly.Types.Add(new TypeData(c, k));
                    Assembly.Cls.Add(new Class());
                }
            }

            foreach (var k in new List<String>(frame.InternalClasses.Keys))
            {
                if (frame.InternalClasses[k].Code == -1)
                {
                    var c = Assembly.ClassIndexer;
                    frame.InternalClasses[k].Code = c;
                    Assembly.ClassIndexer++;
                }
            }

            foreach (var d in frame.InternalConstructors)
            {
                if (d.Code == -1)
                {
                    var c = Assembly.Constructors.Count;
                    d.Code = c;
                    d.ModuleId = hdl;
                    d.ConsAddress = frame.GlobalScope.Locals[d.Name].Address;
                    Assembly.Constructors.Add(d);

                    var ti = default(TypeData);

                    if (d.TypeModuleId == -1)
                        ti = Assembly.Types[frame.InternalTypes[d.TypeName]];
                    else
                        ti = Assembly.Types[Assembly.GetModule(frame.HandleMap[d.TypeModuleId]).InternalTypes[d.TypeName]];

                    ti.Constructors.Add(d.Code);
                    d.TypeCode = ti.TypeCode;
                    d.Private = (frame.GlobalScope.Locals[d.Name].Flags & ElaVariableFlags.Private) == ElaVariableFlags.Private;
                }
            }

            foreach (var id in frame.InternalInstances)
            {
                var typeModule = Assembly.GetModule(id.TypeModuleId == -1 ? hdl : frame.HandleMap[id.TypeModuleId]);
                var typeCode = typeModule.InternalTypes[id.Type];

                var classModule = Assembly.GetModule(id.ClassModuleId == -1 ? hdl : frame.HandleMap[id.ClassModuleId]);
                var classCode = (Int64)classModule.InternalClasses[id.Class].Code;

                var lo = (Int64)typeCode << 32;
                long instanceCode = classCode | lo;

                if (Assembly.Instances.ContainsKey(instanceCode))
                {
                    if (!reload)
                        AddError(ElaLinkerError.InstanceAlreadyExists, frame.File, id.Line, id.Column, id.Class, id.Type);
                }
                else
                    Assembly.Instances.Add(instanceCode, 0);
            }
        }


        private CodeFrame FillExports(CodeFrame frame, ExportVars exportVars, int logicHandle)
        {
            foreach (var kv in frame.GlobalScope.EnumerateVars())
            {
                var sv = kv.Value;

                if ((sv.Flags & ElaVariableFlags.Private) != ElaVariableFlags.Private &&
                    (sv.Flags & ElaVariableFlags.Qualified) != ElaVariableFlags.Qualified)
                    exportVars.AddVariable(kv.Key, 
                        (sv.Flags & ElaVariableFlags.Builtin) == ElaVariableFlags.Builtin ? (ElaBuiltinKind)sv.Data : ElaBuiltinKind.None,
                        sv.Flags,
                        sv.Data,
                        logicHandle, sv.Address);
            }

            return frame;
        }


        private void CheckBinaryConsistency(CodeFrame obj, ModuleReference mod, FileInfo fi, ExportVars exportVars)
        {
            foreach (var l in obj.LateBounds)
            {
                var vk = default(ExportVarData);
                var found = exportVars.FindVariable(l.Name, out vk);

                if (!found || l.Address >> 8 != vk.Address || (l.Address & Byte.MaxValue) != vk.ModuleHandle)
                    AddError(ElaLinkerError.ExportedNameRemoved, fi,
                        mod != null ? mod.Line : 0,
                        mod != null ? mod.Column : 0,
                        l.Name);

                if (found &&
                        (
                        (vk.Kind != ElaBuiltinKind.None && (Int32)vk.Kind != l.Data && l.Data != -1)
                        )
                    )
                    AddError(ElaLinkerError.ExportedNameChanged, fi,
                        mod != null ? mod.Line : 0,
                        mod != null ? mod.Column : 0,
                        l.Name);
            }
        }


		private CodeFrame ReadObjectFile(FileInfo parentFile, ModuleReference mod, FileInfo fi)
		{
			var obj = new ObjectFileReader(fi);
			
			try
			{
				var frame = obj.Read();
                frame.HandleMap = new FastList<Int32>(frame.References.Count);
                frame.HandleMap.Normalize();
                var exportVars = CreateExportVars(fi);

                foreach (var r in frame.References)
                {
                    if (mod == null || !mod.NoPrelude || CompilerOptions.Prelude != r.Value.ModuleName)
                        ResolveModule(fi, r.Value, exportVars);
                }

                CheckBinaryConsistency(frame, mod, fi, exportVars);
				return frame;
			}
			catch (ElaException ex)
			{
				AddError(ElaLinkerError.ObjectFileReadFailed, 
                    parentFile ?? fi, 
                    mod != null ? mod.Line : 0, 
                    mod != null ? mod.Column : 0, 
                    fi.Name, ex.Message);
				return null;
			}
		}


		internal int RegisterFrame(ModuleReference mod, CodeFrame frame, FileInfo fi, bool reload, int logicHandle)
		{
            if (frame != null)
            {
                frame.File = fi;
                var hdl =  Assembly.AddModule(fi, frame, mod.RequireQuailified, logicHandle);
                ProcessTypeAndClasses(frame, reload, hdl);
                return hdl;
            }
            else
                return -1;
		}


        protected virtual ExportVars CreateExportVars(FileInfo fi)
        {
            return new ExportVars();
        }


		private CodeFrame ResolveDll(ModuleReference mod, out int hdl)
		{
			var dict = default(Dictionary<String,ElaModuleAttribute>);
			var fi = default(FileInfo);
            hdl = -1;

			if (foreignModules.TryGetValue(mod.DllName, out dict) || LoadAssemblyFile(mod, out fi))
			{
                if (dict == null)
                {
                    dict = foreignModules[mod.DllName];
                    foreignModulesFiles.Add(mod.DllName, fi);
                }
                else
                    fi = foreignModulesFiles[mod.DllName];

				var attr = default(ElaModuleAttribute);

				if (!dict.TryGetValue(mod.ModuleName, out attr))
					AddError(ElaLinkerError.ModuleNotFoundInAssembly, new FileInfo(mod.DllName), mod.Line, mod.Column,
						mod.ModuleName, mod.DllName);
				else
					return LoadModule(mod, attr, fi, out hdl);
			}

			return null;
		}


		private bool LoadAssemblyFile(ModuleReference mod, out FileInfo fi)
		{
			if (mod.DllName.IndexOfAny(Path.GetInvalidFileNameChars()) != -1)
			{
				AddError(ElaLinkerError.ModuleNameInvalid, new FileInfo("Undefined"), mod.Line, mod.Column, mod.DllName);
				fi = null;
				return false;
			}
			else
			{
				var dll = Path.HasExtension(mod.DllName) ? mod.DllName : String.Concat(mod.DllName, DLLEXT);
				bool uns;
				fi = FindModule(mod, dll, null, out uns);

				if (fi == null)
				{
					UnresolvedModule(mod);
					return false;
				}
				else
					return LoadAssembly(mod, fi);
			}
		}


		private bool LoadAssembly(ModuleReference mod, FileInfo file)
		{
			var asm = default(System.Reflection.Assembly);

			try
			{
				asm = System.Reflection.Assembly.LoadFile(file.FullName);
			}
			catch (Exception ex)
			{
				AddError(ElaLinkerError.AssemblyLoad, file, mod.Line, mod.Column, file.Name, ex.Message);
				return false;
			}

			var attrs = asm.GetCustomAttributes(typeof(ElaModuleAttribute), false);

			if (attrs.Length == 0)
			{
				AddError(ElaLinkerError.ForeignModuleDescriptorMissing, file, mod.Line, mod.Column, mod);
				return false;
			}
			else
			{
                var dict = new Dictionary<String,ElaModuleAttribute>();

				foreach (ElaModuleAttribute a in attrs)
				{
					if (dict.ContainsKey(a.ModuleName))
					{
						AddError(ElaLinkerError.DuplicateModuleInAssembly, new FileInfo(mod.DllName), mod.Line, mod.Column,
							a.ModuleName, mod.DllName);
						return false;
					}
					else
						dict.Add(a.ModuleName, a);
				}

                foreignModules.Add(mod.DllName, dict);
			}

			return true;
		}


		private CodeFrame LoadModule(ModuleReference mod, ElaModuleAttribute attr, FileInfo fi, out int hdl)
		{
			var obj = default(ForeignModule);
            fi = new FileInfo(fi.FullName + "#" + mod.ModuleName);
            hdl = -1;

			try
			{
				obj = Activator.CreateInstance(attr.ClassType) as ForeignModule;
			}
			catch (Exception ex)
			{
				AddError(ElaLinkerError.ForeignModuleInitFailed, fi, mod.Line, mod.Column, mod, ex.Message);
				return null;
			}

			if (obj == null)
			{
				AddError(ElaLinkerError.ForeignModuleInvalidType, fi, mod.Line, mod.Column, mod);
				return null;
			}
			else
			{
				var frame = default(CodeFrame);

				try
				{
					obj.Initialize();
					Assembly.RegisterForeignModule(obj);
					frame = obj.Compile();
				}
				catch (Exception ex)
				{
					AddError(ElaLinkerError.ForeignModuleInitFailed, fi, mod.Line, mod.Column, mod, ex.Message);
					return null;
				}

                hdl = RegisterFrame(mod, frame, fi, false, mod.LogicalHandle);
				return frame;
			}
		}


		private CodeFrame Build(FileInfo parentFile, ModuleReference mod, FileInfo file)
		{
			return Build(parentFile, mod, file, null, null, null);
		}


        internal CodeFrame Build(FileInfo parentFile, ModuleReference mod, FileInfo file, string source,
            CodeFrame frame, Scope scope)
        {
            if (Assembly.ModuleCount == 0)
                Assembly.AddModule(file, frame, mod.RequireQuailified, mod.LogicalHandle);

            var ret = default(CodeFrame);

            //Check if an entry module is already precompiled
            if (source == null && file != null && file == RootFile)
            {
                if (!CheckRootFile(out ret))
                    return null;
                else if (ret != null)
                    return ret;
            }

            var pRes = Parse(file, source);
            
            if (pRes.Success)
            {
                var cRes = Compile(file, pRes.Program, frame, scope, mod.NoPrelude || mod.ModuleName == CompilerOptions.Prelude);
                ret = cRes.CodeFrame;

                if (cRes.Success)
                {
                    if (ret.Symbols != null)
                        ret.Symbols.File = file != null ? file : SafeRootFile;

                    return ret;
                }
                else
                    ret = null;
            }

            if (parentFile != null)
                AddError(ElaLinkerError.ModuleLinkFailed,
                    parentFile,
                    mod.Line,
                    mod.Column,
                    Path.GetFileNameWithoutExtension(file.Name));
            return ret;
        }


        internal bool CheckRootFile(out CodeFrame frame)
        {
            frame = null;
            var fnWex = Path.GetFileNameWithoutExtension(RootFile.FullName);
            
            var fnObj = new FileInfo(Path.Combine(RootFile.DirectoryName, fnWex + OBJEXT));
            var fnSrc = new FileInfo(Path.Combine(RootFile.DirectoryName, fnWex + EXT));
            RootFile = fnSrc;

            if (!LinkerOptions.ForceRecompile && 
                fnObj.Exists && (fnSrc.Exists && fnSrc.LastWriteTime <= fnObj.LastWriteTime) ||
                !fnSrc.Exists)
            {
                frame = ReadObjectFile(null, null, fnObj);
                return true;
            }
            
            if (!fnSrc.Exists && !fnObj.Exists)
            {
                AddError(ElaLinkerError.UnresolvedModule, RootFile, 0, 0);
                return false;
            }

            return true;
        }



		internal CompilerResult Compile(FileInfo file, ElaProgram prog, CodeFrame frame, Scope scope, bool noPrelude)
		{
			var elac = new C();
			var opts = CompilerOptions;

			if (noPrelude)
			{
				opts = opts.Clone();
				opts.Prelude = null;
			}

            var exportVars = CreateExportVars(file);
			elac.ModuleInclude += (o, e) => e.Frame = ResolveModule(file, e.Module, exportVars);
			var res = frame != null ? elac.Compile(prog, opts, exportVars, frame, scope) :
				elac.Compile(prog, opts, exportVars);
			AddMessages(res.Messages, file);
			return res;
		}


		internal ParserResult Parse(FileInfo file, string source)
		{
			var elap = new ElaParser();
			var res = source != null ? elap.Parse(source) : elap.Parse(file);
			AddMessages(res.Messages, file);
			return res;
		}


		private FileInfo FindModule(ModuleReference mod, string firstName, string secondName, out bool sec)
		{
			var ret1 = default(FileInfo);
			var ret2 = default(FileInfo);
			sec = false;
			
			foreach (var d in dirs)
			{
				if (secondName != null)
					ret2 = Combine(d, mod.Path, secondName);

				ret1 = Combine(d, mod.Path, firstName);

				if (ret2 != null && ret1 != null)
				{
					if (!LinkerOptions.SkipTimeStampCheck && ret2.LastWriteTime < ret1.LastWriteTime)
					{
						AddWarning(ElaLinkerWarning.ObjectFileOutdated, ret1, mod.Line, mod.Column, ret2.Name, ret1.Name);
						return ret1;
					}
					else
					{
						sec = true;
						return ret2;
					}
				}
				else if (ret1 != null)
					return ret1;
				else if (ret2 != null)
				{
					sec = true;
					return ret2;
				}
			}

			return null;
		}


		private CodeFrame TryResolveModule(ModuleReference mod)
		{
			var e = new ModuleEventArgs(mod);
			OnModuleResolve(e);

			if (e.HasModule)
				return e.GetFrame();

			UnresolvedModule(mod);
			return null;
		}


		private void UnresolvedModule(ModuleReference mod)
		{
			AddError(ElaLinkerError.UnresolvedModule, new FileInfo(mod.ToString()), mod.Line, mod.Column,
				Path.GetFileNameWithoutExtension(mod.ToString()));
		}


		private FileInfo Combine(DirectoryInfo dir, string[] path, string fileName)
		{
			var finPath = path.Length > 0 ?
				Path.Combine(Path.Combine(dir.FullName, String.Join(Path.DirectorySeparatorChar.ToString(), path)), fileName) :
				Path.Combine(dir.FullName, fileName);

			return File.Exists(finPath) ? new FileInfo(finPath) : null;
		}
		#endregion

		
		#region Service Methods
		internal void AddError(ElaLinkerError error, FileInfo file, int line, int col, params object[] args)
		{
			Success = false;

			Messages.Add(new ElaMessage(Strings.GetError(error, args),
				MessageType.Error, (Int32)error, line, col) { File = file });
		}


		internal void AddWarning(ElaLinkerWarning warning, FileInfo file, int line, int col, params object[] args)
		{
			if (LinkerOptions.NoWarnings)
				return;
			else if (LinkerOptions.WarningsAsErrors)
				AddError((ElaLinkerError)warning, file, line, col, args);
			else
				Messages.Add(new ElaMessage(Strings.GetWarning(warning, args),
					MessageType.Warning, (Int32)warning, line, col) { File = file });
		}


		internal void AddMessages(IEnumerable<ElaMessage> messages, FileInfo file)
		{
			foreach (var m in messages)
			{
				if (m.Type == MessageType.Error)
					Success = false;

				m.File = file;
				Messages.Add(m);
			}
		}
		#endregion


		#region Properties
		public LinkerOptions LinkerOptions { get; private set; }
		
		public CompilerOptions CompilerOptions { get; private set; }

		public FileInfo RootFile { get; private set; }

		internal List<ElaMessage> Messages { get; private set; }

		internal CodeAssembly Assembly { get; set; }

		internal bool Success { get; set; }

        protected FileInfo SafeRootFile
        {
            get { return RootFile != null ? RootFile : new FileInfo(MemoryFile); }
        }

        protected string SafeRootFileName
        {
            get { return RootFile != null ? RootFile.Name : MemoryFile; }
        }
		#endregion


		#region Events
		public event EventHandler<ModuleEventArgs> ModuleResolve;
		protected virtual void OnModuleResolve(ModuleEventArgs e)
		{
			var h = ModuleResolve;

			if (h != null)
				h(this, e);
		}
		#endregion
	}
}
