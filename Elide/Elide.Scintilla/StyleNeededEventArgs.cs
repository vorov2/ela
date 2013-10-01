using System;
using System.Collections.Generic;

namespace Elide.Scintilla
{
	public sealed class StyleNeededEventArgs : EventArgs
	{
		internal StyleNeededEventArgs(int startPosition, int endPosition, int offset, string text)
		{
			StartPosition = startPosition;
			EndPosition = endPosition;
			Text = text;
			Offset = offset;
			Items = new List<StyledToken>();
		}

		public void AddStyleItem(int position, int length, TextStyle styleKey)
		{
			Items.Add(new StyledToken(position + Offset, length, styleKey));
		}

		public int StartPosition { get; private set; }

		public int EndPosition { get; private set; }

		public string Text { get; private set; }

		internal int Offset { get; private set; }

		internal List<StyledToken> Items { get; private set; }
	}
}