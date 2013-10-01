using System;
using System.Runtime.InteropServices;

namespace Elide.Scintilla.Internal
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct ScNotification
	{
		public NotifyHeader NmHdr;
		
		public int Position;	// SCN_STYLENEEDED, SCN_MODIFIED, SCN_DWELLSTART, SCN_DWELLEND
		
		public int Ch;		// SCN_CHARADDED, SCN_KEY
		
		public int Modifiers;	// SCN_KEY
		
		public int ModificationTypes;	// SCN_MODIFIED
		
		public IntPtr Text;	// SCN_MODIFIED
		
		public int Length;		// SCN_MODIFIED
		
		public int LinesAdded;	// SCN_MODIFIED
		
		public int Message;	// SCN_MACRORECORD
		
		public IntPtr WParam;	// SCN_MACRORECORD
		
		public IntPtr LParam;	// SCN_MACRORECORD
		
		public int Line;		// SCN_MODIFIED
		
		public int FoldLevelNow;	// SCN_MODIFIED
		
		public int FoldLevelPrev;	// SCN_MODIFIED
		
		public int Margin;		// SCN_MARGINCLICK
		
		public int ListType;	// SCN_USERLISTSELECTION
		
		public int X;			// SCN_DWELLSTART, SCN_DWELLEND
		
		public int Y;		// SCN_DWELLSTART, SCN_DWELLEND
	}
}
