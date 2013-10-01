using System;
using System.Collections.Generic;
using System.IO;

namespace Elide.CodeWorkbench.CodeSamples
{
    public sealed class CodeSampleFolder : CodeSampleItem
    {
        public CodeSampleFolder(string name, string description) : base(name, description)
        {
            _items = new List<CodeSampleItem>();
        }

        internal void AddNode(CodeSampleItem item)
        {
            _items.Add(item);
        }

        private List<CodeSampleItem> _items;
        public IEnumerable<CodeSampleItem> Items
        {
            get { return _items; }
        }
    }
}
