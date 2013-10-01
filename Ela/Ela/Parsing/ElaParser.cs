using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ela.CodeModel;

namespace Ela.Parsing
{
	public sealed class ElaParser : IElaParser
	{
		#region Construction
		public ElaParser()
		{
		
		}
		#endregion


		#region Methods
		public ParserResult Parse(string source)
		{
			return InternalParse(source);
		}


		public ParserResult Parse(FileInfo file)
		{
			using (var fs = file.Open(FileMode.Open, FileAccess.Read))
				return InternalParse(fs);
		}


		public ParserResult Parse(Stream stream)
		{
			return InternalParse(stream);
		}


		private ParserResult InternalParse(string source)
		{
			using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
				return InternalParse(ms);
		}


		private ParserResult InternalParse(Stream stream)
		{
			var p = new Parser(new Scanner(stream));
			p.Parse();
			return new ParserResult(p.Program, p.errors.ErrorList.Count == 0, p.errors.ErrorList);
		}
		#endregion
	}
}