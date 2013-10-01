using System;
using System.Collections.Generic;
using Elide.Environment.Configuration;

namespace Elide.TextEditor.Configuration
{
    [Serializable]
    public sealed class TextEditorsConfig : Config
    {
        public TextEditorsConfig()
        {
            Configs = new Dictionary<String,TextConfig>();
            Default = TextConfig.CreateDefault();
        }

        public override Config Clone()
        {
            var ret = new TextEditorsConfig
            {
                Default = (TextConfig)Default.Clone()
            };

            if (Configs != null)
            {
                ret.Configs = new Dictionary<String,TextConfig>(Configs);
                Configs.ForEach(kv => ret.Configs[kv.Key] = (TextConfig)kv.Value.Clone());
            }

            return ret;
        }

        public Dictionary<String,TextConfig> Configs { get; set; }

        public TextConfig Default { get; set; }
    }
}
