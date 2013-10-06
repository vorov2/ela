using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace Ela.Debug
{
	public sealed class CallStack : IEnumerable<CallFrame>
	{
		#region Construction
		private List<CallFrame> frames;

		internal CallStack(FileInfo currentFile, int currentLine, int currentCol, List<CallFrame> frames)
		{
			File = currentFile;
			Line = currentLine;
			Column = currentCol;
			this.frames = frames;
		}
		#endregion


		#region Methods
		public IEnumerator<CallFrame> GetEnumerator()
		{
			return frames.GetEnumerator();
		}


		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
		#endregion


		#region Properties
		public CallFrame this[int index]
		{
			get { return frames[index]; }
		}


		public int FrameCount
		{
			get { return frames.Count; }
		}


		public FileInfo File { get; private set; }


		public int Line { get; private set; }


		public int Column { get; private set; }
		#endregion
	}
}
