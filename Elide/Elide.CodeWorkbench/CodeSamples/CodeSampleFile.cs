using System;
using System.IO;

namespace Elide.CodeWorkbench.CodeSamples
{
    public sealed class CodeSampleFile : CodeSampleItem
    {
        public CodeSampleFile(string name, FileInfo file, string description) : base(name, description)
        {
            File = file;
        }

        public FileInfo File { get; private set; }
    }
}
