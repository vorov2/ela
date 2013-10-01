using System;
using System.Resources;

namespace Ela.Runtime
{
	internal static class Strings
	{
		private static ResourceManager errors = new ResourceManager("Ela.Runtime.Strings.Errors", typeof(Strings).Assembly);
		private static ResourceManager messages = new ResourceManager("Ela.Runtime.Strings.Messages", typeof(Strings).Assembly);

		internal static string GetMessage(string key, params object[] args)
		{
			return String.Format(messages.GetString(key), args);
		}
        
		internal static string GetError(ElaRuntimeError error, params object[] args)
		{
			try
			{
				return String.Format(errors.GetString(error.ToString()), args);
			}
			catch (FormatException)
			{
				return errors.GetString(error.ToString()); 
			}
		}
	}
}
