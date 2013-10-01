using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public abstract class AbstractCodeBuilderService : Service, ICodeBuilderService
    {
        protected AbstractCodeBuilderService()
        {
            BuilderInstances = new Dictionary<Type,Object>();
        }

        public virtual T RunBuilder<T>(string source, Document doc, BuildOptions options, params ExtendedOption[] extOptions) where T : ICompiledAssembly
        {
            var type = typeof(T);
            var builderObj = default(Object);

            if (!BuilderInstances.TryGetValue(type, out builderObj))
                throw new ElideException("Unable to find builder for '{0}'.", type);

            var builder = (ICodeBuilder<T>)builderObj;
            builder.App = App;
            return RunBuilder(source, doc, options, builder, extOptions);
        }

        protected abstract T RunBuilder<T>(string source, Document doc, BuildOptions options, ICodeBuilder<T> builder, params ExtendedOption[] extOptions) where T : ICompiledAssembly;

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Builders.ForEach(b =>
                {
                    var iface = default(Type);

                    if ((iface = b.Type.GetInterface(typeof(ICodeBuilder<>).FullName)) == null)
                        throw new ElideException("Builder '{0}' doesn't implement ICodeBuilder<> interface.", b.Type);

                    var arg = iface.GetGenericArguments()[0];
                    BuilderInstances.Add(arg, TypeCreator.New(b.Type));
                });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new CodeBuilderReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Builders.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Builders.FirstOrDefault(b => b.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "builders")
                throw new ElideException("Section '{0}' is not supported by CodeBuilderService.", section);
        }

        protected internal IEnumerable<CodeBuilderInfo> Builders { get; internal set; }

        protected Dictionary<Type,Object> BuilderInstances { get; private set; }
    }
}
