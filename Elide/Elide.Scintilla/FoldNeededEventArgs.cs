using System;
using System.Collections.Generic;
using Elide.Scintilla.ObjectModel;

namespace Elide.Scintilla
{
    public sealed class FoldNeededEventArgs : EventArgs
    {
        internal FoldNeededEventArgs(int startLine, int endLine)
        {
            Regions = new Dictionary<Int32,FoldRegion>();
            StartLine = startLine;
            EndLine = endLine;
        }

        public void AddFoldRegion(int level, int startLine, int endLine)
        {
            Regions.Remove(startLine);
            Regions.Add(startLine, new FoldRegion(level, endLine));
        }

        public int StartLine { get; private set; }

        public int EndLine { get; private set; }
        
        internal Dictionary<Int32,FoldRegion> Regions { get; private set; }
    }
}
