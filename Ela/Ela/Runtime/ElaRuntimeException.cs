using System;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime
{
	internal class ElaRuntimeException : ElaException
	{
		internal ElaRuntimeException(ElaRuntimeError error, params object[] arguments)
		{
            Error = error;
            Arguments = arguments;
		}

        internal ElaRuntimeException(ElaError err)
        {
            ErrorObj = err;
        }

        internal ElaRuntimeError Error { get; set; }

        internal object[] Arguments { get; set; }

        internal ElaError ErrorObj { get; set; }
	}
}
