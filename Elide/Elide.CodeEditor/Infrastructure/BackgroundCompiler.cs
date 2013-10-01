using System;
using System.Collections.Generic;
using System.Threading;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Scintilla;

namespace Elide.CodeEditor.Infrastructure
{
    internal sealed class BackgroundCompiler
    {
        private readonly IApp app;
        private readonly ScintillaControl sci;
        private readonly IBackgroundCompiler compiler;
        private readonly BackgroundCompilerService service;
        private Thread thread;

        internal BackgroundCompiler(IApp app, ScintillaControl sci, IBackgroundCompiler compiler, BackgroundCompilerService service)
        {
            this.app = app;
            this.sci = sci;
            this.compiler = compiler;
            this.service = service;
        }

        internal void Compile(CodeDocument doc)
        {
            if (thread != null)
                return;

            thread = new Thread(InternalCompile);
            thread.IsBackground = true;
            thread.Start(doc);
        }

        internal void CompileAlways(CodeDocument doc)
        {
            var th = new Thread(InternalCompile);
            th.IsBackground = true;
            th.Start(doc);
        }

        internal void Abort()
        {
            var t = thread;

            if (t != null)
            {
                try
                {
                    t.Abort();
                }
                catch { }
            }
        }

        private void InternalCompile(object obj)
        {
            var doc = (CodeDocument)obj;
            var src = String.Empty;

            sci.Invoke(() =>
            {
                using (var sciTemp = new BasicScintillaControl())
                {
                    sciTemp.AttachDocument(doc.GetSciDocument());
                    src = sciTemp.GetTextUnicode();
                }
            });

            if (!String.IsNullOrWhiteSpace(src))
            {
                var ret = default(Tuple<ICompiledUnit, IEnumerable<MessageItem>>);

                try
                {
                    ret = compiler.Compile(doc, src);
                }
                catch { }

                if (ret != null)
                {
                    doc.Unit = ret.Item1;

                    if (service.HighlightErrors)
                    {
                        doc.Messages = ret.Item2;
                        sci.Invoke(() => app.GetService<IOutlinerService>().Outline(doc));
                    }
                    else
                        doc.Messages = null;

                    doc.UnitVersion = doc.Version;
                }
            }

            Thread.Sleep(1000);
            thread = null;
        }
    }
}
