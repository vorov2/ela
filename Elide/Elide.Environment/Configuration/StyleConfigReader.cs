using System;
using System.Drawing;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    internal sealed class StyleConfigReader : IExtReader
    {
        private readonly AbstractStylesConfigService service;

        internal StyleConfigReader(AbstractStylesConfigService service)
        {
            this.service = service;
        }

        public void Read(ExtSection section)
        {
            service.Groups = section.Entries.Select<ExtEntry,StyleGroupInfo>(ProcessGroup);
        }

        private StyleGroupInfo ProcessGroup(ExtEntry entry)
        {
            var k = entry.Key;
            var dn = entry.Element("display");

            var configs = entry.Children.Select(c =>
                    new StyleItemConfig
                    {
                        Type = c.Key,
                        DisplayName = c.Element("display") ?? c.Key,
                        FontName = c.Element("font"),
                        FontSize = c.Element<Int32>("size", 0),
                        ForeColor = c.Element<KnownColor?>("fore"),
                        BackColor = c.Element<KnownColor?>("back"),
                        Bold = c.Element<Boolean?>("bold"),
                        Italic = c.Element<Boolean?>("italic"),
                        Underline = c.Element<Boolean?>("underline")
                    }
                );

            return new StyleGroupInfo(k, dn, configs);
        }
    }
}
