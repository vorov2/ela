using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public interface IStylesConfigService : IExtService
    {
        IEnumerable<StyleItemConfig> EnumerateStyleItems(string styleGroupKey);
    }
}
