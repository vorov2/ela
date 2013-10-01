using System;
using System.IO;

namespace Elide.CodeWorkbench.Documentation
{
    public sealed class DocFile : DocItem
    {
        public DocFile(string name, FileInfo file) : base(name)
        {
            File = file;
        }

        public FileInfo File { get; private set; }
    }
}
