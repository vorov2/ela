using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.Scintilla.ObjectModel
{
    public sealed class SearchResult
    {
        internal static readonly SearchResult NotFound = new SearchResult(-1, -1);

        internal SearchResult(int startPosition, int endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }

        public bool Found
        {
            get { return StartPosition > -1; }
        }

        public readonly int StartPosition;

        public readonly int EndPosition;
    }
}
