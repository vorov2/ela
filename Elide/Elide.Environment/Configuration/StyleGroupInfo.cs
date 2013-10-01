using System;
using Elide.Core;
using System.Collections.Generic;

namespace Elide.Environment.Configuration
{
    public sealed class StyleGroupInfo : ExtInfo
    {
        public StyleGroupInfo(string key, string displayName, IEnumerable<StyleItemConfig> items) : base(key)
        {
            DisplayName = displayName;
            StyleItems = items;
        }

        public override string ToString()
        {
            return DisplayName;
        }

        public string DisplayName { get; private set; }

        public IEnumerable<StyleItemConfig> StyleItems { get; private set; } 
    }
}
