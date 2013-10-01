using System;
using System.Collections.Generic;

namespace Elide.Environment.Configuration
{
    [Serializable]
    public sealed class StylesConfig : Config
    {
        public StylesConfig()
        {
            Styles = new Dictionary<String,List<StyleItemConfig>>();
        }

        public override Config Clone()
        {
            var ret = new StylesConfig();

            if (Styles != null)
            {
                ret.Styles = new Dictionary<String,List<StyleItemConfig>>(Styles);

                Styles.ForEach(kv => {
                    var list = new List<StyleItemConfig>();
                    kv.Value.ForEach(v => list.Add((StyleItemConfig)v.Clone()));
                    ret.Styles[kv.Key] = list;
                });
            }

            return ret;
        }

        public Dictionary<String,List<StyleItemConfig>> Styles { get; set; }
    }
}
