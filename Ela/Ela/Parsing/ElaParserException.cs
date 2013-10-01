using System;

namespace Ela.Parsing
{
	public sealed class ElaParserException : ElaException
	{
		#region Construction
		internal ElaParserException(string message) : base(message, null)
		{

		}
		#endregion
	}
}
