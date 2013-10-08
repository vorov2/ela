using System;
using System.IO;
using Ela;

namespace Elide.ElaCode
{
    public static class ModuleFileInfoExtensions
    {
        public static FileInfo ToFileInfo(this ModuleFileInfo self)
        {
            return self != null ? new FileInfo(self.FullName) : null;
        }
    }

    public static class FileInfoExtensions
    {
        public static ModuleFileInfo ToModuleFileInfo(this FileInfo self)
        {
            return self != null ? new ModuleFileInfo(self.FullName) : null;
        }
    }
}
