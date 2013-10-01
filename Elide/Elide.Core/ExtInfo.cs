using System;

namespace Elide.Core
{
    public abstract class ExtInfo
    {
        protected ExtInfo(string key)
        {
            Key = key;
        }

        public string Key { get; private set; }
    }
}
