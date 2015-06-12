using System;

namespace Ela.Linking
{
    public sealed class ElaLinkerException : ElaException
    {
        public ElaLinkerException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
