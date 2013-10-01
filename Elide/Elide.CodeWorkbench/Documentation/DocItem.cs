using System;

namespace Elide.CodeWorkbench.Documentation
{
    public abstract class DocItem
    {
        protected DocItem(string name)
        {
            Name = name;
        }

        public string Name { get; private set; }
    }
}
