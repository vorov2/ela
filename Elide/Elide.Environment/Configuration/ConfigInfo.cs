using System;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public sealed class ConfigInfo : ExtInfo
    {
        public ConfigInfo(string key, Type configType, string displayName, string category, int position, Type widget) : base(key)
        {
            Type = configType;
            DisplayName = displayName;
            Category = category;
            Position = position;
            Widget = widget;
        }

        public Type Type { get; private set; }

        public string DisplayName { get; private set; }

        public string Category { get; private set; }

        public int Position { get; private set; }

        public Type Widget { get; private set; }
    }
}
