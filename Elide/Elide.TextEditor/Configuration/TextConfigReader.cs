using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;
using Elide.Scintilla.ObjectModel;

namespace Elide.TextEditor.Configuration
{
    internal sealed class TextConfigReader : IExtReader
    {
        private readonly AbstractTextConfigService service;

        public TextConfigReader(AbstractTextConfigService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Configs = section.Entries.Select(e => new TextConfigInfo(
                e.Key,
                e.Element("display"),
                e.Element<TextConfigOptions>("options"))
                {
                    IndentMode = e.ElementNullable<IndentMode>("indentMode"),
                    UseTabs = e.ElementNullable<Boolean>("useTabs"),
                    TabSize = e.ElementNullable<Int32>("tabSize"),
                    IndentSize = e.ElementNullable<Int32>("indentSize")
                }
                ).ToList();
        }
    }
}
