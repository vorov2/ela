using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    internal sealed class TaskProviderReader : IExtReader
    {
        private readonly AbstractTaskListService service;

        internal TaskProviderReader(AbstractTaskListService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Providers = section.Entries.Select(s => new TaskProviderInfo(s.Key, s.Element("editorKey"), TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
