using System;
using System.Collections.Generic;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment.Views;

namespace Elide.CodeWorkbench.Views
{
    public sealed class ErrorListService : Service, IErrorListService
    {
        public ErrorListService()
        {

        }

        public void ShowMessages(IEnumerable<MessageItem> messages)
        {
            View().ShowMessages(messages);
        }

        public void Clear()
        {
            View().Clear();
        }

        private ErrorListView View()
        {
            return (ErrorListView)App.GetService<IViewService>().GetView("ErrorList");
        }
    }
}
