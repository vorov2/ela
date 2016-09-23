using System;
using System.Collections.Generic;
using Elide.Core;
using Elide.Environment.Views;
using Elide.TextEditor;

namespace Elide.ElaCode.Views
{
    public sealed class DebugViewService : Service
    {
        public DebugViewService()
        {

        }

        public void AddItems(IEnumerable<TraceItem> items)
        {
            var grid = (DebugView)App.GetService<IViewService>().GetView("Debug");
            grid.AddItems(items);
        }

        public void ClearItems()
        {
            var grid = (DebugView)App.GetService<IViewService>().GetView("Debug");
            grid.ClearItems();
        }
    }
}
