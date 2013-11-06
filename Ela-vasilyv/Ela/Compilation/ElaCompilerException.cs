using System;

namespace Ela.Compilation
{
	public sealed class ElaCompilerException : ElaException
	{
		#region Construction
		public ElaCompilerException(string message, Exception ex) : base(message, ex)
		{

		}
		#endregion
	}
}
