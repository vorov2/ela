using System;
using System.Collections.Generic;
using System.Drawing;

namespace Elide.Forms
{
    public static class FontHelper
    {
        public static readonly IEnumerable<String> MonospaceFonts = new List<String>();

        static FontHelper()
        {
            using (var g = Graphics.FromImage(new Bitmap(50, 20)))
            {
                foreach (var f in FontFamily.Families)
                    if (!String.IsNullOrEmpty(f.Name) && IsFixedWidth(g, f.Name))
                        ((List<String>)MonospaceFonts).Add(f.Name);                    
            }
        }

        private static bool IsFixedWidth(Graphics g, string font)
        {
            try
            {
                using (var f = new Font(font.Replace("(Default) ", String.Empty), 12))
                    return g.MeasureString("M", f).Width == g.MeasureString(".", f).Width;
            }
            catch
            {
                return false;
            }
        }
    }
}
