using System;
using System.Collections.Generic;
using System.IO;
using Ela.Compilation;
using Ela.Linking;
using Ela.Parsing;
using Elide.CodeEditor.Infrastructure;
using Elide.Core;
using Elide.ElaCode.ObjectModel;
using Elide.Environment;

namespace Elide.ElaCode
{
    public sealed class ElaReferenceResolver : IReferenceResolver<Reference>
    {
        public static ExtendedOption NoBuild = new ExtendedOption(0x02, true);
        private LinkerOptions opts;
        private DirectoryInfo dir;

        public ElaReferenceResolver()
        {
            
        }

        private FileInfo FindModule(string path, string firstName, string secondName, out bool sec)
        {
            var ret1 = default(FileInfo);
            var ret2 = default(FileInfo);
            sec = false;

            var dirs = new List<DirectoryInfo>(opts.CodeBase.Directories);

            if (opts.CodeBase.LookupStartupDirectory && dir != null)
                dirs.Insert(0, dir);

            foreach (var d in dirs)
            {
                if (secondName != null)
                    ret2 = GetFileInfo(d, path, secondName);

                ret1 = GetFileInfo(d, path, firstName);

                if (ret2 != null && ret1 != null)
                {
                    if (!opts.SkipTimeStampCheck && ret2.LastWriteTime < ret1.LastWriteTime)
                        return ret1;
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


        private FileInfo GetFileInfo(DirectoryInfo d, string path, string name)
        {
            var fi = new FileInfo(Path.Combine(Path.Combine(d.FullName, path), name));
            return fi.Exists ? fi : null;
        }
        
        public ICompiledUnit Resolve(IReference reference, params ExtendedOption[] options)
        {
            var rf = (Reference)reference;
            var mod = rf.Module;
            dir = rf.Unit.Document.FileInfo != null ? rf.Unit.Document.FileInfo.Directory : null;
            opts = new BuildOptionsManager(App).CreateLinkerOptions();
            var frame = default(CodeFrame);

            if (String.IsNullOrEmpty(mod.DllName))
            {
                var ela = String.Concat(mod.ModuleName, ".ela");
                var obj = opts.ForceRecompile ? null : String.Concat(mod.ModuleName, ".elaobj");
                bool bin;
                var fi = FindModule(mod.GetPath(), ela, obj, out bin);

                if (fi != null)
                {
                    if (options.Set(NoBuild.Code))
                        return new CompiledUnit(new VirtualDocument(fi), null);
                    if (!bin)
                        frame = Build(mod, fi);
                    else
                        frame = ReadObjectFile(mod, fi);
                }
            }

            return frame != null ? new CompiledUnit(rf.Unit.Document, frame) : null;
        }

        private CodeFrame Build(ModuleReference mod, FileInfo fi)
        {
            try
            {
                var elap = new ElaParser();
                var pres = elap.Parse(fi);

                if (pres.Success)
                {
                    var elac = new ElaCompiler();
                    var copt = new BuildOptionsManager(App).CreateCompilerOptions();
                    return elac.Compile(pres.Program,
                        new CompilerOptions { IgnoreUndefined = true, NoWarnings = true, ShowHints = false, Prelude = copt.Prelude },
                        new ExportVars()
                        ).CodeFrame;
                }
            }
            catch { }

            return null;
        }

        private CodeFrame ReadObjectFile(ModuleReference mod, FileInfo fi)
        {
            var obj = new ObjectFileReader(fi);

            try
            {
                return obj.Read();
            }
            catch
            {
                return null;
            }
        }

        public IApp App { get; set; }
    }
}
