using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;

namespace Elide.CodeEditor.Infrastructure
{
    public class ReferenceResolverService : Service, IReferenceResolverService
    {
        public ReferenceResolverService()
        {
            ResolverInstances = new Dictionary<Type,Object>();
        }

        public virtual ICompiledUnit Resolve<T>(T reference, params ExtendedOption[] options) where T : IReference
        {
            var type = typeof(T);
            var resolverObj = default(Object);

            if (type == typeof(IReference))
                type = reference.GetType();
            
            if (!ResolverInstances.TryGetValue(type, out resolverObj))
                throw new ElideException("Unable to find resolver for '{0}'.", type);

            var resolver = (IReferenceResolver<T>)resolverObj;
            resolver.App = App;
            return resolver.Resolve(reference, options);
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Resolvers.ForEach(r =>
            {
                var iface = default(Type);

                if ((iface = r.Type.GetInterface(typeof(IReferenceResolver<>).FullName)) == null)
                    throw new ElideException("Resolver '{0}' doesn't implement IReferenceResolver<> interface.", r.Type);

                var arg = iface.GetGenericArguments()[0];
                ResolverInstances.Add(arg, TypeCreator.New(r.Type));
            });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new ReferenceResolverReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Resolvers.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Resolvers.FirstOrDefault(r => r.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "resolvers")
                throw new ElideException("Section '{0}' is not supported by ReferenceResolverService.", section);
        }

        protected internal IEnumerable<ResolverInfo> Resolvers { get; internal set; }

        protected Dictionary<Type,Object> ResolverInstances { get; private set; }
    }
}
