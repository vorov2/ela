using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Elide.Scintilla;
using Elide.CodeEditor.Infrastructure;

namespace Elide.ElaCode.Lexer
{
	public sealed class ElaLexer : ICodeLexer
	{
		public IEnumerable<StyledToken> Parse(string source)
		{
            if (String.IsNullOrEmpty(source))
                return new List<StyledToken>();

			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
			{
				var s = new Scanner(ms);
				var p = new Parser(s);
				p.Parse();
				return p.Markers;
			}
		}
	}
}
