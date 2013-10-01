using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment.Configuration;

namespace Elide.Configuration
{
    public sealed class ConfigService : AbstractConfigService
    {
        private readonly Dictionary<Type,Config> configInstances;
        private BinarySerializer serializer;

        public ConfigService()
        {
            configInstances = new Dictionary<Type,Config>();
            serializer = new BinarySerializer();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
        }

        public override void Unload()
        {
            base.Unload();
            Save();
        }

        public override IEnumerable<Config> EnumerateConfigs()
        {
            foreach (var t in configInstances.Keys.ToArray())
            {
                var c = configInstances[t];

                if (c == null)
                {
                    c = Load(t) ?? (Config)Activator.CreateInstance(t);
                    configInstances[t] = c;
                }

                yield return c;
            }
        }
        
        public override Config QueryConfig(Type configType)
        {
            var ret = default(Config);

            if (!configInstances.TryGetValue(configType, out ret))
            {
                ret = Load(configType) ?? (Config)Activator.CreateInstance(configType);
                configInstances[configType] = ret;
            }

            return ret;
        }

        public override void UpdateConfig(Config config)
        {
            var type = config.GetType();
            configInstances[type] = config;
            OnConfigUpdated(new ConfigEventArg(config));
        }
        
        private void Save()
        {
            foreach (var t in configInstances.Keys)
            {
                var c = configInstances[t];

                if (c != null)
                    serializer.Serialize(c, t.FullName);
            }
        }
        
        private Config Load(Type type)
        {
            var obj = serializer.Deserialize(type.FullName);
            return obj as Config;
        }
    }
}
