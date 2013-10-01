using System;
using System.Collections.Generic;
using System.Xml;

namespace Elide.Core
{
    public sealed class ExtParser
    {
        private readonly XmlReader reader;

        public ExtParser(XmlReader reader)
        {
            this.reader = reader;
        }

        public ExtList<ExtSection> Parse()
        {
            var sections = new List<ExtSection>();

            if (reader.ReadToFollowing("configuration"))
            {
                var dict = new Dictionary<String,String>();

                while (reader.Read())
                {
                    if (reader.IsStartElement())
                    {
                        var o = ReadSection(reader.ReadSubtree());

                        if (dict.ContainsKey(o.Name))
                            throw new ElideException("Duplicate section name '{0}'.", o.Name);

                        dict.Add(o.Name, null);
                        sections.Add(o);
                    }
                }
            }

            return new ExtList<ExtSection>(sections);
        }

        private ExtSection ReadSection(XmlReader reader)
        {
            reader.Read();
            var name = reader.Name;
            var entries = ReadEntries(reader.ReadSubtree());
            return new ExtSection(name, entries);
        }

        private ExtList<ExtEntry> ReadEntries(XmlReader reader)
        {
            reader.Read();
            var list = new List<ExtEntry>();
            var dict = new Dictionary<String,String>();

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    var o = ReadEntry(reader.ReadSubtree());
                    
                    if (dict.ContainsKey(o.Key))
                        throw new ElideException("Duplicate entry key '{0}'.", o.Key);

                    dict.Add(o.Key, null);
                    list.Add(o);
                }
            }

            return new ExtList<ExtEntry>(list);
        }

        private ExtEntry ReadEntry(XmlReader reader)
        {
            reader.Read();
            var key = reader.GetAttribute("key");
            var els = new List<ExtElement>();

            for (var i = 0; i < reader.AttributeCount; i++)
            {
                reader.MoveToAttribute(i);
                
                if (reader.Name != "key")
                    els.Add(ReadElement(reader));
            }

            reader.MoveToElement();
            var cls = ReadEntries(reader.ReadSubtree());
            return new ExtEntry(key, new ExtList<ExtElement>(els), cls);
        }

        private ExtElement ReadElement(XmlReader reader)
        {
            return new ExtElement(reader.Name, reader.Value);
        }
    }
}
