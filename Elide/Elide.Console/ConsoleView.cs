using System;
using System.Drawing;
using System.IO;
using Elide.CodeEditor.Views;
using Elide.Console.Configuration;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Views;

namespace Elide.Console
{
    public sealed class ConsoleView : AbstractView
	{
		private TextWriter conOut;
        private TextReader conIn;

		public ConsoleView()
		{
			
		}

        private void InitializeControl()
        {
            _control = new ConsoleControl();
            _control.App = App;
            _control.Cout.GetScintilla().ReadOnly = true;

            var srv = App.GetService<IStylesConfigService>();
            srv.EnumerateStyleItems("Console").UpdateStyles(_control.Cout.GetScintilla());
            UpdateConsoleConfig(App.Config<ConsoleConfig>());

            App.GetService<IConfigService>().ConfigUpdated += new EventHandler<ConfigEventArg>(ConfigUpdated);
        }

        public override void Activate()
        {
            ConControl.Cout.GetScintilla().Select();
        }

        private void ConfigUpdated(object sender, ConfigEventArg e)
        {            
            if (e.Config is StylesConfig)
                ((StylesConfig)e.Config).Styles["Console"].UpdateStyles(ConControl.Cout.GetScintilla());
            else if (e.Config is ConsoleConfig)
                UpdateConsoleConfig((ConsoleConfig)e.Config);
        }

        private void UpdateConsoleConfig(ConsoleConfig cfg)
        {
            ConControl.Cout.Styling = cfg.Styling;
            ConControl.Cout.HistorySize = cfg.HistorySize;
            ConControl.Cout.CaretStyle = cfg.CaretStyle;
            ConControl.Cout.CaretWidth = cfg.CaretWidth;
            ConControl.Cout.GetScintilla().SelectionAlpha = cfg.SelectionTransparency;
            ConControl.Cout.GetScintilla().UseSelectionColor = cfg.UseSelectionColor;
            ConControl.Cout.GetScintilla().MainSelectionForeColor = Color.FromKnownColor(cfg.SelectionForeColor);
            ConControl.Cout.GetScintilla().MainSelectionBackColor = Color.FromKnownColor(cfg.SelectionBackColor);
        }

        private DateTime sessionStart;
        public void StartSession(ConsoleSessionInfo sessionInfo)
        {
            ConControl.Invoke(() =>
                {
                    ConControl.Cout.Clear();
                    ConControl.Cout.GetScintilla().ReadOnly = false;
                    conOut = System.Console.Out;
                    conIn = System.Console.In;
                    System.Console.SetOut(new ConsoleTextWriter(ConControl.Cout));
                    System.Console.SetIn(new ConsoleTextReader(ConControl.Cout));
                    ConControl.Cout.PrintHeader(sessionInfo.Banner);
                });
            sessionStart = DateTime.Now;
            OnContentChanged(new ViewEventArgs(true, sessionInfo.SessionName));
        }
		        
		public void EndSession()
		{
            ConControl.Invoke(() =>
                {
                    ConControl.Cout.PrintLine();
                    ConControl.Cout.PrintLine(String.Format("Session ended. Session taken: {0}", DateTime.Now - sessionStart));
                    ConControl.Cout.GetScintilla().ReadOnly = true;
                    System.Console.Out.Dispose();
                    System.Console.In.Dispose();
                    System.Console.SetOut(conOut);
                    System.Console.SetIn(conIn);
                });
            OnContentChanged(ViewEventArgs.None);
		}

        private ConsoleControl _control;
        public override object Control
        {
            get
            {
                if (_control == null)
                    InitializeControl();

                return _control;
            }
        }

        internal ConsoleControl ConControl
        {
            get { return (ConsoleControl)Control; }
        }
	}
}
