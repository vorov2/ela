using System;
using System.Collections.Generic;
using System.Linq;
using Ela.Linking;
using Elide.CodeEditor.Infrastructure;
using Elide.Environment;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class CompiledAssembly : ICompiledAssembly
    {
        internal CompiledAssembly(Document rootDoc, CodeAssembly asm)
        {
            Assembly = asm;
            Units = Assembly.EnumerateModules()
                .Select(n => Assembly.GetModule(Assembly.GetModuleHandle(n)))
                .Select(m => new CompiledUnit(new VirtualDocument(m.File), m))
                .ToList();
            var root = Assembly.GetRootModule();
            MainUnit = new CompiledUnit(rootDoc, root);
        }

        public ICompiledUnit MainUnit { get; private set; }

        public IEnumerable<ICompiledUnit> Units { get; private set; }

        internal CodeAssembly Assembly { get; private set; }
    }
}
