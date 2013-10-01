using System;

namespace Elide.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public sealed class ExecOrderAttribute : Attribute
    {
        public ExecOrderAttribute(int order)
        {
            Order = order;
        }

        public int Order { get; private set; }
    }
}
