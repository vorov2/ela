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
        private static readonly StringBuilder empty = new StringBuilder();

        public StringModule()
        {

        }

        public override void Initialize()
        {
            Add<String,IEnumerable<ElaString>,String>("format", Format);
            Add<String,ElaObject>("getFormats", GetFormats);

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

        private string Format(string format, IEnumerable<ElaString> values)
        {
            var objs = new List<Object>(10);

            foreach (var v in values)
                objs.Insert(0, v.ToString());

            return String.Format(format, objs.ToArray());
        }

        public static ElaObject GetFormats(string format)
        {
            try
            {
                var fmt = 0;
                var sb = new StringBuilder();
                var fsb = default(StringBuilder);
                var buffer = format.ToCharArray();
                var len = buffer.Length;
                var dict = new Dictionary<Int32,List<String>>();
                var num = String.Empty;

                for (var i = 0; i < len; i++)
                {
                    var c = buffer[i];
                    var end = i == len - 1;

                    if (fmt > 0 && c == '}' && (end || buffer[i + 1] != '}'))
                    {
                        var i4 = Int32.Parse(num);
                        var list = default(List<String>);

                        if (!dict.TryGetValue(i4, out list))
                        {
                            list = new List<String>();
                            dict.Add(i4, list);
                        }

                        sb.Append(num + ":" + list.Count);
                        sb.Append(c);
                        list.Add((fsb ?? empty).ToString());
                        num = String.Empty;
                        fsb = null;
                        fmt = 0;
                    }
                    else if (fmt == 1 && c == ':')
                    {
                        fsb = new StringBuilder();
                        fmt = 2;
                    }
                    else if (fmt == 1)
                    {
                        num += c;
                    }
                    else if (fmt == 2)
                    {
                        fsb.Append(c);
                    }
                    else if (fmt == 0 && c == '{' && !end && buffer[i + 1] != '{' && (i == 0 || buffer[i - 1] != '{'))
                    {
                        sb.Append(c);
                        fmt = 1;
                    }
                    else
                        sb.Append(c);
                }

                var shift = 0;

                for (var n = 0; n < dict.Count; n++)
                {
                    var val = dict[n];

                    for (var i = 0; i < val.Count; i++)
                    {
                        if (i > 0)
                            shift++;

                        var mask = "{" + n + ":" + i + "}";
                        sb.Replace(mask, "{" + (n + shift) + "}");
                    }
                }
                
                var lst = new ElaList[dict.Count];

                foreach (var kv in dict)
                    lst[kv.Key] = ElaList.FromEnumerable(kv.Value);

                var elaStr = new ElaValue(sb.ToString());
                var elaList = new ElaValue(ElaList.FromEnumerable(lst));
                return new ElaTuple(elaStr, elaList);
            }
            catch (Exception)
            {
                return ElaUnit.Instance;
            }
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
