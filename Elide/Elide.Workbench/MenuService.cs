using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;
using Elide.Main;

namespace Elide.Workbench
{
    [DependsFrom(typeof(IDaemonService))]
    public sealed class MenuService : Service, IMenuService, IDaemon
    {
        private List<ToolStrip> menus;

        public MenuService()
        {
            menus = new List<ToolStrip>();
        }

        public override void Initialize(IApp app)
        {
            base.Initialize(app);
            app.GetService<IDaemonService>().RegisterDaemon(this);
        }

        public IMenuBuilder<T> CreateMenuBuilder<T>() where T : new()
        {
            var builder = new MenuBuilder<T>();
            builder.Finished += (_,__) =>
                {
                    var ms = builder.GetToolStrip();

                    if (ms is ContextMenuStrip)
                        MenuRenderer.ApplySkin((ContextMenuStrip)ms);
                    else
                        ms.Renderer = new MenuRenderer();
                };

            var menuStrip = builder.Start();
            menus.Add(menuStrip);
            return builder;
        }

        public IMenuBuilder<T> CreateMenuBuilder<T>(T menu) where T : new()
        {
            var builder = new MenuBuilder<T>();
            builder.Finished += (_,__) =>
            {
                var ms = builder.GetToolStrip();

                if (ms is ContextMenuStrip)
                    MenuRenderer.ApplySkin((ContextMenuStrip)ms);
                else
                    ms.Renderer = new MenuRenderer();
            };

            var menuStrip = builder.Start(menu);
            menus.Add(menuStrip);
            return builder;
        }

        public void Execute()
        {
            menus.ForEach(m => Process(m.Items));
        }

        private void Process(ToolStripItemCollection items)
        {
            foreach (ToolStripItem i in items)
            {
                var t = i.Tag as Tag;

                if (t == null)
                    continue;

                if (t.Predicate != null)
                    i.Enabled = t.Predicate();

                if (t.CheckPredicate != null)
                    ((ToolStripMenuItem)i).Checked = t.CheckPredicate();

                if (i is ToolStripMenuItem)
                {
                    var mi = (ToolStripMenuItem)i;

                    if (mi.DropDownItems.Count > 0)
                        Process(mi.DropDownItems);
                }
            }
        }
    }
}
