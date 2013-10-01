using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class CodeRunnerReader : IExtReader
    {
        private readonly AbstractCodeRunnerService service;

        internal CodeRunnerReader(AbstractCodeRunnerService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Runners = section.Entries.Select(s => new CodeRunnerInfo(s.Key, TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
