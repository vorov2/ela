using System;
using System.Resources;

namespace Ela.Linking
{
	internal static class Strings
	{
		private static ResourceManager errors = new ResourceManager("Ela.Linking.Strings.Errors", typeof(Strings).Assembly);
		private static ResourceManager warnings = new ResourceManager("Ela.Linking.Strings.Warnings", typeof(Strings).Assembly);
		private static ResourceManager messages = new ResourceManager("Ela.Linking.Strings.Messages", typeof(Strings).Assembly);

		internal static string GetMessage(string key, params object[] args)
		{
			return String.Format(messages.GetString(key), args);
		}
		

		internal static string GetError(ElaLinkerError error, params object[] args)
		{
			return String.Format(errors.GetString(error.ToString()), args);
		}


		internal static string GetWarning(ElaLinkerWarning warning, params object[] args)
		{
			return String.Format(warnings.GetString(warning.ToString()), args);
		}
	}
}
