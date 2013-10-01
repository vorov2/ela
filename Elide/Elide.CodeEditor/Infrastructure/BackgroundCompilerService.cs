using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Editors;
using Elide.Main;
using Elide.Scintilla;

namespace Elide.CodeEditor.Infrastructure
{
    [DependsFrom(typeof(IDaemonService))]
    [DependsFrom(typeof(IEditorService))]
    public class BackgroundCompilerService : Service, IBackgroundCompilerService, IDaemon
    {
        public BackgroundCompilerService()
        {
            CompilerInstances = new Dictionary<String,BackgroundCompiler>();
            EnableBackgroundCompilation = true;
            HighlightErrors = true;
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);

            Compilers.ForEach(b =>
            {
                if (b.Type.GetInterface(typeof(IBackgroundCompiler).Name) == null)
                    throw new ElideException("Compiler '{0}' doesn't implement IBackgroundCompiler interface.", b.Type);

                var bc = TypeCreator.New<IBackgroundCompiler>(b.Type);
                var inf = (EditorInfo)app.GetService<IEditorService>().GetInfo("editors", b.EditorKey);
                var ins = inf.Instance as ICodeEditor;

                if (ins == null)
                    throw new ElideException("Compiler '{0}' can be only registered for the code editor.", b.Type);

                var sci = ins.Control as ScintillaControl;
                var wrap = new BackgroundCompiler(app, sci, bc, this);
                CompilerInstances.Add(b.EditorKey, wrap);
            });

            app.GetService<IDaemonService>().RegisterDaemon(this);
            app.GetService<IDocumentService>().DocumentAdded += (_,e) => DocumentOpened(e.Document);
        }

        public override void Unload()
        {
            CompilerInstances.Values.ForEach(c => c.Abort());
        }

        private void DocumentOpened(Document doc)
        {
            var inf = App.EditorInfo(doc);
            var wrap = default(BackgroundCompiler);

            if (inf.Instance is ICodeEditor && CompilerInstances.TryGetValue(inf.Key, out wrap) && EnableBackgroundCompilation)
                wrap.CompileAlways((CodeDocument)doc);
        }

        public void Execute()
        {
            var doc = App.Document() as CodeDocument;
            var wrap = default(BackgroundCompiler);

            if (doc != null && doc.UnitVersion != doc.Version && CompilerInstances.TryGetValue(doc.CodeEditor.Key, out wrap) && EnableBackgroundCompilation)
               wrap.Compile(doc);
        }

        public IExtReader CreateExtReader(string section)
        {
            ValidateSection(section);
            return new BackgroundCompilerReader(this);
        }

        public IEnumerable<ExtInfo> EnumerateInfos(string section)
        {
            ValidateSection(section);
            return Compilers.OfType<ExtInfo>().ToArray();
        }

        public ExtInfo GetInfo(string section, string key)
        {
            ValidateSection(section);
            return Compilers.FirstOrDefault(c => c.Key == key);
        }

        private void ValidateSection(string section)
        {
            if (section != "compilers")
                throw new ElideException("Section '{0}' is not supported by BackgroundCompilerService.", section);
        }

        protected internal IEnumerable<BackgroundCompilerInfo> Compilers { get; internal set; }

        internal Dictionary<String,BackgroundCompiler> CompilerInstances { get; private set; }

        public bool EnableBackgroundCompilation { get; set; }

        public bool HighlightErrors { get; set; }
    }
}
