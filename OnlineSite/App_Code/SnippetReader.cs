using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using Eladoc.Lexers;
using System.Text;
using System.Collections.Generic;

public class SnippetReader
{
    public string ReadSnippet(string file)
    {
        using (StreamReader fs = new StreamReader(file))
        {
            string src = fs.ReadToEnd();
            return Lex(src);
        }
    }

    string Lex(string src)
    {
        ElaLexer lexer = new ElaLexer();
        IEnumerable<StyledToken> tokens = lexer.Parse(src);

        if (tokens != null)
        {
            StringBuilder sb = new StringBuilder(src);
            int offset = 0;

            foreach (StyledToken t in tokens)
            {
                string span = String.Format("<span class='lexer_{0}'>", t.StyleKey);
                string endSpan = "</span>";
                sb.Insert(t.Position + offset, span);
                offset += span.Length;
                sb.Insert(t.Position + offset + t.Length, endSpan);
                offset += endSpan.Length;
            }

            return sb.ToString();
        }

        return src;
    }
}
