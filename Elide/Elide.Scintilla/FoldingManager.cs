using System;
using System.Linq;
using System.Collections.Generic;
using Elide.Scintilla.Internal;
using Elide.Scintilla.ObjectModel;

namespace Elide.Scintilla
{
    internal sealed class FoldingManager
    {
        private readonly EditorRef @ref;

        internal FoldingManager(EditorRef @ref)
        {
            this.@ref = @ref;
        }

        internal void Fold(Dictionary<Int32,FoldRegion> regions)
        {
            if (regions.Count == 0)
                return;

            var start = regions.Min(rv => rv.Key);
            var end = regions.Max(rv => rv.Value.EndLine);
            var lc =  @ref.Send(Sci.SCI_GETLINECOUNT);
            
            for (var i = start; i < end + 2; i++)
            {
                FoldRegion reg;

                if (regions.TryGetValue(i, out reg))
                {
                    //System.Diagnostics.Debug.WriteLine("sl=" + i + ";el=" + reg.EndLine + ";level=" + reg.Level);
                    ProcessRegion(ref i, 0, regions, reg);
                    i--;
                }
                else if (i < lc)
                    @ref.Send(Sci.SCI_SETFOLDLEVEL, i, 0 | Sci.SC_FOLDLEVELBASE);
            }
        }


        private void ProcessRegion(ref int pos, int prevLevel, Dictionary<Int32,FoldRegion> regions, FoldRegion reg)
        {
            @ref.Send(Sci.SCI_SETFOLDLEVEL, pos, prevLevel | Sci.SC_FOLDLEVELHEADERFLAG | Sci.SC_FOLDLEVELBASE);
            pos++;

            for (; pos < reg.EndLine; pos++)
            {
                FoldRegion child;

                if (regions.TryGetValue(pos, out child))
                    ProcessRegion(ref pos, reg.Level, regions, child);

                @ref.Send(Sci.SCI_SETFOLDLEVEL, pos, reg.Level | Sci.SC_FOLDLEVELBASE);
            }
        }
    }
}
