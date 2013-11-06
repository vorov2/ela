using System;
using System.Collections.Generic;
using System.Configuration;

namespace ElaConsole.Options
{
	internal static class ConfigReader
	{
		#region Methods
		internal static void ReadOptions(ElaOptions cls)
		{
			foreach (var k in ConfigurationManager.AppSettings.AllKeys)
			{
				var value = ConfigurationManager.AppSettings[k];

				if (value.Contains(";"))
				{
					var arr = value.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

					foreach (var s in arr)
						OptionMap<ElaOptions>.SetOption(k, s, cls);
				}
				else
					OptionMap<ElaOptions>.SetOption(k, value, cls);
			}
		}
		#endregion
	}
}
