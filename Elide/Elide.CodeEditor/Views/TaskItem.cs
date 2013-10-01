using System;

namespace Elide.CodeEditor.Views
{
    public struct TaskItem
    {
        public readonly TaskType Type;
        public readonly int Position;
        public readonly int Length;

        public TaskItem(TaskType type, int position, int length)
        {
            Type = type;
            Position = position;
            Length = length;
        }
    }
}
