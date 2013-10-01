using System;

namespace Ela.Compilation
{
    //This exception is thrown to terminate the compilation
    //It is handled and not rethrown. It is only used to stop compilation
    //(e.g. when a maximum error limit is reached).
    internal sealed class TerminationException : Exception
    {
    }
}
