using System;

namespace Elide.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class DependsFromAttribute : Attribute
    {
        public DependsFromAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; private set; }
    }
}
