using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Elide.Core;
using Elide.Environment;
using Elide.Workbench.Configuration;

namespace Elide.Workbench
{
    public sealed class RecentFilesService : Service, IRecentFilesService
    {
        public RecentFilesService()
        {

        }

        public void AddRecentFile(FileInfo fileInfo)
        {
            var con = App.Config<WorkbenchConfig>();

            if (con.RecentFiles == null)
                con.RecentFiles = new List<String>();

            var fn = fileInfo.FullName;

            if (con.RecentFiles.Contains(fn))
                return;

            if (con.RecentFiles.Count == con.RecentFilesMax)
                con.RecentFiles.RemoveAt(con.RecentFiles.Count - 1);

            con.RecentFiles.Insert(0, fn);
        }

        public IEnumerable<FileInfo> EnumerateRecentFiles()
        {
            var files = App.Config<WorkbenchConfig>().RecentFiles;
            return files == null ? new List<FileInfo>() : files.Select(f => new FileInfo(f)).ToList();
        }
    }
}
