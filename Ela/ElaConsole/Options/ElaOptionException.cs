using System;
using System.Collections.Generic;

namespace ElaConsole.Options
{
	public sealed class ElaOptionException : Exception
	{
		#region Construction
		internal ElaOptionException(string opt, ElaOptionError error)
		{
			Option = opt;
			Error = error;
		}
		#endregion


		#region Properties
		public string Option { get; private set; }

		public ElaOptionError Error { get; private set; }
		#endregion
	}
}
