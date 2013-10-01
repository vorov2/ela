using System;
using System.Collections.Generic;
using Elide.Core;

namespace Elide.TextEditor
{
    public interface IResultGridService : IService
    {
        void AddItems(IEnumerable<ResultItem> items);

        void ClearItems();
    }
}
