using System;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class CodeLexerReader : IExtReader
    {
        private readonly CodeLexerService service;

        internal CodeLexerReader(CodeLexerService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Lexers = section.Entries.Select(s => new CodeLexerInfo(s.Key, TypeFinder.Get(s.Element("type")))).ToList();
        }
    }
}
