using System;

namespace Ela.Linking
{
	public sealed class ElaLinkerException : ElaException
	{
		#region Construction
		public ElaLinkerException(string message, Exception ex) : base(message, ex)
		{

		}
		#endregion
	}
}
