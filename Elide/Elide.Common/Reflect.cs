using System;
using System.Linq;
using System.Reflection;

namespace Elide
{
    public static class Reflect
    {
        public static T Create<T>(params object[] args)
        {
            var flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var ctor = typeof(T).GetConstructor(flags, null, args.Select(a => a.GetType()).ToArray(), null);
            var res = ctor.Invoke(flags, null, args, null);
            return (T)res;
        }

        public static object GetPropertyValue<T>(T obj, string name)
        {
            var pi = typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            return pi.GetValue(obj, null);
        }
    }
}
