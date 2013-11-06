using System;
using System.Resources;

namespace Ela.Parsing
{
	internal static class Strings
	{
		private static ResourceManager res = new ResourceManager("Ela.Parsing.Strings.Errors", typeof(Strings).Assembly);

		internal static string GetMessage(ElaParserError error, params object[] args)
		{
			return String.Format(res.GetString(error.ToString()), args);
		}
	}
}
