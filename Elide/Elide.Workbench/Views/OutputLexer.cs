using System;
using System.Collections.Generic;
using Elide.Scintilla;

namespace Elide.Workbench.Views
{
    internal sealed class OutputLexer
    {
        private const char HS = '<';
        private const char HE = '>';
        private const char MM = '!';
        private const char SL = '/';

        private const char S1 = 'h';
        private const char S2 = 'e';
        private const char S3 = 'w';
        private const char S4 = 'i';
        
        private const char CR = '\r';
        private const char LF = '\n';
        private const char SP = ' ';
        private const char TB = '\t';
        private const char NIL = '\0';

        public OutputLexer()
        {

        }

        public IEnumerable<StyledToken> Parse(string source)
        {
            var buffer = new CharBuffer(source);

            while (buffer.MoveNext())
            {
                var c = buffer.GetCurrent();

                if (c == HS && buffer.GetNext() == MM)
                {
                    var ec = CheckStyle(buffer);

                    if (ec != NIL)
                    {
                        var sp = buffer.Offset;

                        for (;;)
                        {
                            if (!buffer.MoveNext())
                                break;

                            c = buffer.GetCurrent();

                            if (c == HS && IsEndStyle(buffer, ec))
                            {
                                var ts = ec == S1 ? TextStyle.MultilineStyle1 :
                                    ec == S2 ? TextStyle.MultilineStyle2 :
                                    ec == S3 ? TextStyle.MultilineStyle3 :
                                    ec == S4 ? TextStyle.MultilineStyle4 :
                                    TextStyle.Default;

                                yield return new StyledToken(sp, 5, TextStyle.Invisible);
                                yield return new StyledToken(buffer.Offset, 6, TextStyle.Invisible);
                                yield return new StyledToken(sp + 5, buffer.Offset - sp - 5, ts);

                                break;
                            }
                        }
                    }
                }
            }
        }

        private char CheckStyle(CharBuffer buffer)
        {
            return IsStyle(buffer, S1) ? S1 :
                IsStyle(buffer, S2) ? S2 :
                IsStyle(buffer, S3) ? S3 :
                IsStyle(buffer, S4) ? S4 :
                NIL;
        }

        private bool IsStyle(CharBuffer buffer, char c)
        {
            return buffer.Get(2) == c && buffer.Get(3) == MM && buffer.Get(4) == HE;
        }

        private bool IsEndStyle(CharBuffer buffer, char c)
        {
            return buffer.Get(1) == SL && buffer.Get(2) == MM && buffer.Get(3) == c &&
                buffer.Get(4) == MM && buffer.Get(5) == HE;
        }

        private bool IsEol(char c)
        {
            return c == CR || c == LF;
        }

        private bool IsSpace(char c)
        {
            return c == SP || c == TB || c == NIL;
        }
    }
}