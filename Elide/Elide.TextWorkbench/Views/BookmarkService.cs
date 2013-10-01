using System;
using Elide.Core;
using Elide.Environment;
using Elide.Scintilla;
using Elide.TextEditor;

namespace Elide.TextWorkbench.Views
{
    public sealed class BookmarkService : Service, IBookmarkService
    {
        public BookmarkService()
        {

        }

        public void ToggleBookmark()
        {
            var sci = Scintilla();

            if (sci != null)
                sci.ToggleBookmark();
        }

        public void ClearBookmarks()
        {
            var sci = Scintilla();

            if (sci != null)
                sci.ClearBookmarks();
        }

        public void NextBookmark()
        {
            var sci = Scintilla();

            if (sci == null)
                return;

            if (!sci.NextBookmark())
            {
                var doc = App.GetService<IDocumentService>().GetNextDocument(App.Document(), typeof(TextDocument));

                if (doc != null)
                {
                    App.GetService<IDocumentService>().SetActiveDocument(doc);
                    sci.CaretPosition = 1;
                    NextBookmark();
                }
            }
        }

        public void PreviousBookmark()
        {
            var sci = Scintilla();

            if (sci == null)
                return;

            if (!sci.PreviousBookmark())
            {
                var doc = App.GetService<IDocumentService>().GetPreviousDocument(App.Document(), typeof(TextDocument));

                if (doc != null)
                {
                    App.GetService<IDocumentService>().SetActiveDocument(doc);
                    sci.CaretPosition = sci.GetTextLength() - 1;
                    PreviousBookmark();
                }
            }
        }

        public bool IsBookmarkServiceAvailable()
        {
            return Scintilla() != null;
        }

        internal ScintillaControl Scintilla()
        {
            var ed = App.Editor();
            
            if (ed != null)
                return ed.Control as ScintillaControl;

            return null;
        }
    }
}
