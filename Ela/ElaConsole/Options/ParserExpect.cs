using System;

namespace ElaConsole.Options
{
	internal enum ParserExpect
	{
		ParameterStart,

		ParameterEnd,

		ParameterValue,

		ParameterValueEnd,

		ParameterValueQuoteEnd
	}
}
