using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public interface IConfigService : IExtService
    {
        IEnumerable<Config> EnumerateConfigs();

        Config QueryConfig(Type configType);

        void UpdateConfig(Config config);

        event EventHandler<ConfigEventArg> ConfigUpdated;
    }
}
