using System;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment.Views;

namespace Elide.Console
{
    public sealed class ConsoleService : Service, IConsoleService
    {
        public ConsoleService()
        {

        }

        public void StartSession(ConsoleSessionInfo sessionInfo)
        {
            View().StartSession(sessionInfo);
        }

        public void EndSession(object printValue)
        {
            if (printValue != null)
                View().ConControl.Cout.Print(printValue.ToString());

            View().EndSession();
        }

        private ConsoleView View()
        {
            return (ConsoleView)App.GetService<IViewService>().GetView("Console");
        }
    }
}
