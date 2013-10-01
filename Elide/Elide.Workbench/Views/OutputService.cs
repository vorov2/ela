using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Core;
using Elide.Environment.Views;
using Elide.Scintilla;

namespace Elide.Workbench.Views
{
    public sealed class OutputService : Service, IOutputService
    {
        public OutputService()
        {

        }

        public void WriteLine()
        {
            WriteLine(OutputFormat.None, String.Empty, null);
        }

        public void WriteLine(string text, params object[] args)
        {
            WriteLine(OutputFormat.None, text, args);
        }

        public void Write(string text, params object[] args)
        {
            Write(OutputFormat.None, text, args);
        }

        public void WriteLine(OutputFormat format, string text, params object[] args)
        {
            Write(format, text + System.Environment.NewLine, args);
        }

        public void Write(OutputFormat format, string text, params object[] args)
        {
            var str =
                format == OutputFormat.Error ? "<!e!>" + text + "</!e!>" :
                format == OutputFormat.Important ? "<!i!>" + text + "</!i!>" :
                format == OutputFormat.Header ? "<!h!>" + text + "</!h!>" :
                format == OutputFormat.Warning ? "<!w!>" + text + "</!w!>" :
                text;
            View().WriteString(str, args);
        }

        public void Clear()
        {
            View().Clear();
        }

        private OutputView View()
        {
            return (OutputView)App.GetService<IViewService>().GetView("Output");
        }
    }
}
