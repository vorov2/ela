using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
    public sealed class ResultGridView : Elide.Environment.Views.AbstractView
    {
        private ResultListGrid grid;

        public ResultGridView()
        {
            grid = new ResultListGrid();
            grid.Dock = DockStyle.Fill;
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            grid.App = app;

            var builder = app.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            grid.ContextMenuStrip = builder.Item("Clear", ClearItems, () => grid.ItemsCount > 0).Finish();
        }

        public override void Activate()
        {
            grid.Select();
        }

        public void AddItems(IEnumerable<ResultItem> items)
        {
            grid.ClearItems();
            grid.AddItems(items);
            OnContentChanged(new ViewEventArgs(true, grid.ItemsCount.ToString()));
        }

        public void ClearItems()
        {
            grid.ClearItems();
            OnContentChanged(ViewEventArgs.None);
        }

        public override object Control
        {
            get { return grid; }
        }
    }
}
