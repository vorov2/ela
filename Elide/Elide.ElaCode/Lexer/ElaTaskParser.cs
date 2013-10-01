using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Elide.CodeEditor.Views;

namespace Elide.ElaCode.Lexer
{
    public sealed class ElaTaskParser
    {
        public IEnumerable<TaskItem> Parse(string source)
        {
            if (String.IsNullOrEmpty(source))
                return new List<TaskItem>();

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(source)))
            {
                var s = new Scanner(ms);
                var p = new Parser(s) { ProcessTasks = true };
                p.Parse();
                return p.Tasks;
            }
        }
    }
}
