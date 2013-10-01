using System;
using System.Threading;
using Elide.CodeEditor.Infrastructure;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.Scintilla;
using Elide.TextEditor;
using System.Threading.Tasks;

namespace Elide.CodeWorkbench
{
    public sealed class CodeRunnerService : AbstractCodeRunnerService
    {
        private Action abortCallBack;

        public CodeRunnerService()
        {

        }

        protected override bool Run<T>(T compiled, ExecOptions options, ICodeRunner<T> runner, params ExtendedOption[] extOptions)
        {
            return Execute(compiled, options, runner, extOptions);
        }

        public override bool IsRunning()
        {
            return abortCallBack != null;
        }

        public override bool AbortExecution()
        {
            var f = abortCallBack;

            if (f != null)
            {
                try
                {
                    f();
                    return true;
                }
                catch
                {
                    return false;
                }
                finally
                {
                    abortCallBack = null;
                }
            }

            return false;
        }

        internal bool Execute<T>(T compiled, ExecOptions options, ICodeRunner<T> runner, params ExtendedOption[] extOptions) where T : ICompiledAssembly
        {
            var output = App.GetService<IOutputService>();
            var con = App.GetService<IConsoleService>();
            var sci = App.Editor().Control as ScintillaControl;

            if (options.Set(ExecOptions.Console))
            {
                App.OpenView("Console");                
                var sess = new ConsoleSessionInfo
                    {
                        SessionName = compiled.MainUnit.Name,
                        Banner = String.Format("Running module {0}", compiled.MainUnit.Name)
                    };
                con.StartSession(sess);
            }

            var ah = new ManualResetEvent(false);
            var success = false;
            var th = new Thread(() =>
            {
                var res = default(Object);

                try
                {
                    res = runner.Execute(compiled, extOptions);
                    success = true;
                    ah.Set();
                    WriteExecutionResult(sci, options, res);                    
                }
                catch (CodeException ex)
                {
                    ah.Set();
                    output.WriteLine(OutputFormat.Header, "Unhandled run-time error:");
                    output.WriteLine(OutputFormat.Error, ex.ToString());
                    var nav = App.GetService<IDocumentNavigatorService>();

                    if (options.Set(ExecOptions.TipError) && sci != null)
                        sci.ShowCallTip(ex.Message);

                    if (options.Set(ExecOptions.Annotation) && sci != null && ex.Document != null && nav.SetActive(ex.Document))
                    {
                        sci.SetAnnotation(ex.Line - 1, ex.Message, TextStyle.Annotation1);
                        sci.CaretPosition = sci.GetPositionFromLine(ex.Line);
                        sci.ScrollToCaret();
                    }

                    if (options.Set(ExecOptions.Console) && con != null)
                        con.EndSession("Session terminated because of an unhandled run-time error.");

                    if (options.Set(ExecOptions.ShowOutput))
                        App.OpenView("Output");

                    return;
                }
                finally
                {
                    abortCallBack = null;                    
                }
                
                if (options.Set(ExecOptions.Console))
                    con.EndSession(res);
            });

            abortCallBack = () =>
            {
                ah.Set();
                th.Abort();

                if (options.Set(ExecOptions.Console) && con != null)
                    con.EndSession("Session terminated.");
            };
           
            if (options.Set(ExecOptions.LimitTime))
            {
                try
                {
                    th.Start();

                    if (!ah.WaitOne(500))
                    {
                        var s = success;

                        if (!s)
                            try
                            {
                                th.Abort();
                                abortCallBack = null;
                            }
                            catch { }

                        return s;
                    }

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                th.Start();
                return true;
            }
        }

        private void WriteExecutionResult(ScintillaControl sci, ExecOptions options, object data)
        {
            if (options.Set(ExecOptions.PrintResult))
            {
                var outp = App.GetService<IOutputService>();
                outp.WriteLine(OutputFormat.Header, "Execution result:");
                outp.WriteLine((data ?? "[unit]").ToString());
            }

            if (options.Set(ExecOptions.TipResult) && sci != null)
                sci.ShowCallTip(sci.GetSelection().CaretPosition, (data ?? "[unit]").ToString());
        }        
    }
}
