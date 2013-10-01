using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using Elide.CodeEditor;
using Elide.Core;
using Elide.Environment;
using Elide.TextEditor;

namespace Elide.CodeWorkbench
{
    public sealed class SymbolSearchService : Service, ISymbolSearchService
    {
        public SymbolSearchService()
        {

        }

        public void SearchSymbol(string symbol, ISymbolFinder finder)
        {
            var dlg = new FindSymbolDialog();

            if (!String.IsNullOrWhiteSpace(symbol))
                dlg.Symbol = symbol;

            var sbs = App.GetService<IStatusBarService>();
            sbs.ClearStatusString();

            if (dlg.ShowDialog((IWin32Window)App.GetService<IEnvironmentService>().GetMainWindow()) == DialogResult.OK)
            {
                var sw = new Stopwatch();
                sw.Start();
                var res = finder.FindSymbols(dlg.Symbol, dlg.AllFiles).ToList();
                sw.Stop();
                var s = (Single)sw.ElapsedMilliseconds / 1000;
                                
                sbs.SetStatusString(StatusType.Information, "{0} symbol(s) found. Search took: {1:F2} s.", res.Count(), s);
                App.GetService<IResultGridService>().AddItems(
                    res.Select(r =>
                    {
                        var editor = App.Editor(r.Document.GetType());
                        var txt = ((ITextEditor)editor).GetContent(r.Document, r.Line).TrimStart(' ', '\t');
                        return new ResultItem(txt, r.Document, r.Line, r.Column, dlg.Symbol.Length);
                    }));
                App.OpenView("ResultGrid");
            }
        }
    }
}
