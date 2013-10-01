using System;
using Ela.Compilation;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.ObjectModel
{
    public sealed class Reference : IReference
    {
        internal Reference(CompiledUnit unit, ModuleReference mod)
        {
            Module = mod;
            Unit = unit;
        }

        public override string ToString()
        {
            return Name;
        }

        internal ModuleReference Module { get; private set; }

        internal CompiledUnit Unit { get; private set; }

        public string Name
        {
            get { return Module.ModuleName; }
        }
    }
}
