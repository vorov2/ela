using System;

namespace Ela.Debug
{
	public sealed class ScopeSym
	{
		#region Construction
		internal ScopeSym(int index, int parentIndex, int startOffset, int startLine, int startColumn, bool runtimeScope)
		{
			Index = index;
			ParentIndex = parentIndex;
			StartOffset = startOffset;
            StartLine = startLine;
            StartColumn = startColumn;
            RuntimeScope = runtimeScope;
		}
		#endregion

        #region Methods
        public override string ToString()
        {
            return String.Format("Index={0};Parent={1};Start={2};End={3};RuntimScope={4}",
                Index, ParentIndex, StartOffset, EndOffset, RuntimeScope);
        }
        #endregion

        #region Properties
        public bool RuntimeScope { get; private set; }

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
