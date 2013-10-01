using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Eladoc.Lexers
{
	public sealed class ElaLexer
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
