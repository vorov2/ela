using System;

namespace Elide.Workbench
{
    public sealed class Tag
    {
        internal Func<Boolean> Predicate { get; set; }

        internal Func<Boolean> CheckPredicate { get; set; }

        public object Data { get; set; }

        public object CustomData { get; set; }
    }
}
