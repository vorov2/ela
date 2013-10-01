using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public abstract class AbstractConfigService : Service, IConfigService
    {
        protected AbstractConfigService()
        {

        }

        public abstract IEnumerable<Config> EnumerateConfigs();

        public abstract Config QueryConfig(Type configType);

        public abstract void UpdateConfig(Config config);

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new ConfigReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Configs.OfType<ExtInfo>();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Configs.FirstOrDefault(c => c.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "configs")
                throw new ElideException("ConfigService doesn't support '{0}' section.", section);
        }

        protected internal IEnumerable<ConfigInfo> Configs { get; internal set; }
        
        public event EventHandler<ConfigEventArg> ConfigUpdated;
        protected virtual void OnConfigUpdated(ConfigEventArg e)
        {
            var h = ConfigUpdated;

            if (h != null)
                h(this, e);
        }
    }
}
