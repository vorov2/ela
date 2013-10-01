using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.CodeEditor.Views
{
    public interface IErrorListService : IService
    {
        void ShowMessages(IEnumerable<MessageItem> messages);

        void Clear();
    }
}
