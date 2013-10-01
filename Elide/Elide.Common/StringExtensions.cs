using System;
using System.Text;

namespace Elide
{
    public static class StringExtensions
    {
        public static string Remove(this string @this, string text)
        {
            return @this.Replace(text, String.Empty);
        }

        public static string Repeat(this string @this, string text, int times)
        {
            var sb = new StringBuilder(@this);

            for (var i = 0; i < times; i++)
                sb.Append(text);

            return sb.ToString();
        }
    }
}
