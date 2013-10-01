using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Elide.Scintilla;

namespace Elide.Environment.Configuration
{
    public static class StyleItemConfigExtensions
    {
        public static void UpdateStyles(this IEnumerable<StyleItemConfig> styles, ScintillaControl sci)
        {
            var def = styles.First(s => s.Type == "Default");
            def.ApplyStyle(sci);

            sci.ResetStyles();

            foreach (var s in styles.Where(t => t.Type != "Default"))
                s.ApplyStyle(sci);
        }

        public static void ApplyStyle(this StyleItemConfig s, ScintillaControl sci)
        {
            var style = (Style)Reflect.GetPropertyValue(sci.Styles, s.Type);

            if (s.FontName != null)
                style.Font = s.FontName;

            if (s.FontSize != 0)
                style.FontSize = s.FontSize;

            if (s.ForeColor != null)
                style.ForeColor = Color.FromKnownColor(s.ForeColor.Value);

            if (s.BackColor != null)
                style.BackColor = Color.FromKnownColor(s.BackColor.Value);

            if (s.Bold != null)
                style.Bold = s.Bold.Value;

            if (s.Italic != null)
                style.Italic = s.Italic.Value;

            if (s.Underline != null)
                style.Underline = s.Underline.Value;
        }
    }
}
