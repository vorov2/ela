using System;
namespace Elide
{
    public static class Attr
    {
        public static T Get<T>(Type type) where T : Attribute
        {
            return (T)Attribute.GetCustomAttribute(type, typeof(T));
        }
    }
}
