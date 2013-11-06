using System;
using System.Collections.Generic;
using System.Reflection;

namespace ElaConsole.Options
{
	internal sealed class CommandLineParser
	{
		private ElaOptions cls;
		
		internal CommandLineParser(ElaOptions cls)
		{
			this.cls = cls;
		}
		
        internal void Parse(string[] args)
		{
			var index = 0;

			if (args.Length > 0 && args[0][0] != '-')
			{
				cls.FileName = args[0];
				index = 1;
			}

			ParseOptions(args, index);
		}
        
		private void ParseOptions(string[] args, int index)
		{
			var opt = String.Empty;

			for (var i = index; i < args.Length; i++)
			{
				var s = args[i];

				if (s[0] != '-')
				{
					if (opt.Length == 0)
						throw new ElaOptionException(String.Empty, ElaOptionError.InvalidFormat);
					else
					{
						OptionMap<ElaOptions>.SetOption(opt.Substring(1), s, cls);
						opt = String.Empty;
					}
				}
				else
				{
					if (opt.Length != 0)
						OptionMap<ElaOptions>.SetOption(opt.Substring(1), null, cls);

					opt = s;
				}
			}

			if (opt.Length != 0)
				OptionMap<ElaOptions>.SetOption(opt.Substring(1), null, cls);
		}
	}
}
