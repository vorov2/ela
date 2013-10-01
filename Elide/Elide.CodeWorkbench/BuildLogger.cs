using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.CodeEditor.Infrastructure;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.Scintilla;

namespace Elide.CodeWorkbench
{
    internal sealed class BuildLogger : IBuildLogger
    {
        private readonly IApp app;
        private readonly Document doc;
        private readonly ScintillaControl sci;
        private readonly BuildOptions options;
        private readonly IErrorListService err;
        private readonly IOutputService output;
        private bool firstWrite = true;

        internal BuildLogger(IApp app, Document doc, ScintillaControl sci, BuildOptions options)
        {
            this.app = app;
            this.doc = doc;
            this.sci = sci;
            this.options = options;

            app.CloseView("ErrorList");
            this.err = app.GetService<IErrorListService>();
            this.output = app.GetService<IOutputService>();
        }

        public void WriteBuildInfo(string name, Version version)
        {
            if (options.Set(BuildOptions.ErrorList))
                err.Clear();

            if (options.Set(BuildOptions.Output))
            {
                output.Clear();
                output.WriteLine(OutputFormat.Header, "Build started. File: {0}", doc.Title);
                output.WriteLine(OutputFormat.Important, "Using {0} version {1}.", name, version);
            }
        }

        public void WriteBuildOptions(string format, params object[] args)
        {
            if (options.Set(BuildOptions.Output))
            {
                if (firstWrite)
                {
                    output.WriteLine();
                    firstWrite = false;
                }
                
                output.WriteLine(format, args);
            }
        }

        public void WriteMessages(IEnumerable<MessageItem> messages, Func<MessageItem,Boolean> verboseFilter)
        {
            var success = !messages.Any(m => m.Type == MessageItemType.Error);

            if (options.Set(BuildOptions.Output))
            {
                output.WriteLine();
                output.WriteLine(OutputFormat.Important, "Build complete: {0} errors, {1} warnings",
                    messages.Count(m => m.Type == MessageItemType.Error),
                    messages.Count(m => m.Type == MessageItemType.Warning));
                messages.ForEach(m => output.WriteLine(
                        m.Type == MessageItemType.Error ? OutputFormat.Error :
                        m.Type == MessageItemType.Warning ? OutputFormat.Warning :
                        OutputFormat.None,
                    PrintMessageFull(m)));
                output.WriteLine();
            }

            if (messages.Count() > 0 && options.Set(BuildOptions.ErrorList))
            {
                err.ShowMessages(messages);

                if (!success)
                    app.OpenView("ErrorList");
            }

            if (!success && options.Set(BuildOptions.TipError))
            {
                var sb = new StringBuilder();
                messages
                    .Where(verboseFilter)
                    .ForEachIndex((m, i) => sb.Append((i != 0 ? "\n" : "") + PrintMessageShort(m))); //use "short" custom print for msgs
                sci.ShowCallTip(sb.ToString());
            }
        }
        
        private string PrintMessageFull(MessageItem msg)
        {
            return String.Format("{4}({0},{1}) {2} {3}", msg.Line, msg.Column, msg.Type, msg.Message, msg.Document.Title);
        }
        
        private string PrintMessageShort(MessageItem msg)
        {
            return String.Format("({0},{1}) {2} {3}", msg.Line, msg.Column, msg.Type, msg.Message);
        }
    }
}
