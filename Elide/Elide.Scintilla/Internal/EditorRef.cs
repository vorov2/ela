using System;

namespace Elide.Scintilla.Internal
{
	internal class EditorRef
	{
		internal EditorRef(IntPtr handle)
		{
			Handle = handle;
		}

		internal IntPtr Handle { get; private set; }
	}
}
