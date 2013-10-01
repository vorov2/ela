using System;

namespace Elide.Scintilla
{
	public sealed class BookmarkEventArgs : EventArgs
	{
		internal BookmarkEventArgs(int line)
		{
			Line = line;
		}
		
		public int Line { get; private set; }
	}
}
