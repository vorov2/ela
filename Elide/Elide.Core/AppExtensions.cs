using System;

namespace Elide.Core
{
    public static class AppExtensions
    {
        public static T GetService<T>(this IApp app)
        {
            return (T)app.GetService(typeof(T));
        }
    }
}
