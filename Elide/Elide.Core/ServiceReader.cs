using System;
using System.Linq;

namespace Elide.Core
{
    internal sealed class ServiceReader : IExtReader
    {
        private readonly AbstractApp app;

        internal ServiceReader(AbstractApp app)
        {
            this.app = app;
        }

        public void Read(ExtSection section)
        {
            app.Services = section.Entries.Select(ent => new ServiceInfo(ent.Key, 
                TypeFinder.Get(ent.Element("interface")), TypeFinder.Get(ent.Element("type")),
                ent.Children.Select(e => e.Key)));
        }
    }
}
