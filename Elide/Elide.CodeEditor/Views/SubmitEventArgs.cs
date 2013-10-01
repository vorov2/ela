using System;

namespace Elide.CodeEditor.Views
{
	public sealed class SubmitEventArgs : EventArgs
	{
		public SubmitEventArgs(string text)
		{
			Text = text;
		}
		
        public string Text { get; private set; }
	}
}
