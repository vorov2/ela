using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class CodeBuilderReader : IExtReader
    {
        private readonly AbstractCodeBuilderService service;

        internal CodeBuilderReader(AbstractCodeBuilderService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Builders = section.Entries.Select(s => new CodeBuilderInfo(s.Key, TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
