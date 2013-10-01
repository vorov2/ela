using System;
using System.Collections.Generic;

namespace Elide
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> seq, Action<T> act)
        {
            foreach (var e in seq)
                act(e);
        }

        public static void ForEachIndex<T>(this IEnumerable<T> seq, Action<T,Int32> act)
        {
            var idx = 0;

            foreach (var e in seq)
                act(e, idx++);
        }

        public static IEnumerable<T2> SelectIndex<T1,T2>(this IEnumerable<T1> seq, Func<T1,Int32,T2> act)
        {
            var idx = 0;

            foreach (var e in seq)
                yield return act(e, idx++);
        }

        public static IEnumerable<T> TakeUntil<T>(this IEnumerable<T> seq, Func<T,Boolean> pred)
        {
            foreach (var e in seq)
            {
                yield return e;

                if (pred(e))
                    break;
            }
        }
    }
}
