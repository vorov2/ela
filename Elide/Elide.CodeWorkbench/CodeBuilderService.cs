using System;
using Elide.CodeEditor.Infrastructure;
using Elide.Environment;
using Elide.Scintilla;

namespace Elide.CodeWorkbench
{
    public sealed class CodeBuilderService : AbstractCodeBuilderService
    {
        public CodeBuilderService()
        {

        }

        protected override T RunBuilder<T>(string source, Document doc, BuildOptions options, ICodeBuilder<T> builder, params ExtendedOption[] extOptions)
        {
            source = (source ?? String.Empty).TrimEnd('\0');
            var sci = App.Editor().Control as ScintillaControl;
            var logger = new BuildLogger(App, doc, sci, options);
            var res = builder.Build(source, doc, logger, extOptions);
            return res;
        }
    }
}
