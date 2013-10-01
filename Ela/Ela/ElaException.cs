using System;

namespace Ela
{
	public class ElaException : Exception
	{
        internal ElaException()
        {

        }

        public ElaException(string message) : base(message, null)
        {

        }

		public ElaException(string message, Exception ex) : base(message, ex)
		{

		}
	}
}
