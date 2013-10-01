using System;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Reference
    {
        internal Reference(string moduleName, string alias, string dll, string path, bool requireQualified)
        {
            ModuleName = moduleName;
            Alias = alias;
            Dll = dll;
            Path = path;
            RequireQualified = requireQualified;
        }

        public string ModuleName { get; private set; }

        public string Alias { get; private set; }

        public string Dll { get; private set; }

        public string Path { get; private set; }

        public bool RequireQualified { get; private set; }
    }
}
