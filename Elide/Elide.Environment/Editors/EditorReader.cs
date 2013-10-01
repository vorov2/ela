using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;

namespace Elide.Environment.Editors
{
    internal sealed class EditorReader : IExtReader
    {
        private readonly AbstractEditorService service;

        public EditorReader(AbstractEditorService service)
        {
            this.service = service;
        }
        
        public void Read(ExtSection section)
        {
            service.Editors = section.Entries.Select(e => new EditorInfo(
                e.Key,
                TypeFinder.Get(e.Element("type")),
                TypeFinder.Get(e.Element("documentType")), 
                e.Element("displayName"),
                e.Element("fileExtension"),
                e.Element("fileExtensionDescription"),
                e.Element<EditorFlags>("flags")))
                .ToList();
        }
    }
}
