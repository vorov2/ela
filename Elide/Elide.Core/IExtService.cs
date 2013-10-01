using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.Core
{
    public interface IExtService : IService
    {
        IExtReader CreateExtReader(string section);

        IEnumerable<ExtInfo> EnumerateInfos(string section);

        ExtInfo GetInfo(string section, string key);
    }
}
