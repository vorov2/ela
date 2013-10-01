using System;
using System.Collections.Generic;
using Elide.Core;
using Elide.Environment.Views;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
    public sealed class ResultGridService : Service, IResultGridService
    {
        public ResultGridService()
        {

        }

        public void AddItems(IEnumerable<ResultItem> items)
        {
            var grid = (ResultGridView)App.GetService<IViewService>().GetView("ResultGrid");
            grid.AddItems(items);
        }

        public void ClearItems()
        {
            var grid = (ResultGridView)App.GetService<IViewService>().GetView("ResultGrid");
            grid.ClearItems();
        }
    }
}
