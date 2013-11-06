using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Library
{
    public sealed class Wrapper<T> : ElaObject
    {
        public T Value { get; private set; }

        public Wrapper(T value) : base()
        {
            Value = value;
        }
    }
}
