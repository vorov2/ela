using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class ReferenceResolverReader : IExtReader
    {
        private readonly ReferenceResolverService service;

        internal ReferenceResolverReader(ReferenceResolverService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Resolvers = section.Entries.Select(s => new ResolverInfo(s.Key, TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
