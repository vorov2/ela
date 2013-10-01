using System;
using System.Collections.Generic;
using System.Linq;

namespace Elide.Core
{
    public sealed class ServiceInfo : ExtInfo
    {
        public ServiceInfo(string key, Type interfaceType, Type type, IEnumerable<String> regSections) : base(key)
        {
            InterfaceType = interfaceType;
            Type = type;
            RegSections = regSections;
        }

        public bool HasRegSections()
        {
            return RegSections != null && RegSections.Count() > 0;
        }

        public Type InterfaceType { get; private set; }

        public Type Type { get; private set; }

        public IEnumerable<String> RegSections { get; private set; }
    }
}
