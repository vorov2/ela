using System;
using System.Linq;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.TextEditor.Configuration;

namespace Elide.TextWorkbench.Configuration
{
    [DependsFrom(typeof(IConfigService))]
    public sealed class TextConfigService : AbstractTextConfigService
    {
        public TextConfigService()
        {
            
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            var con = App.Config<TextEditorsConfig>();
            Configs
                .Where(c => !con.Configs.ContainsKey(c.Key))
                .ForEach(c => con.Configs.Add(c.Key,
                    new TextConfig
                    {
                        UseTabs = c.UseTabs,
                        IndentMode = c.IndentMode,
                        TabSize = c.TabSize,
                        IndentSize = c.IndentSize
                    }));
        }

        public override TextConfig GetConfig(string key)
        {
            var inf = GetInfo("textConfigs", key);
            var con = default(TextConfig);

            if (inf != null)
                con = FindConfig(key);

            return con;
        }

        private TextConfig FindConfig(string key)
        {
            var con = App.Config<TextEditorsConfig>();
            var ret = default(TextConfig);
            con.Configs.TryGetValue(key, out ret);
            return ret;
        }
    }
}
