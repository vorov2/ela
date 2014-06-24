using Ela.Compilation;
using Ela.Linking;
using Ela.Runtime;
using Elide.CodeEditor.Views;
using Elide.Console;
using Elide.Console.Configuration;
using Elide.Core;
using Elide.ElaCode.Configuration;
using Elide.Environment;
using Elide.Environment.Configuration;
using Elide.Environment.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;


namespace Elide.ElaCode.Views
{
    public sealed class AstView : AbstractView
    {        
        public AstView()
        {
            
        }

        public override void Activate()
        {
            if (_control.TreeView.ContextMenuStrip == null)
                _control.TreeView.ContextMenuStrip = BuildContextMenu();
            
            base.Activate();
        }

        private ContextMenuStrip BuildContextMenu()
        {
            var builder = App.GetService<IMenuService>().CreateMenuBuilder<ContextMenuStrip>();
            var contextMenu = builder
                .Item("&Clear", () => { _control.TreeView.Nodes.Clear(); _control.ShowNoData(); }, () => _control.TreeView.Nodes.Count > 0)
                .Separator()
                .Item("&Expand All", () => _control.TreeView.ExpandAll(), () => _control.TreeView.Nodes.Count > 0)
                .Item("Co&llapse All", () => _control.TreeView.CollapseAll(), () => _control.TreeView.Nodes.Count > 0)
                .Finish();
            return contextMenu;
        }
        
        private AstControl _control = new AstControl();
        public override object Control
        {
            get { return _control; }
        }
    }
}
