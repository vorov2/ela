using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.Console;
using Elide.CodeEditor.Views;
using Ela.Runtime;
using Ela.Linking;
using Elide.Environment.Configuration;
using Elide.Console.Configuration;
using System.Drawing;
using Elide.ElaCode.Configuration;
using System.Threading;

namespace Elide.ElaCode.Views
{
    public sealed class InteractiveView : AbstractView
    {
        private ElaMachine vm;
        private ElaIncrementalLinker link;
        private int lastOffset;

        public InteractiveView()
        {

        }

        private void InitializeControl()
        {
            _control = new InteractiveControl();
            _control.App = App;
                        
            Con.Styling = true;
            Con.HistorySize = 20;
            Con.Header = "Ela Interactive Console";
            Con.Prompt = "``ela";
            Con.ClearAll();
            Con.Submit += Submit;
            Con.SessionReset += SessionReset;

            var srv = App.GetService<IStylesConfigService>();
            srv.EnumerateStyleItems("Interactive").UpdateStyles(Con.GetScintilla());
            UpdateConsoleConfig(App.Config<InteractiveConfig>());

            App.GetService<IConfigService>().ConfigUpdated += new EventHandler<ConfigEventArg>(ConfigUpdated);

        }

        private void SessionReset(object sender, EventArgs e)
        {
            link = null;
            vm.Dispose();
            vm = null;
            lastOffset = 0;
        }

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {
            if (e.Config is StylesConfig)
                ((StylesConfig)e.Config).Styles["Interactive"].UpdateStyles(Con.GetScintilla());
            else if (e.Config is InteractiveConfig)
                UpdateConsoleConfig((InteractiveConfig)e.Config);
        }

        private void UpdateConsoleConfig(InteractiveConfig cfg)
        {
            ConControl.Cout.CaretStyle = cfg.CaretStyle;
            ConControl.Cout.CaretWidth = cfg.CaretWidth;
            ConControl.Cout.GetScintilla().SelectionAlpha = cfg.SelectionTransparency;
            ConControl.Cout.GetScintilla().UseSelectionColor = cfg.UseSelectionColor;
            ConControl.Cout.GetScintilla().MainSelectionForeColor = Color.FromKnownColor(cfg.SelectionForeColor);
            ConControl.Cout.GetScintilla().MainSelectionBackColor = Color.FromKnownColor(cfg.SelectionBackColor);
        }

        private void Submit(object sender, SubmitEventArgs e)
        {
            if (link == null)
            {
                var bo = new BuildOptionsManager(App);
                link = new ElaIncrementalLinker(bo.CreateLinkerOptions(),
                    bo.CreateCompilerOptions());
            }

            link.SetSource(e.Text.Trim('\0'));
            var lr = link.Build();

            foreach (var m in lr.Messages)
            {
                var tag = m.Type == Ela.MessageType.Error ? "``!!!" :
                    m.Type == Ela.MessageType.Warning ? "``|||" :
                    "``???";
                Con.PrintLine(String.Format("{0}{1} ({2},{3}): {4} ELA{5}: {6}", tag, m.File.Name, m.Line, m.Column, m.Type, m.Code, m.Message));
            }

            if (lr.Success)
            {
                var th = new Thread(() => ExecuteInput(lr));
                Con.SetExecControl(th);
                App.GetService<IStatusBarService>().SetStatusString(StatusType.Information, "Executing code from Interactive...");
                th.Start();
            }
            else
            {
                Con.PrintLine();
                Con.Print(Con.Prompt + ">");
                Con.ScrollToCaret();
            }
        }

        private void ExecuteInput(LinkerResult lr)
        {
            if (vm == null)
                vm = new ElaMachine(lr.Assembly);
            else
            {
                vm.RefreshState();
                vm.Recover();
            }

            var mod = lr.Assembly.GetRootModule();
            var os = lastOffset;
            lastOffset = mod.Ops.Count;

            try
            {
                Con.Invoke(() => { Con.ReadOnly = true; });
                var exer = vm.Run(os);

                if (exer.ReturnValue.TypeCode != Ela.ElaTypeCode.Unit)
                    Con.Invoke(() => Con.PrintLine(exer.ReturnValue.ToString()));
            }
            catch (ElaCodeException ex)
            {
                Con.Invoke(() => Con.PrintLine("``!!!ELA" + ex.Error.Code + ": " + ex.Error.Message));
            }
            catch (Exception) { }
            finally
            {
                Con.Invoke(() => { Con.ReadOnly = false; });
                Con.PrintLine();
                Con.PrintLine();
                Con.Print(Con.Prompt + ">");
                Con.ScrollToCaret();
                App.GetService<IStatusBarService>().ClearStatusString();
            }
        }

        public override void Activate()
        {
            ConControl.Cout.Activate();
        }

        private InteractiveControl _control;
        public override object Control
        {
            get
            {
                if (_control == null)
                    InitializeControl();

                return _control;
            }
        }

        internal InteractiveControl ConControl
        {
            get { return (InteractiveControl)Control; }
        }

        internal InteractiveTextBox Con
        {
            get { return ConControl.Cout; }
        }
    }
}
