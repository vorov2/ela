using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class CodeParserReader : IExtReader
    {
        private readonly AbstractCodeParserService service;

        internal CodeParserReader(AbstractCodeParserService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Parsers = section.Entries.Select(s => new CodeParserInfo(s.Key, TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
