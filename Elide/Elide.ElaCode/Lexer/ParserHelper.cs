using System;
using System.Collections.Generic;
using Elide.CodeEditor.Views;
using Elide.Scintilla;

namespace Elide.ElaCode.Lexer
{
	partial class Parser
	{
		internal List<StyledToken> Markers = new List<StyledToken>();
        internal List<TaskItem> Tasks = new List<TaskItem>();

        internal bool ProcessTasks { get; set; }

		internal int ErrorCount { get; set; }

		internal int Add(int position, int length, TextStyle style)
		{
			Markers.Add(new StyledToken(position, length, style));
			return Markers.Count - 1;
		}

        internal void TryAddTask(int position, int length)
        {
            if (!ProcessTasks)
                return;

            var b = this.scanner.buffer;
            var cmt = String.Empty;

            try
            {
                cmt = b.GetString(position, position + length);
                var trim = cmt.TrimStart();
                var idx = trim.IndexOf(' ');

                if (idx != -1)
                {
                    var tag = trim.Substring(0, idx).TrimEnd('.', ':', '-');
                    var task = GetTask(tag);
                    var taskStr = task.ToString().ToUpper();

                    if (task != TaskType.None)
                    {
                        var app = cmt.IndexOf(taskStr) + taskStr.Length;
                        Tasks.Add(new TaskItem(task, position + app, length - app));
                    }
                }
            }
            catch { }
        }

		internal void Patch(int index, int length)
		{
            Markers[index] = new StyledToken(Markers[index].Position, length, Markers[index].StyleKey); 
		}

        internal TaskType GetTask(string val)
        {
            return val == "TODO" ? TaskType.Todo :
                val == "HACK" ? TaskType.Hack :
                val == "FIXME" ? TaskType.Fixme :
                val == "UNDONE" ? TaskType.Undone :
                TaskType.None;
        }
	}
}
