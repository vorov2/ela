using System;
using System.IO;

namespace Ela.Parsing
{
    public interface IElaParser
    {
        ParserResult Parse(SourceBuffer buffer);

        ParserResult Parse(FileInfo file);
    }
}
