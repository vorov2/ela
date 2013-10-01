using System;

namespace Elide.Scintilla.ObjectModel
{
    internal struct FoldRegion
    {
        public FoldRegion(int level, int endLine)
        {
            Level = level;
            EndLine = endLine;
        }
        
        internal readonly int Level;

        internal readonly int EndLine;
    }
}
