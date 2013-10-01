using System;
using System.Collections.Generic;

namespace Ela.Compilation
{
    internal sealed class ConstructorData
    {
        public string TypeName { get; internal set; }

        public int TypeModuleId { get; internal set; }

        public int Code { get; internal set; }

        internal int TypeCode { get; set; }

        public bool Private { get; internal set; }

        public string Name { get; internal set; }

        internal List<String> Parameters { get; set; }

        //These are populated only by linker
        internal int ModuleId;
        internal int ConsAddress;
    }
}
