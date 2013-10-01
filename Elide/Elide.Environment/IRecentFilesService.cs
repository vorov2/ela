using System;
using System.Collections.Generic;
using System.IO;
using Elide.Core;

namespace Elide.Environment
{
    public interface IRecentFilesService : IService
    {
        void AddRecentFile(FileInfo fileInfo);

        IEnumerable<FileInfo> EnumerateRecentFiles();
    }
}
