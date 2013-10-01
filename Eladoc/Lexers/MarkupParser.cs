using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eladoc.Lexers
{
    internal static class MarkupParser
    {
        private const char CR = '\r';
        private const char LF = '\n';
        private const char SP = ' ';
        private const char TB = '\t';
        private const char SL = '/';
        private const char DD = ':';
        private const char NIL = '\0';
        private const char BS = '*';
        private const char IS = '_';
        private const char KS = '`';


        enum ItemType
        {
            Bold,
            Italic,
            Keyword
        }

        struct Loc
        {
            public readonly int Start;
            public readonly int Len;
            public readonly ItemType Type;

            internal Loc(int start, int len, ItemType type)
            {
                Start = start;
                Len = len;
                Type = type;
            }
        }

        public static string Parse(string source)
        {
            if (source.Trim().Length < 2)
                return source;

            var sb = new StringBuilder(source);
            var shift = 0;

            foreach (var l in Process(source))
            {
                sb.Remove(l.Start + shift, 1);
                sb.Remove(l.Start + shift + l.Len - 2, 1);

                switch (l.Type)
                {
                    case ItemType.Bold:
                        sb.Insert(l.Start + shift, "<b>");
                        shift += 2;
                        sb.Insert(l.Start + shift + l.Len - 1, "</b>");
                        shift += 3;
                        break;
                    case ItemType.Italic:
                        sb.Insert(l.Start + shift, "<i>");
                        shift += 2;
                        sb.Insert(l.Start + shift + l.Len - 1, "</i>");
                        shift += 3;
                        break;
                    case ItemType.Keyword:
                        sb.Insert(l.Start + shift, "<code>");
                        shift += 5;
                        sb.Insert(l.Start + shift + l.Len - 1, "</code>");
                        shift += 6;
                        break;
                }
            }

            return sb.Replace("//br", "<div style=\"padding-bottom:5px\"></div>").ToString();
        }

        private static IEnumerable<Loc> Process(string source)
        {
            var buffer = new CharBuffer(source);
            var sb = new StringBuilder(source);

            while (buffer.MoveNext())
            {
                var c = buffer.GetCurrent();

                if ((c == KS || c == BS || c == IS) && IsStart(buffer))
                {
                    var type = c == KS ? ItemType.Keyword :
                               c == BS ? ItemType.Bold :
                               c == IS ? ItemType.Italic :
                               ItemType.Italic;
                    var a = buffer.SetAnchor();
                    var rc = c;

                    while (!EndSym(rc, buffer.GetNext()))
                        buffer.MoveNext();

                    if (buffer.GetNext() != rc)
                        buffer.BackToAnchor();
                    else
                    {
                        yield return new Loc(a, buffer.Offset - a + 2, type);
                        buffer.MoveNext();
                    }
                }
            }
        }

        private static bool EndSym(char sym, char c)
        {
            return c == sym || IsEol(c) || c == NIL;
        }

        private static bool IsStart(CharBuffer buffer)
        {
            if (buffer.Offset == 1)
                return true;

            var c = buffer.GetPrevious();
            return !Char.IsLetterOrDigit(c);
        }

        private static bool IsEol(char c)
        {
            return c == CR || c == LF;
        }
    }

  
}
