using System;
using System.Collections.Generic;
using System.Reflection;

namespace ElaConsole.Options
{
	internal static class OptionMap<T>
	{
		#region Construction
		private const string DEF_OPT = "Default";
		private static Dictionary<String,PropertyInfo> properties;

		static OptionMap()
		{
			properties = new Dictionary<String,PropertyInfo>();
			BuildMap();
		}
		#endregion


		#region Methods
		internal static void SetOption(string key, string value, T cls)
		{
			key = key.Trim();
			var pi = default(PropertyInfo);

            //This is an argument
            if (key.Length > 0 && key[0] == '-' && properties.TryGetValue("arg", out pi) &&
                pi.PropertyType == typeof(List<ElaArg>))
            {
                var list = (List<ElaArg>)pi.GetValue(cls, null);
                list.Add(new ElaArg(key.TrimStart('-'), value));
            }
            else
            {
                if (!properties.TryGetValue(key, out pi))
                    throw new ElaOptionException(key, ElaOptionError.UnknownOption);

                if (pi.PropertyType == typeof(List<String>))
                {
                    var list = (List<String>)pi.GetValue(cls, null);
                    list.Add(value.Trim(' ', '"'));
                }
                else
                {
                    var obj = value == null ? true : ChangeType(value, pi.PropertyType);

                    if (value == null && pi.PropertyType != typeof(Boolean))
                        throw new ElaOptionException(key, ElaOptionError.InvalidFormat);

                    if (value is String)
                        value = value.Trim(' ', '"');

                    pi.SetValue(cls, obj, null);
                }
            }
		}


		private static object ChangeType(string value, Type type)
		{
			var ret = default(Object);

			if (type.IsEnum)
				ret = Enum.Parse(type, value.Trim(), true);
			else
				ret = Convert.ChangeType(value, type);

			return ret;
		}


		private static void BuildMap()
		{
			foreach (var pi in typeof(T).GetProperties())
			{
				var attr = (CommandLineOptionAttribute)Attribute.GetCustomAttribute(
					pi, typeof(CommandLineOptionAttribute));

				if (attr != null)
				{
					if (!String.IsNullOrEmpty(attr.Key))
						properties.Add(attr.Key, pi);
					if (!String.IsNullOrEmpty(attr.ShortKey))
						properties.Add(attr.ShortKey, pi);
					if (attr.DefaultOption)
						properties.Add(DEF_OPT, pi);
				}
			}
		}
		#endregion
	}
}
