using System;
using System.IO;

namespace Elide.Environment
{
    public sealed class VirtualDocument : Document
    {
        public VirtualDocument(FileInfo fileInfo) : base(fileInfo)
        {

        }

        public override bool IsDirty
        {
            get { return false; }
            set { }
        }
    }
}
