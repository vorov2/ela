using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    public abstract class AbstractTaskListService : Service, ITaskListService
    {
        protected AbstractTaskListService()
        {
            ProviderInstances = new Dictionary<String,ITaskProvider>();
        }

        protected ITaskProvider GetTaskProvider(string editorKey)
        {
            var ret = default(ITaskProvider);
            ProviderInstances.TryGetValue(editorKey, out ret);

            if (ret != null)
                ret.App = App;

            return ret;
        }

        protected ITaskProvider GetTaskProvider(CodeDocument doc)
        {
            if (doc != null)
                return GetTaskProvider(doc.CodeEditor.Key);

            return null;
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Providers.ForEach(b =>
            {
                var iface = default(Type);

                if ((iface = b.Type.GetInterface(typeof(ITaskProvider).FullName)) == null)
                    throw new ElideException("Task provider '{0}' doesn't implement ITaskProvider interface.", b.Type);

                ProviderInstances.Add(b.EditorKey, TypeCreator.New<ITaskProvider>(b.Type));
            });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new TaskProviderReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Providers.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Providers.FirstOrDefault(b => b.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "taskProviders")
                throw new ElideException("Section '{0}' is not supported by TaskProviderService.", section);
        }

        protected internal IEnumerable<TaskProviderInfo> Providers { get; internal set; }

        protected Dictionary<String,ITaskProvider> ProviderInstances { get; private set; }
    }
}
