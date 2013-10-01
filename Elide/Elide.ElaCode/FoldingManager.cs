using System;
using Elide.Scintilla;

namespace Elide.ElaCode
{
    internal sealed class FoldingManager
    {
        private readonly ScintillaControl sci;

        internal FoldingManager(ScintillaControl sci)
        {
            this.sci = sci;
        }

        internal void Fold(FoldNeededEventArgs e)
        {
            var lineCount = sci.LineCount;
            var firstLine = e.StartLine;
            var lastLine = e.EndLine;
            e.AddFoldRegion(0, firstLine, lastLine);
            
            var lastStart = -1;

            for (int line = firstLine; line < lastLine + 2; line++)
            {
                var li = sci.GetLineIndentation(line);
                var posLine = sci.GetPositionFromLine(line);
                var colEnd = sci.GetLineEndColumn(line);
                var style = sci.GetStyleAt(posLine);
                var cmt = style == TextStyle.MultilineStyle1 || style == TextStyle.MultilineStyle2 ||
                    style == TextStyle.Style6 || style == TextStyle.Style7;

                var hasLet = li == 0 && !cmt;
                
                if ((li == 0 || colEnd == li) && lastStart > -1 && line - lastStart > 1 && !cmt)
                {
                    e.AddFoldRegion(1, lastStart, line);
                    lastStart = -1;
                }
                else if (li == 0 || colEnd == li)
                    lastStart = -1;
                
                if (hasLet)
                    lastStart = line;

                if (line == lastLine + 1 && lastStart != -1 && lastLine < lineCount)
                    lastLine++;
            }

            if (lastStart > -1 && lastLine == lineCount - 1)
                e.AddFoldRegion(1, lastStart, lineCount - 1);
        }
    }
}
