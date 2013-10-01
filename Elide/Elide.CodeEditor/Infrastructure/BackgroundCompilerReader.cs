using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class BackgroundCompilerReader : IExtReader
    {
        private readonly BackgroundCompilerService service;

        internal BackgroundCompilerReader(BackgroundCompilerService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Compilers = section.Entries.Select(s => new BackgroundCompilerInfo(s.Key, s.Element("editorKey"),
                TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
