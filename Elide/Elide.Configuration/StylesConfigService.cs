using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Forms;

namespace Elide.Configuration
{
    [DependsFrom(typeof(IConfigService))]
    public class StylesConfigService : AbstractStylesConfigService
    {
        public StylesConfigService()
        {
            
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            var _ = FontHelper.MonospaceFonts;

            var con = app.Config<StylesConfig>();
            Groups
                .Where(c => !con.Styles.ContainsKey(c.Key))
                .ForEach(c => con.Styles.Add(c.Key, c.StyleItems.ToList()));
        }

        protected override IEnumerable<StyleGroupInfo> EnumerateGroups()
        {
            return Groups.ToList();
        }

        protected override IEnumerable<StyleItemConfig> FindStyleItems(string groupKey)
        {
            return Config().Styles[groupKey];
        }

        private StylesConfig Config()
        {
            return App.Config<StylesConfig>();
        }
    }
}
