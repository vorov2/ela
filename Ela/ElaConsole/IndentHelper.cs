using System;
using System.Collections.Generic;
using System.Text;

namespace ElaConsole
{
    internal static class IndentHelper
    {
        internal static int GetIndent(string line)
        {
            line = line ?? String.Empty;
            var upc = line.ToUpper();
            var trim = upc.TrimStart(' ');

            if (trim.StartsWith("WHERE"))
                return upc.IndexOf("WHERE") + 6;
            else
                return upc.Length - trim.Length;
        }
    }
}
