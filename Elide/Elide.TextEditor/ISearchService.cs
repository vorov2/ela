using System;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla.ObjectModel;

namespace Elide.TextEditor
{
    public interface ISearchService : IService
    {
        void ShowSearchDialog();

        void ShowSearchReplaceDialog();

        void Search(string text, Document doc, SearchFlags flags, SearchScope scope);

        void SearchAll(string text, Document doc, SearchFlags flags, SearchScope scope);

        void Replace(string textToFind, string textToReplace, Document doc, SearchFlags flags, SearchScope scope);

        void ReplaceAll(string text, string textToReplace, Document doc, SearchFlags flags, SearchScope scope);

        void SearchNext();

        bool CanContinueSearch();
    }
}
