using System;
using System.Drawing;

namespace Elide.Scintilla.Internal
{
	internal static class SciColor
	{
		internal static int ToScintillaColor(this Color @this)
		{
			return @this.R + (@this.G << 8) + (@this.B << 16);
		}

		internal static Color FromScintillaColor(int rgb)
		{
			return Color.FromArgb(0xff, rgb & 0xff, (rgb & 0xff00) >> 8, (rgb & 0xff0000) >> 16);
		}
	}
}
