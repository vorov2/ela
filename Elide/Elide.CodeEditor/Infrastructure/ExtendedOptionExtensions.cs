using System;
using System.Linq;
using System.Collections.Generic;

namespace Elide.CodeEditor.Infrastructure
{
    public static class ExtendedOptionExtensions
    {
        public static bool Set(this IEnumerable<ExtendedOption> @this, int code)
        {
            return @this.Any(o => o.Code == code && o.IsSet());
        }
    }
}
