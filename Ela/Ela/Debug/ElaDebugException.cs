using System;

namespace Ela.Debug
{
	public sealed class ElaDebugException : ElaException
	{
		#region Construction
		internal ElaDebugException(string message, Exception ex) : base(message, ex)
		{

		}


		internal ElaDebugException(string message) : base(message, null)
		{

		}
		#endregion
	}
}
