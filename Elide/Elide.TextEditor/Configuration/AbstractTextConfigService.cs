using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;

namespace Elide.TextEditor.Configuration
{
    public abstract class AbstractTextConfigService : Service, ITextConfigService
    {
        protected AbstractTextConfigService()
        {

        }

        public abstract TextConfig GetConfig(string key);

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new TextConfigReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Configs.OfType<ExtInfo>().ToList();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Configs.FirstOrDefault(c => c.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "textConfigs")
                throw new ElideException("Section '{0}' is not supported by TextConfigService.", section);
        }

        protected internal IEnumerable<TextConfigInfo> Configs { get; internal set; }
    }
}
