using System;

namespace Ela.Parsing
{
    public sealed class ElaParserException : ElaException
    {
        internal ElaParserException(string message) : base(message, null)
        {

        }
    }
}
