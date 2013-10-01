using System;
using System.IO;
using System.Xml;
using Elide.Core;
using Elide.Main;

namespace Elide.CodeWorkbench.Documentation
{
    internal sealed class DocReader
    {
        private readonly string baseDir;

        internal DocReader(IApp app)
        {
            this.baseDir = app.GetService<IPathService>().GetPath(PlatformPath.Docs);
        }

        public DocFolder Read()
        {
            var fileInfo = new FileInfo(Path.Combine(baseDir, "_dir.xml"));
            var root = new DocFolder("Root");

            if (fileInfo.Exists)
            {
                using (var xmlReader = XmlReader.Create(fileInfo.OpenRead()))
                    ReadElements(xmlReader, root);
            }

            return root;
        }

        private void ReadElements(XmlReader reader, DocFolder parent)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement("folder"))
                {
                    var folder = new DocFolder(reader.GetAttribute("name"));
                    parent.AddNode(folder);
                    var sr = reader.ReadSubtree();
                    sr.Read();
                    ReadElements(sr, folder);
                }
                else if (reader.IsStartElement("doc"))
                {
                    var file = new FileInfo(Path.Combine(baseDir, reader.GetAttribute("file")));

                    if (file.Exists)
                    {
                        var sample = new DocFile(reader.GetAttribute("name"), file);
                        parent.AddNode(sample);
                    }
                }
            }
        }
    }
}
