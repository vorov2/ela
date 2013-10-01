using System;
using Elide.Environment;
using Elide.Environment.Editors;

namespace Elide.TextEditor
{
    public interface ITextEditor : IEditor
    {
        bool Overtype { get; set; }

        int CurrentLine { get; }

        int CurrentColumn { get; }

        int SelectionStart { get; }

        int SelectionEnd { get; }

        int CaretPosition { get; }

        TextLocation GetLocationFromPosition(Document doc, int pos);

        void SetContent(Document doc, string text);

        string GetContent(Document doc);

        string GetContent(Document doc, int start, int length);

        string GetContent(Document doc, int lineNumber);

        void SelectText(int start, int length);

        void SelectText(int line, int col, int length);

        void ReplaceText(int start, int end, string text);

        void BeginUndoAction();

        void EndUndoAction();
    }
}
