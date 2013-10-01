using System;
using System.Collections.Generic;
using Elide.CodeEditor.Views;
using Elide.Core;
using Elide.Environment.Views;
using Elide.CodeEditor;

namespace Elide.CodeWorkbench.Views
{
    public sealed class TaskListService : AbstractTaskListService
    {
        public TaskListService()
        {

        }

        public new ITaskProvider GetTaskProvider(CodeDocument doc)
        {
            return base.GetTaskProvider(doc);
        }
    }
}
