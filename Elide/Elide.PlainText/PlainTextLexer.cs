using System;
using System.Collections.Generic;
using Elide.Scintilla;

namespace Elide.PlainText
{
    internal sealed class PlainTextLexer
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

        public PlainTextLexer()
        {
            
        }

        public IEnumerable<StyledToken> Parse(string source)
        {
            var buffer = new CharBuffer(source);
            var spacePos = 0;

            while (buffer.MoveNext())
            {
                var c = buffer.GetCurrent();
                
                if (c == SL && Hyperlinks)
                {
                    if (buffer.GetNext() == SL && buffer.GetPrevious() == DD && spacePos > -1)
                    {
                        var sp = spacePos > 0 ? spacePos + 1 : spacePos;
                        var ep = buffer.Offset - 2;
                        var str = buffer.GetString(sp, ep - sp + 1);

                        while (!IsEmptyOrBrace(buffer.GetNext()))
                            buffer.MoveNext();                        

                        yield return new StyledToken(sp, buffer.Offset - sp + 1, TextStyle.Hyperlink);

                        spacePos = buffer.Offset;
                    }
                }
                else if (c == BS && Bold && IsStart(buffer))
                {
                    var a = buffer.SetAnchor();

                    while (!IsEmpty(buffer.GetNext()))
                        buffer.MoveNext();

                    if (buffer.GetCurrent() != BS)
                        buffer.BackToAnchor();
                    else
                        yield return new StyledToken(a, buffer.Offset - a + 1, TextStyle.Style1);
                }
                else if (c == IS && Italic && IsStart(buffer))
                {
                    var a = buffer.SetAnchor();

                    while (!IsEmpty(buffer.GetNext()))
                        buffer.MoveNext();

                    if (buffer.GetCurrent() != IS)
                        buffer.BackToAnchor();
                    else
                        yield return new StyledToken(a, buffer.Offset - a + 1, TextStyle.Style2);
                }
                else if (IsSep(c))
                    spacePos = buffer.Offset;
            }
        }

        private bool IsStart(CharBuffer buffer)
        {
            if (buffer.Offset == 1)
                return true;

            var c = buffer.GetPrevious();
            return IsEmpty(c);
        }
        
        private bool IsEol(char c)
        {
            return c == CR || c == LF;
        }
        
        private bool IsSpace(char c)
        {
            return c == SP || c == TB || c == NIL;
        }

        private bool IsSep(char c)
        {
            return IsSpace(c) || c == '=' || c == '(' || c == '<' || c == '[';
        }

        private bool IsCloseBrace(char c)
        {
            return c == ')' || c == '>' || c == ']';
        }
        
        private bool IsEmpty(char c)
        {
            return IsSpace(c) || IsEol(c);
        }

        private bool IsEmptyOrBrace(char c)
        {
            return IsSpace(c) || IsEol(c) || IsCloseBrace(c);
        }

        public bool Hyperlinks { get; set; }

        public bool Bold { get; set; }

        public bool Italic { get; set; }
    }
}