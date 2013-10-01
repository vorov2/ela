using System;
using System.Resources;

namespace Ela.Properties
{
	internal static class Strings
	{
		private static ResourceManager resErr = new ResourceManager("Ela.Properties.Errors", typeof(Strings).Assembly);
		private static ResourceManager resMsg = new ResourceManager("Ela.Properties.Strings", typeof(Strings).Assembly);

		internal static string GetError(ElaErrorType error, params object[] args)
		{
			return String.Format(resErr.GetString(error.ToString()), args);
		}


		internal static string GetMessage(string key, params object[] args)
		{
			return String.Format(resMsg.GetString(key), args);
		}
	}
}
