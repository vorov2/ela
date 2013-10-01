using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment;

namespace Elide.CodeEditor.Infrastructure
{
    public class CodeLexerService : Service, ICodeLexerService
    {
        public CodeLexerService()
        {
            LexerInstances = new Dictionary<String,ICodeLexer>();
        }

        public virtual ICodeLexer GetLexer(string editorKey)
        {
            ICodeLexer lexer;
            LexerInstances.TryGetValue(editorKey, out lexer);
            return lexer;
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Lexers.ForEach(b =>
            {
                var iface = default(Type);

                if ((iface = b.Type.GetInterface(typeof(ICodeLexer).FullName)) == null)
                    throw new ElideException("ICodeLexer '{0}' doesn't implement ICodeLexer interface.", b.Type);

                LexerInstances.Add(b.Key, TypeCreator.New<ICodeLexer>(b.Type));
            });
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new CodeLexerReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Lexers.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Lexers.FirstOrDefault(b => b.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "lexers")
                throw new ElideException("Section '{0}' is not supported by LexerService.", section);
        }

        protected internal IEnumerable<CodeLexerInfo> Lexers { get; internal set; }

        protected Dictionary<String,ICodeLexer> LexerInstances { get; private set; }
    }
}
