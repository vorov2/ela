using System;

namespace Ela
{
	public sealed class ElaTranslationException : ElaException
	{
		#region Construction
		public ElaTranslationException(string message)
			: base(message, null)
		{

		}


		public ElaTranslationException(string message, Exception ex)
			: base(message, ex)
		{

		}
		#endregion
	}
}
