using System;
using System.Collections.Generic;
using System.IO;

namespace Elide.CodeWorkbench.Documentation
{
    public sealed class DocFolder : DocItem
    {
        public DocFolder(string name) : base(name)
        {
            _items = new List<DocItem>();
        }

        internal void AddNode(DocItem item)
        {
            _items.Add(item);
        }

        private List<DocItem> _items;
        public IEnumerable<DocItem> Items
        {
            get { return _items; }
        }
    }
}
