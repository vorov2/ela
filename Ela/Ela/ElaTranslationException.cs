using System;

namespace Ela
{
    public sealed class ElaTranslationException : ElaException
    {
        public ElaTranslationException(string message) : base(message, null)
        {

        }

        public ElaTranslationException(string message, Exception ex) : base(message, ex)
        {

        }
    }
}
