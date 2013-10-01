using System;
using System.IO;
using System.Xml;
using Elide.Core;
using Elide.Main;

namespace Elide.CodeWorkbench.CodeSamples
{
    internal sealed class CodeSamplesReader
    {
        private readonly string baseDir;

        internal CodeSamplesReader(IApp app)
        {
            this.baseDir = Path.Combine(app.GetService<IPathService>().GetPath(PlatformPath.Docs), "samples");
        }

        public CodeSampleFolder Read()
        {
            var fileInfo = new FileInfo(Path.Combine(baseDir, "_dir.xml"));
            var root = new CodeSampleFolder("Root", String.Empty);

            if (fileInfo.Exists)
            {
                using (var xmlReader = XmlReader.Create(fileInfo.OpenRead()))
                    ReadElements(xmlReader, root);
            }

            return root;
        }

        private void ReadElements(XmlReader reader, CodeSampleFolder parent)
        {
            while (reader.Read())
            {
                if (reader.IsStartElement("folder"))
                {
                    var folder = new CodeSampleFolder(reader.GetAttribute("name"), reader.GetAttribute("description"));
                    parent.AddNode(folder);
                    var sr = reader.ReadSubtree();
                    sr.Read();
                    ReadElements(sr, folder);
                }
                else if (reader.IsStartElement("sample"))
                {
                    var file = new FileInfo(Path.Combine(baseDir, reader.GetAttribute("file")));

                    if (file.Exists)
                    {
                        var sample = new CodeSampleFile(reader.GetAttribute("name"), file, reader.GetAttribute("description"));
                        parent.AddNode(sample);
                    }
                }
            }
        }
    }
}
