using System;

namespace Elide.Core
{
    public class ElideException : Exception
    {
        public ElideException(string message, params object[] args) : base(String.Format(message, args))
        {

        }

        public ElideException(Exception ex, string message, params object[] args) : base(String.Format(message, args), ex)
        {

        }
    }
}
