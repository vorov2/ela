using System;

namespace Elide.Scintilla
{
	public sealed class LinkClickedEventArgs : EventArgs
	{
		internal LinkClickedEventArgs(string link)
		{
			Link = link;
		}
		
		public string Link { get; private set; }
	}
}