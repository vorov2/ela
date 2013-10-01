using System;
using Elide.Core;

namespace Elide.Environment.Views
{
    public sealed class ViewInfo : ExtInfo
    {
        public ViewInfo(string key, string title, string shortcut, ViewType viewType, Type type) : base(key)
        {
            Title = title;
            Shortcut = shortcut;
            ViewType = viewType;
            Type = type;
        }

        public string Title { get; private set; }

        public string Shortcut { get; private set; }

        public ViewType ViewType { get; private set; }

        public Type Type { get; private set; }
    }
}
