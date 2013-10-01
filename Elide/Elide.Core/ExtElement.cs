using System;

namespace Elide.Core
{
    public sealed class ExtElement
    {
        public ExtElement(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return String.Format("{0}={1}", Key, Value);
        }

        public string Key { get; private set; }

        public string Value { get; private set; }
    }
}
