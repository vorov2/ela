using System;
using System.Collections.Generic;
using System.Text;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;
using System.Globalization;

namespace Ela.Library.General
{
	public sealed class StringModule : ForeignModule
	{
        private sealed class FormatValue : IFormattable
        {
            internal static readonly IFormatProvider NumberFormat = CultureInfo.GetCultureInfo("en-US").NumberFormat;
            
            private readonly ElaFunction showf;
            private readonly ElaValue val;

            public FormatValue(ElaFunction showf, ElaValue val)
            {
                this.showf = showf;
                this.val = val;
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                if (val.TypeCode == ElaTypeCode.Long ||
                    val.TypeCode == ElaTypeCode.String ||
                    val.TypeCode == ElaTypeCode.Char ||
                    val.TypeCode == ElaTypeCode.Double ||
                    val.TypeCode == ElaTypeCode.Single ||
                    val.TypeCode == ElaTypeCode.Integer ||
                    val.TypeCode == ElaTypeCode.Boolean ||
                    val.TypeCode == ElaTypeCode.Unit ||
                    val.TypeCode == ElaTypeCode.Function ||
                    val.TypeCode == ElaTypeCode.Module)
                    return val.ToString(format ?? String.Empty, NumberFormat);

                return showf.Call(new ElaValue(format ?? String.Empty), val).AsString();
            }
        }

		public StringModule()
		{

		}

		public override void Initialize()
		{
            Add<String,Int32>("countArgs", CountArguments);
            Add<String,ElaFunction,IEnumerable<ElaValue>,String>("format", Format);
			Add<String,String>("upper", ToUpper);
			Add<String,String>("lower", ToLower);
			Add<String,String>("trim", Trim);
			Add<Char[],String,String>("trimChars", TrimChars);
			Add<String,String>("trimStart", TrimStart);
			Add<String,String>("trimEnd", TrimEnd);
			Add<Char[],String,String>("trimStartChars", TrimStartChars);
			Add<Char[],String,String>("trimEndChars", TrimEndChars);
			Add<String,String,Int32>("indexOf", IndexOf);
			Add<String,Int32,String,Int32>("indexOfFrom", IndexOfFrom);
			Add<String,String,Int32>("indexOfLast", LastIndexOf);
			Add<String,String,Boolean>("startsWith", StartsWith);
			Add<String,String,Boolean>("endsWith", EndsWith);
			Add<String,String,String,String>("replace", Replace);
			Add<Int32,Int32,String,String>("remove", Remove);
			Add<String,String,ElaList>("split", Split);
			Add<Int32,Int32,String,String>("substr", Substring);
			Add<Int32,String,String,String>("insert", Insert);
            Add<ElaString,ElaList>("toList", ToList);
		}

        private string Format(string format, ElaFunction showf, IEnumerable<ElaValue> values)
        {
            var objs = new List<Object>(10);

            foreach (var v in values)
                objs.Insert(0, new FormatValue(showf, v));

            return String.Format(format, objs.ToArray());
        }

        private int CountArguments(string format)
        {
            var ptr = 0;
            var start = ptr;
            var args = 0;
            var dict = new Dictionary<Int32, Int32>();

            while (ptr < format.Length)
            {
                var c = format[ptr++];

                if (c == '{')
                {
                    if (format[ptr] == '{')
                    {
                        start = ptr++;
                        continue;
                    }

                    var idx = format.IndexOf('}', ptr - 1);

                    if (idx != -1)
                    {
                        var sub = format.Substring(ptr, idx - ptr);
                        var idx2 = sub.IndexOf(':');

                        if (idx2 != -1)
                            sub = sub.Substring(0, idx2);

                        var i = Int32.Parse(sub);

                        if (!dict.ContainsKey(i))
                        {
                            start = ptr;
                            args++;
                            dict.Add(i, i);
                        }
                    }
                }
            }

            return args;
        }		
        
		public string ToUpper(string val)
		{
			return val.ToUpper();
		}

		public string ToLower(string val)
		{
			return val.ToLower();
		}
        
		public string Trim(string val)
		{
			return val.Trim();
		}
        
		public string TrimChars(char[] cz, string val)
		{
			return val.Trim(cz);
		}
        
		public string TrimStart(string str)
		{
			return str.TrimStart();
		}

		public string TrimEnd(string str)
		{
			return str.TrimEnd();
		}
        
		public string TrimStartChars(char[] cz, string val)
		{
			return val.TrimStart(cz);
		}

		public string TrimEndChars(char[] cz, string val)
		{
			return val.TrimEnd(cz);
		}
        
		public int IndexOf(string search, string str)
		{
			return str.IndexOf(search);
		}
        
		public int IndexOfFrom(string search, int index, string str)
		{
			return str.IndexOf(search, index);
		}

		public int LastIndexOf(string search, string str)
		{
			return str.LastIndexOf(search);
		}
        		
		public bool StartsWith(string search, string str)
		{
			return str.StartsWith(search);
		}
        
		public bool EndsWith(string search, string str)
		{
			return str.EndsWith(search);
		}
        
		public string Replace(string search, string replace, string str)
		{
			return str.Replace(search, replace);
		}

		public string Remove(int start, int count, string str)
		{
			return str.Remove(start, count);
		}

		public string Substring(int start, int length, string str)
		{
			return str.Substring(start, length);
		}

		public ElaList Split(string sep, string str)
		{
			var arr = str.Split(new string[] { sep }, StringSplitOptions.RemoveEmptyEntries);
			var list = ElaList.Empty;

			for (var i = arr.Length - 1; i > -1; i--)
				list = new ElaList(list, new ElaValue(arr[i]));

			return list;
		}

		public string Insert(int index, string toInsert, string str)
		{
            return str.Insert(index, toInsert);
		}

        public ElaList ToList(ElaString str)
        {
            return str.ToList();
        }
	}
}
