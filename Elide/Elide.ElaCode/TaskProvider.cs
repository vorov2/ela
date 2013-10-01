using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.CodeEditor.Views;
using Elide.CodeEditor;
using Elide.ElaCode.Lexer;
using Elide.Core;
using Elide.Environment;

namespace Elide.ElaCode
{
    public sealed class TaskProvider : ITaskProvider
    {
        public TaskProvider()
        {

        }

        public IEnumerable<TaskItem> GetTasks(CodeDocument doc)
        {
            var tp = new ElaTaskParser();
            return tp.Parse(doc.GetContent());
        }

        public IApp App { get; set; }
    }
}
