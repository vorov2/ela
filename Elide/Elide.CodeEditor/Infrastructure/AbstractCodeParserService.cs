using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public abstract class AbstractCodeParserService : Service, ICodeParserService
    {
        protected AbstractCodeParserService()
        {
            BuilderInstances = new Dictionary<Type,Object>();
        }

        public virtual T RunParser<T>(string source, Document doc) where T : IAst
        {
            var type = typeof(T);
            var parserObj = default(Object);

            if (!BuilderInstances.TryGetValue(type, out parserObj))
                throw new ElideException("Unable to find parser for '{0}'.", type);

            var parser = (ICodeParser<T>)parserObj;
            parser.App = App;
            return RunParser(source, doc, parser);
        }

        protected abstract T RunParser<T>(string source, Document doc, ICodeParser<T> parser) where T : IAst;

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Parsers.ForEach(b =>
            {
                var iface = default(Type);

                if ((iface = b.Type.GetInterface(typeof(ICodeParser<>).FullName)) == null)
                    throw new ElideException("Parser '{0}' doesn't implement ICodeParser<> interface.", b.Type);

                var arg = iface.GetGenericArguments()[0];
                BuilderInstances.Add(arg, TypeCreator.New(b.Type));
            });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new CodeParserReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Parsers.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Parsers.FirstOrDefault(b => b.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "parsers")
                throw new ElideException("Section '{0}' is not supported by CodeParserService.", section);
        }

        protected internal IEnumerable<CodeParserInfo> Parsers { get; internal set; }

        protected Dictionary<Type,Object> BuilderInstances { get; private set; }
    }
}
