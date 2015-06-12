using System;
using System.IO;
using Ela.Compilation;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.Linking
{
    public sealed class ElaIncrementalLinker : ElaIncrementalLinker<ElaParser,ElaCompiler>
    {
        public ElaIncrementalLinker(LinkerOptions linkerOptions, CompilerOptions compOptions, ModuleFileInfo file) :
            base(linkerOptions, compOptions, file)
        {

        }

        public ElaIncrementalLinker(LinkerOptions linkerOptions, CompilerOptions compOptions) :
            base(linkerOptions, compOptions, null)
        {

        }
    }


    public class ElaIncrementalLinker<P,C> : ElaLinker<P,C>
        where P : IElaParser, new() where C : IElaCompiler, new()
    {
        private string source;
        private Dictionary<String,ExportVars> exportMap = new Dictionary<String,ExportVars>();
        
        public ElaIncrementalLinker(LinkerOptions linkerOptions, CompilerOptions compOptions, ModuleFileInfo file) :
            base(linkerOptions, compOptions, file)
        {

        }

        public ElaIncrementalLinker(LinkerOptions linkerOptions, CompilerOptions compOptions) :
            base(linkerOptions, compOptions, null)
        {
            
        }

        public override LinkerResult Build()
        {
            Messages.Clear();
            Success = true;
            var mod = new ModuleReference(Path.GetFileNameWithoutExtension(SafeRootFileName));
            var frame = default(CodeFrame);
            var scope = default(Scope);
            var scratch = true;

            if (Assembly.ModuleCount != 0)
            {
                var root = Assembly.GetRootModule();
                frame = root.Clone();
                scope = root.GlobalScope.Clone();
                scratch = false;
            }

            frame = Build(null, mod, SafeRootFile, source, frame, scope);
            RegisterFrame(mod, frame, SafeRootFile, !scratch, -1);

            if (Success)
                Assembly.RefreshRootModule(frame);
            else if (scratch)
                Assembly = new CodeAssembly();
            
            return new LinkerResult(Assembly, Success, Messages);
        }

        protected override ExportVars CreateExportVars(ModuleFileInfo fi)
        {
            var vars = default(ExportVars);
            var key = fi == null ? MemoryFile : fi.ToString();

            if (!exportMap.TryGetValue(key, out vars))
            {
                vars = base.CreateExportVars(fi);
                exportMap.Add(key, vars);
            }

            return vars;
        }

        public void SetSource(string source)
        {
            this.source = source;
        }
    }
}
