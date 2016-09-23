using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;
using Elide.TextEditor;

namespace Elide.ElaCode.Views
{
    public sealed class DebugView : Elide.Environment.Views.AbstractView
    {
        private DebugViewGrid grid;

        public DebugView()
        {
            grid = new DebugViewGrid();
            grid.Dock = DockStyle.Fill;
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            grid.App = app;
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            var cm = builder
                .Item("Clear", ClearItems, () => grid.ItemsCount > 0)
                .Separator()
                .Item("All", null, () => grid.Show = TraceShowFlag.All, null, () => grid.Show == TraceShowFlag.All)
                .Item("Locals Only", null, () => grid.Show = TraceShowFlag.Locals, null, () => grid.Show == TraceShowFlag.Locals)
                .Item("Captured Only", null, () => grid.Show = TraceShowFlag.Captured, null, () => grid.Show == TraceShowFlag.Captured)
                .Finish();
            grid.ContextMenuStrip = cm;
        }

        public override void Activate()
        {
            grid.Select();
        }

        public void AddItems(IEnumerable<TraceItem> items)
        {
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
