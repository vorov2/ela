using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Elide.CodeEditor;
using Elide.CodeWorkbench.Images;
using Elide.Core;
using Elide.Environment;
using Elide.Forms;
using Elide.Scintilla;
using Elide.Scintilla.ObjectModel;

namespace Elide.CodeWorkbench
{
    public sealed class AutocompleteService : Service, IAutocompleteService
    {
        private Dictionary<ScintillaControl,Object> scis;

        private sealed class Comparer : IEqualityComparer<AutocompleteSymbol>
        {
            internal static readonly Comparer Instance = new Comparer();

            public bool Equals(AutocompleteSymbol x, AutocompleteSymbol y)
            {
                return x == y || (x != null && y != null && x.Name == y.Name);
            }

            public int GetHashCode(AutocompleteSymbol obj)
            {
                return obj != null ? obj.Name.GetHashCode() : 0;
            }
        }

        public AutocompleteService()
        {
            scis = new Dictionary<ScintillaControl,Object>();
        }

        public void ShowAutocomplete(IEnumerable<AutocompleteSymbol> symbols)
        {
            var ed = App.Editor();

            if (ed != null)
            {
                symbols = symbols.Distinct(Comparer.Instance).OrderBy(s => s.Name);
                var sci = ed.Control as ScintillaControl;

                if (sci != null)
                {
                    if (!scis.ContainsKey(sci))
                    {
                        sci.RegisterAutocompleteImage(1, Pixmap.FromBitmap((Bitmap)Bitmaps.Load<NS>("PixmapVariable")));
                        sci.RegisterAutocompleteImage(2, Pixmap.FromBitmap((Bitmap)Bitmaps.Load<NS>("PixmapModule")));
                        sci.RegisterAutocompleteImage(3, Pixmap.FromBitmap((Bitmap)Bitmaps.Load<NS>("PixmapMember")));
                        sci.RegisterAutocompleteImage(4, Pixmap.FromBitmap((Bitmap)Bitmaps.Load<NS>("PixmapType")));
                        sci.RegisterAutocompleteImage(5, Pixmap.FromBitmap((Bitmap)Bitmaps.Load<NS>("PixmapKeyword")));
                        scis.Add(sci, null);
                    }

                    var words = String.Join(" ", symbols.Select(SymbolToString).ToArray());
                    sci.ShowAutocompleteList(0, words);
                }
            }
        }

        private string SymbolToString(AutocompleteSymbol symbol)
        {
            return String.Format("{0}?{1}", symbol.Name,
                symbol.Type == AutocompleteSymbolType.Variable ? "1" :
                symbol.Type == AutocompleteSymbolType.Module ? "2" :
                symbol.Type == AutocompleteSymbolType.Member ? "3" :
                symbol.Type == AutocompleteSymbolType.Type ? "4" : 
                "5");
        }
    }
}
