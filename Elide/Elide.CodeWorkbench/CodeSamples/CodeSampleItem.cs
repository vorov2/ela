using System;

namespace Elide.CodeWorkbench.CodeSamples
{
    public abstract class CodeSampleItem
    {
        protected CodeSampleItem(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; private set; }

        public string Description { get; private set; }
    }
}
