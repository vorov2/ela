using System;
using System.Collections.Generic;

namespace Ela.Runtime
{
    internal static class EqHelper
    {
        internal static bool ListEquals<T>(IList<T> left, IList<T> right) where T : IEquatable<T>
        {
            if (left.Count != right.Count)
                return false;

            for (var i = 0; i < left.Count; i++)
                if (!left[i].Equals(right[i]))
                    return false;

            return true;
        }
    }
}
