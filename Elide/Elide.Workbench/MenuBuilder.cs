using System;
using System.Linq;
using System.Windows.Forms;
using Elide.Core;
using System.Collections.Generic;
using Elide.Environment;
using Elide.Forms;
using System.Globalization;

namespace Elide.Workbench
{
    internal sealed class MenuBuilder<T> : IMenuBuilder<T> where T : new()
    {
        private static Dictionary<String,Keys> keysCache = new Dictionary<String,Keys>();

        private ToolStrip menuStrip;
        private Stack<ToolStripMenuItem> menu;
        private KeysConverter conv;
        private bool dynamic;
        private int dynamicIndex;

        static MenuBuilder()
        {
            //Add some commonly used keys
            //Ctrl, Ins and Del are used as shortcuts
            keysCache.Add("Ctrl", Keys.Control);
            keysCache.Add("Ins", Keys.Insert);
            keysCache.Add("Del", Keys.Delete);
        }

        public MenuBuilder()
        {
            conv = new KeysConverter();
            menu = new Stack<ToolStripMenuItem>();
        }

        public ToolStrip Start()
        {
            return menuStrip = (ToolStrip)(Object)new T();
        }

        public ToolStrip Start(T menu)
        {
            return menuStrip = (ToolStrip)(Object)menu;
        }

        public T Finish()
        {
            OnFinished();
            return (T)(Object)menuStrip;
        }

        public IMenuBuilder<T> Separator()
        {
            var s = new ToolStripSeparator();
            s.Tag = new Tag { Data = (dynamic ? (Object)true : null) };
            Add(s);
            return this;
        }

        public IMenuBuilder<T> Menu(string text)
        {
            return Menu(text, null);
        }

        public IMenuBuilder<T> Menu(string text, Action<Object> expandHandler)
        {
            var mi = new ToolStripMenuItem(text);
            mi.Tag = new Tag();

            if (expandHandler != null)
                mi.DropDownOpening += (o, e) => expandHandler(o);

            Add(mi);
            menu.Push(mi);
            return this;
        }

        public IMenuBuilder<T> CloseMenu()
        {
            menu.Pop();
            return this;
        }

        public IMenuBuilder<T> Items(Action<IMenuBuilder<T>> addAct)
        {
            addAct(this);
            return this;
        }

        public IMenuBuilder<T> ItemsDynamic(Action<IMenuBuilder<T>> addAct)
        {
            if (menu.Count != 0)
            {
                var mi = menu.Peek();
                var newBuilder = new MenuBuilder<T>();
                newBuilder.dynamic = true;
                var dynIndex = mi.DropDownItems.Count;
                mi.DropDownItems.Add("-");
                
                mi.DropDownOpening += (o,e) => {
                    newBuilder.dynamicIndex = dynIndex;
                    mi.DropDownItems.OfType<ToolStripItem>()
                        .ToList()
                        .Where(i => i.Tag == null || ((Tag)i.Tag).Data != null)
                        .ForEach(i => mi.DropDownItems.Remove(i));

                    ((MenuBuilder<T>)newBuilder).Start((T)(Object)menuStrip);
                    newBuilder.menu.Push(mi);
                    addAct(newBuilder);
                    newBuilder.Finish();
                    var size = MenuRenderer.MeasureDropDown(mi.DropDown);
                    mi.DropDown.Height = size.Height + 5;
                    mi.DropDown.Width = size.Width;
                };
            }

            return this;
        }

        public IMenuBuilder<T> Item(string text, Action handler, Func<Boolean> pred)
        {
            return Item(text, null, handler, pred, null);
        }

        public IMenuBuilder<T> Item(string text, Action handler)
        {
            return Item(text, null, handler, null, null);
        }

        public IMenuBuilder<T> Item(string text, string keys, Action handler)
        {
            return Item(text, keys, handler, null, null);
        }

        public IMenuBuilder<T> Item(string text, string keys, Action handler, Func<Boolean> pred)
        {
            return Item(text, keys, handler, pred, null);
        }

        public IMenuBuilder<T> Item(string text, string keys, Action handler, Func<Boolean> pred, Func<Boolean> checkPred)
        {
            var mi = new ToolStripMenuItem(text, null, (o,e) => handler());

            if (!String.IsNullOrEmpty(keys))
                mi.ShortcutKeys = ConvertFromString(keys);

            mi.Tag = new Tag { Predicate = pred, CheckPredicate = checkPred, Data = (dynamic ? (Object)true : null) };
            Add(mi);
            return this;
        }

        private Keys ConvertFromString(string str)
        {
            var arr = str.Split(new char[] { '+' }, StringSplitOptions.RemoveEmptyEntries);
            var keys = default(Keys);

            foreach (var a in arr)
            {
                var kv = default(Keys);

                if (!keysCache.TryGetValue(a, out kv))
                {
                    kv = (Keys)Enum.Parse(typeof(Keys), a);
                    keysCache.Add(a, kv);
                }

                keys |= kv;
            }

            return keys;
        }
        
        private void Add(ToolStripItem mi)
        {
            if (menu.Count > 0)
            {
                if (dynamic)
                    menu.Peek().DropDownItems.Insert(dynamicIndex++, mi);
                else
                    menu.Peek().DropDownItems.Add(mi);
            }
            else
            {
                if (dynamic)
                    menuStrip.Items.Insert(dynamicIndex++, mi);
                else
                    menuStrip.Items.Add(mi);
            }
        }

        internal ToolStrip GetToolStrip()
        {
            return menuStrip;
        }

        internal event EventHandler Finished;
        private void OnFinished()
        {
            var h = Finished;

            if (h != null)
                h(this, EventArgs.Empty);
        }
    }
}
