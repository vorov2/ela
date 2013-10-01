using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eladoc
{
    public sealed class Doc : DocItem
    {
        public Doc()
        {
            Items = new List<DocItem>();
        }

        public string Title { get; set; }

        public string Category { get; set; }

        public string File { get; set; }

        public List<DocItem> Items { get; private set; }
    }
}
