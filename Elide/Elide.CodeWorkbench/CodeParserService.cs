using System;
using Elide.CodeEditor.Infrastructure;
using Elide.Environment;
using Elide.Scintilla;

namespace Elide.CodeWorkbench
{
    public sealed class CodeParserService : AbstractCodeParserService
    {
        public CodeParserService()
        {

        }

        protected override T RunParser<T>(string source, Document doc, ICodeParser<T> parser)
        {
            source = (source ?? String.Empty).TrimEnd('\0');
            var sci = App.Editor().Control as ScintillaControl;
            var logger = new BuildLogger(App, doc, sci, new BuildOptions());
            var res = parser.Parse(source, doc, logger);
            return res;
        }
    }
}
