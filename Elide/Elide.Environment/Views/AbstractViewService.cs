using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.Environment.Views
{
    public abstract class AbstractViewService : Service, IViewService
    {
        protected AbstractViewService()
        {

        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new ViewReader(this);
        }

        public abstract bool IsViewActive(string key);

        public abstract IView GetView(string key);

        public abstract bool OpenView(string key);

        public abstract bool CloseView(string key);

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Views.OfType<ExtInfo>().ToList();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Views.FirstOrDefault(v => v.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "views")
                throw new ElideException("Section '{0}' is not supported by ViewService.", section);
        }

        protected internal IEnumerable<ViewInfo> Views { get; internal set; }
    }
}
