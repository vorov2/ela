using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Elide.Scintilla.ObjectModel;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla
{
    public sealed class SearchManager : IDisposable
    {
        private readonly string textDocument;
        private BasicScintillaControl scintilla;

        public SearchManager(string textDocument)
        {
            this.textDocument = textDocument;
            this.scintilla = new BasicScintillaControl();
            this.scintilla.SetText(textDocument);
        }

        public void Dispose()
        {
            scintilla.SetText(String.Empty);
            scintilla.Dispose();            
        }

        public SearchResult Search(SearchFlags flags, string text, int startPosition, int endPosition)
        {
            return InternalSearch(flags, text, startPosition, endPosition);
        }

        private unsafe SearchResult InternalSearch(SearchFlags flags, string text, int startPosition, int endPosition)
        {
            fixed (byte* pt = Encoding.UTF8.GetBytes(text))
            {
                var pf = new TextToFind();
                pf.lpstrText = (IntPtr)pt;
                pf.chrg.Min = startPosition;
                pf.chrg.Max = endPosition <= 0 ? startPosition + scintilla.Ref.Send(Sci.SCI_GETTEXTLENGTH) : endPosition;

                var res = RunSearch(flags, ref pf);

                if (res == -1)
                    return SearchResult.NotFound;
                else
                    return new SearchResult(pf.chrgText.Min, pf.chrgText.Max);
            }
        }

        private unsafe int RunSearch(SearchFlags flags, ref TextToFind ttf)
        {
            fixed (TextToFind* pff = &ttf)
                return scintilla.Ref.Send(Sci.SCI_FINDTEXT, (Int32)flags, (IntPtr)pff);
        }
    }
}
