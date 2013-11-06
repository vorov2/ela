using System;

namespace Ela.Debug
{
	public sealed class ScopeSym
	{
		#region Construction
		internal ScopeSym(int index, int parentIndex, int startOffset, int startLine, int startColumn)
		{
			Index = index;
			ParentIndex = parentIndex;
			StartOffset = startOffset;
            StartLine = startLine;
            StartColumn = startColumn;
		}
		#endregion


		#region Properties
		public int Index { get; private set; }

		public int ParentIndex { get; private set; }

		public int StartOffset { get; private set; }

		public int EndOffset { get; internal set; }

        public int StartLine { get; private set; }

        public int StartColumn { get; private set; }

        public int EndLine { get; internal set; }

        public int EndColumn { get; internal set; }
		#endregion
	}
}
