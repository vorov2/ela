using System;
using Elide.Core;

namespace Elide.Main
{
    public interface IPathService : IService
    {
        string GetPath(PlatformPath path);

        string ResolvePathMacros(string path);
    }
}
