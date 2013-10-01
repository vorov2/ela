using System;

namespace Elide.Environment.Configuration
{
    public sealed class ConfigEventArg : EventArgs
    {
        public ConfigEventArg(Config config)
        {
            Config = config;
        }

        public Config Config { get; private set; }
    }
}
