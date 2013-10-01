using System;
using System.IO;

namespace Ela.Parsing
{
	public interface IElaParser
	{
		ParserResult Parse(string source);
		
		ParserResult Parse(FileInfo file);
	}
}
