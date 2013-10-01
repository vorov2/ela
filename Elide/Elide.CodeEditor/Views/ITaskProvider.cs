using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    public interface ITaskProvider
    {
        IEnumerable<TaskItem> GetTasks(CodeDocument doc);

        IApp App { get; set; }
    }
}
