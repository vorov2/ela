using System;
using System.IO;
using System.Reflection;
using Elide.Core;
using System.Diagnostics;

namespace Elide.Main
{
    public sealed class PathService : Service, IPathService
    {
        public PathService()
        {

        }

        public string GetPath(PlatformPath path)
        {
            switch (path)
            {
                case PlatformPath.Ela: return GetElaPath();
                case PlatformPath.Elide: return GetElidePath();
                case PlatformPath.Root: return GetRootPath();
                case PlatformPath.Docs: return GetDocsPath();
                default: return null;
            }
        }

        public string ResolvePathMacros(string path)
        {
            return path
                .Replace("%elide%", GetElidePath())
                .Replace("%ela%", GetElaPath())
                .Replace("%root%", GetRootPath())
                .Replace("%docs%", GetDocsPath());
        }

        private string GetDocsPath()
        {
            var elide = GetRootPath();
            var di = new DirectoryInfo(Path.Combine(elide, "docs"));
            return di.Exists ? di.FullName : elide;
        }

        private string GetElaPath()
        {
            var elide = GetElidePath();
            var di = new DirectoryInfo(Path.Combine(elide, "ela"));
            return di.Exists ? di.FullName : elide;
        }

        private string GetRootPath()
        {
            var di = new DirectoryInfo(GetElidePath());
            return di.Parent != null ? di.Parent.FullName : di.FullName;
        }

        private string GetElidePath()
        {
#if DEBUG
            return @"c:\ela-platform\elide\";
#else
            return new FileInfo(Assembly.GetEntryAssembly().Location).DirectoryName;
#endif
        }
    }
}
