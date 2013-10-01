using System;
using System.Resources;

namespace Ela.Compilation
{
	internal static class Strings
	{
		private static ResourceManager errors = new ResourceManager("Ela.Compilation.Strings.Errors", typeof(Strings).Assembly);
		private static ResourceManager warnings = new ResourceManager("Ela.Compilation.Strings.Warnings", typeof(Strings).Assembly);
		private static ResourceManager hints = new ResourceManager("Ela.Compilation.Strings.Hints", typeof(Strings).Assembly);
		private static ResourceManager messages = new ResourceManager("Ela.Compilation.Strings.Messages", typeof(Strings).Assembly);

		internal static string GetMessage(string key, params object[] args)
		{
			return String.Format(messages.GetString(key), args);
		}
		
		
		internal static string GetError(ElaCompilerError error, params object[] args)
		{
			return String.Format(errors.GetString(error.ToString()), args);
		}


		internal static string GetWarning(ElaCompilerWarning warning, params object[] args)
		{
			return String.Format(warnings.GetString(warning.ToString()), args);
		}


		internal static string GetHint(ElaCompilerHint hint, params object[] args)
		{
			return String.Format(hints.GetString(hint.ToString()), args);
		}
	}
}
