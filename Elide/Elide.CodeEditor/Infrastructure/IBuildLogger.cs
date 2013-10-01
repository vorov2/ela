using System;
using System.Collections.Generic;
using Elide.CodeEditor.Views;

namespace Elide.CodeEditor.Infrastructure
{
    public interface IBuildLogger
    {
        void WriteBuildInfo(string name, Version version);

        void WriteBuildOptions(string format, params object[] args);

        void WriteMessages(IEnumerable<MessageItem> messages, Func<MessageItem,Boolean> verboseFilter);
    }
}
