using System;
using System.Linq;

namespace Elide.Core
{
    public sealed class ExtEntry
    {
        public ExtEntry(string key, ExtList<ExtElement> elements) : this(key, elements, ExtList<ExtEntry>.Empty)
        {
            
        }

        public ExtEntry(string key, ExtList<ExtElement> elements, ExtList<ExtEntry> children)
        {
            Key = key;
            Elements = elements;
            Children = children;
        }

        public Nullable<T> ElementNullable<T>(string key) where T : struct
        {
            return Element<Nullable<T>>(key, null);
        }

        public T Element<T>(string key)
        {
            return Element<T>(key, default(T));
        }

        public T Element<T>(string key, T def)
        {
            var str = Element(key);
            var type = typeof(T);

            if (str == null)
                return def;

            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];

            if (type.IsEnum)
                return (T)Enum.Parse(type, str);
            else if (type == typeof(Boolean))
                return (T)(Object)StringComparer.OrdinalIgnoreCase.Equals(str, Boolean.TrueString);
            else
                return (T)Convert.ChangeType(str, typeof(T));
        }

        public string Element(string key)
        {
            var el = Elements.FirstOrDefault(e => e.Key == key);
            return el != null ? el.Value : null;
        }

        public override string ToString()
        {
            return String.Format("{0}: {1}", Key, Elements.Count);
        }

        public string Key { get; private set; }

        public ExtList<ExtElement> Elements { get; private set; }

        public ExtList<ExtEntry> Children { get; private set; }
    }
}
