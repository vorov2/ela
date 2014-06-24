using Elide.Core;
using Elide.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.CodeEditor.Infrastructure
{
    public interface ICodeParserService : IExtService
    {
        T RunParser<T>(string source, Document doc) where T : IAst;
    }
}
