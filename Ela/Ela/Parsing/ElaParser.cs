using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ela.CodeModel;

namespace Ela.Parsing
{
    public sealed class ElaParser : IElaParser
    {
        public ElaParser()
        {
        
        }

        public ParserResult Parse(ModuleFileInfo file)
        {
            return Parse(new FileInfo(file.FullName));
        }

        public ParserResult Parse(FileInfo file)
        {
            var str = File.ReadAllText(file.FullName);
            return Parse(new StringBuffer(str));
        }

        public ParserResult Parse(SourceBuffer buffer)
        {
            var p = new Parser(new Scanner(buffer));
            p.Parse();
            return new ParserResult(p.Program, p.errors.ErrorList.Count == 0, p.errors.ErrorList);
        }
    }
}