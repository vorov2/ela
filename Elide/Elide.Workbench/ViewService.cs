using System;
using System.Collections.Generic;
using System.Linq;
using Elide.Environment.Views;

namespace Elide.Workbench
{
    public sealed class ViewService : AbstractViewService
    {
        private readonly object syncRoot = new Object();
        private Dictionary<String,IView> views;

        public ViewService()
        {
            views = new Dictionary<String,IView>();
        }

        public override IView GetView(string key)
        {
            var view = default(IView);

            if (!views.TryGetValue(key, out view))
            {
                lock (syncRoot)
                {
                    if (!views.TryGetValue(key, out view))
                    {
                        var inf = (ViewInfo)GetInfo("views", key);
                        view = (IView)Activator.CreateInstance(inf.Type);
                        view.Initialize(App);
                        views.Add(key, view);
                    }
                }
            }

            return view;
        }

        public override bool OpenView(string key)
        {
            var idx = WB.Form.ActiveToolbar.EnumerateTags().OfType<ViewInfo>().Select(v => v.Key).ToList().IndexOf(key);

            if (idx != -1)
            {
                if (WB.Form.ActiveToolbar.SelectedIndex != idx)
                    WB.Form.ActiveToolbar.SelectedIndex = idx;

                return true;
            }
            else
            {
                idx = WB.Form.ToolWindow.Items.Select(i => i.Tag).OfType<ViewInfo>().Select(v => v.Key).ToList().IndexOf(key);

                if (idx != -1)
                {
                    if (WB.Form.ToolWindow.SelectedIndex != idx)
                        WB.Form.ToolWindow.SelectedIndex = idx;

                    return true;
                }
                
                return false;                 
            }
        }

        public override bool CloseView(string key)
        {
            if (WB.Form.ActiveToolbar.SelectedTag != null && ((ViewInfo)WB.Form.ActiveToolbar.SelectedTag).Key == key)
            {
                WB.Form.ActiveToolbar.SelectedIndex = -1;
                return true;
            }
            else if (WB.Form.ToolWindow.SelectedIndex != -1 && ((ViewInfo)WB.Form.ToolWindow.Items[WB.Form.ToolWindow.SelectedIndex].Tag).Key == key)
            {
                WB.Form.ToolWindow.SelectedIndex = -1;
                return true;
            }
            else
                return false;
        }

        public override bool IsViewActive(string key)
        {
            return WB.Form.ActiveToolbar.SelectedTag != null && ((ViewInfo)WB.Form.ActiveToolbar.SelectedTag).Key == key ||
                (WB.Form.ToolWindow.SelectedIndex != -1 && ((ViewInfo)WB.Form.ToolWindow.Items[WB.Form.ToolWindow.SelectedIndex].Tag).Key == key);
        }
    }
}
