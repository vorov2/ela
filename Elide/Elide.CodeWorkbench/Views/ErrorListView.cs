using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment;
using Elide.Environment.Views;

namespace Elide.CodeWorkbench.Views
{
    public sealed class ErrorListView : AbstractView
    {
        private ErrorListGrid grid;
        
        public ErrorListView()
        {
            
        }

        public override void Activate()
        {
            InitializeControl();
            grid.Select();
        }

        private void InitializeControl()
        {
            if (grid == null)
            {
                grid = new ErrorListGrid();
                grid.App = App;

                var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
                var cm = builder
                    .Item("Clear", Clear, () => grid.ItemsCount > 0)
                    .Separator()
                    .Item("Show Errors", null, () => grid.ShowErrors = !grid.ShowErrors, null, () => grid.ShowErrors)
                    .Item("Show Warnings", null, () => grid.ShowWarnings = !grid.ShowWarnings, null, () => grid.ShowWarnings)
                    .Item("Show Messages", null, () => grid.ShowMessages = !grid.ShowMessages, null, () => grid.ShowMessages)
                    .Finish();
                grid.ContextMenuStrip = cm;
            }
        }

        public void ShowMessages(IEnumerable<MessageItem> messages)
        {
            InitializeControl();
            grid.AddItems(messages);
            OnContentChanged(new ViewEventArgs(true, grid.ItemsCount.ToString()));
        }

        public void Clear()
        {
            InitializeControl();
            grid.ClearItems();
            OnContentChanged(ViewEventArgs.None);
        }

        public override object Control
        {
            get 
            {
                InitializeControl();
                return grid; 
            }
        }
    }
}
