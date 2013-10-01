using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using Elide.Core;
using System.Collections.Generic;

namespace Elide
{
    internal sealed class ProgramContext : ApplicationContext
    {
        private readonly IApp app;
        
        public ProgramContext(string[] args)
        {
            var sections = ExtList<ExtSection>.Empty;
            var dir = new DirectoryInfo(Application.StartupPath);
            
            foreach (var fi in dir.GetFiles("*.xml"))
                sections = sections.Merge(ReadConfig(fi));

            app = new App(sections, args);
            var reader = app.CreateExtReader("services");
            reader.Read(sections.First(s => s.Name == "services"));
            app.Initialize(app);
            app.Run();
        }

        private ExtList<ExtSection> ReadConfig(FileInfo fi)
        {
            using (var reader = XmlReader.Create(fi.OpenRead()))
            {
                var p = new ExtParser(reader);
                return p.Parse();
            }
        }
    }
}
