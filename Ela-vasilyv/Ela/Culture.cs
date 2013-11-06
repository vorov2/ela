using System;
using System.Globalization;

namespace Ela
{
	internal static class Culture
	{
		internal static readonly IFormatProvider NumberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;
	}
}
