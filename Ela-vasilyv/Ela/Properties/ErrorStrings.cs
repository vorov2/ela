using System;
using System.Resources;

namespace Ela.Properties
{
	internal static class ErrorStrings
	{
		private static ResourceManager res = new ResourceManager("Ela.Properties.Resources", typeof(ErrorStrings).Assembly);

		internal static string GetString(ElaErrorType error, params object[] args)
		{
			return String.Format(res.GetString(error.ToString()), args);
		}
	}
}
