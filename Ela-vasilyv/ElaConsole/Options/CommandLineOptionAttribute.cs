using System;

namespace ElaConsole.Options
{
	[AttributeUsage(AttributeTargets.Property, Inherited = false)]
	public sealed class CommandLineOptionAttribute : Attribute
	{
		#region Construction
		public CommandLineOptionAttribute(bool defaultOption) :
			this(null, null, defaultOption)
		{

		}
		
		
		public CommandLineOptionAttribute(string key) :
			this(key, null, false)
		{

		}
		
		
		public CommandLineOptionAttribute(string key, string shortKey) :
			this(key, shortKey, false)
		{
			
		}


		public CommandLineOptionAttribute(string key, string shortKey, bool defaultOption)
		{
			Key = key;
			ShortKey = shortKey;
			DefaultOption = defaultOption;
		}
		#endregion


		#region Properties
		public string Key { get; private set; }

		public string ShortKey { get; private set; }

		public bool DefaultOption { get; private set; }
		#endregion
	}
}
