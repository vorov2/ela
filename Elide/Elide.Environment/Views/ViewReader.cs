using System;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Views
{
    internal sealed class ViewReader : IExtReader
    {
        private readonly AbstractViewService service;

        internal ViewReader(AbstractViewService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Views = section.Entries.Select(e => new ViewInfo(
                e.Key,
                e.Element("title"),
                e.Element("shortcut"),
                e.Element<ViewType>("kind"),
                TypeFinder.Get(e.Element("type"))
                ));
        }
    }
}
