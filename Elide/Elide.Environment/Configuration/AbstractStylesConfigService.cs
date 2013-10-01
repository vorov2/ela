using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Configuration
{
    public abstract class AbstractStylesConfigService : Service, IStylesConfigService
    {
        protected AbstractStylesConfigService()
        {

        }

        public virtual IEnumerable<StyleItemConfig> EnumerateStyleItems(string styleGroupKey)
        {
            var group = GetInfo("styles", styleGroupKey) as StyleGroupInfo;

            if (group == null)
                throw new ElideException("Style group '{0}' not found.", styleGroupKey);

            return FindStyleItems(styleGroupKey);
        }

        protected abstract IEnumerable<StyleItemConfig> FindStyleItems(string groupKey);

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new StyleConfigReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return EnumerateGroups().OfType<ExtInfo>();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return EnumerateGroups().FirstOrDefault(c => c.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "styles")
                throw new ElideException("StylesConfigService doesn't support '{0}' section.", section);
        }

        protected internal IEnumerable<StyleGroupInfo> Groups { get; internal set; }

        protected abstract IEnumerable<StyleGroupInfo> EnumerateGroups();
    }
}
