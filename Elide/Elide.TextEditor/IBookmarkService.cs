using System;
using Elide.Core;

namespace Elide.TextEditor
{
    public interface IBookmarkService : IService
    {
        void ToggleBookmark();

        void ClearBookmarks();

        void PreviousBookmark();

        void NextBookmark();

        bool IsBookmarkServiceAvailable();
    }
}
