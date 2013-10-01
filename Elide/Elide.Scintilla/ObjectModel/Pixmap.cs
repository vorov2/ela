using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Elide.Scintilla.ObjectModel
{
	public sealed class Pixmap
	{
		private const string XPM_FORMAT = 
@"/* XPM */
static char* image[] = {{
""{0} {1} {2} 1"",
{3}
{4}
}};";
		private const string COLOR_FORMAT = "\"{0} c {1}\",\r\n";
		private const string CHARS = ",<.>/?;:'[{]}~!@#$%^&*()_`-+=1234567890qwertyuiopasdfghjklzxcvbnmQWERTYUIOPASDFGHJKLZXCVBNM|";
		private const string TRANSPARENT = "None";
		private const char QUOTE = '"';
		private const char COMMA = ',';
		private const string CRLF = "\r\n";

		private Bitmap bmp;
		private string xpm;
		private object syncRoot = new Object();

		private Pixmap(Bitmap bmp)
		{
			this.bmp = bmp;
		}

		public static Pixmap FromBitmap(Bitmap bmp)
		{
			return new Pixmap(bmp);
		}

        internal static string FromResource(string key)
        {
            return new Pixmap((Bitmap)Bitmaps.Load(key)).GetPixmap();
        }

		public string GetPixmap()
		{
			if (xpm == null)
				lock (syncRoot)
					if (xpm == null)
					{
						using (bmp)
							xpm = ConvertBitmap(bmp);
						bmp = null;
					}

			return xpm;
		}

		private string ConvertBitmap(Bitmap bmp)
		{
			var colorBuilder = new StringBuilder();
			var mapBuilder = new StringBuilder();
			var colors = new Dictionary<Color,Char>();
			var colIndex = 0;

			for (var y = 0; y < bmp.Height; y++)
			{
				mapBuilder.Append(QUOTE);

				for (var x = 0; x < bmp.Width; x++)
				{
					var c = bmp.GetPixel(x, y);
					var ch = default(Char);

					if (!colors.TryGetValue(c, out ch))
					{
						var white = c.R == 255 && c.G == 255 && c.B == 255;

                        if (!white && colIndex > CHARS.Length - 1)
                            throw new Exception(String.Format("Image can have up to {0} colors", CHARS.Length));

						ch = white ? ' ' : CHARS[colIndex];
						colors.Add(c, ch);
						colorBuilder.AppendFormat(COLOR_FORMAT, ch,
							white ? TRANSPARENT : ColorTranslator.ToHtml(c));
                        colIndex++;
					}

					mapBuilder.Append(ch);
				}

				mapBuilder.Append(QUOTE);

				if (y < bmp.Height - 1)
				{
					mapBuilder.Append(COMMA);
					mapBuilder.Append(CRLF);
				}
			}

			return String.Format(XPM_FORMAT, bmp.Width, bmp.Height, colors.Count,
				colorBuilder.ToString(), mapBuilder.ToString());
		}

		public static explicit operator String(Pixmap xpm)
		{
			return xpm.GetPixmap();
		}
	}
}
