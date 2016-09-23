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
            var seq = Config().Styles[groupKey].ToList();
            var group = Groups.FirstOrDefault(g => g.Key == groupKey);

            if (group != null)
            {
                var missings = group.StyleItems.Except(seq);

                if (missings.Any())
                    seq.AddRange(missings);
            }

            return seq;
        }

        private StylesConfig Config()
        {
            return App.Config<StylesConfig>();
        }
    }
}
