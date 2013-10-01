using System;

namespace Elide
{
    public static class EnumExtensions
    {
        public static bool Set<T>(this T enu, T val) where T : struct
        {
            return ((Int32)(Object)enu & (Int32)(Object)val) == (Int32)(Object)val;
        }
    }
}
