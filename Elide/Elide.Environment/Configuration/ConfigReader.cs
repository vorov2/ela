using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    internal sealed class ConfigReader : IExtReader
    {
        private readonly AbstractConfigService service;

        internal ConfigReader(AbstractConfigService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Configs = section.Entries.Select(e => new ConfigInfo(e.Key, TypeFinder.Get(e.Element("type")),
                e.Element("displayName"), e.Element("category"), e.Element<Int32>("position"), TypeFinder.Get(e.Element("widget")))).ToList();
        }
    }
}
