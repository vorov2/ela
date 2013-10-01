using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Ela.Debug;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime
{
	public sealed class ElaCodeException : ElaException
	{
		private const string EXC_FORMAT = "{0}\r\n{1}";

		internal ElaCodeException(string message, ElaRuntimeError error, FileInfo file, int line, int col,
			IEnumerable<CallFrame> callStack, ElaError errObj, Exception innerException)
            : base(message, innerException)
		{
			Error = new ElaMessage(message, MessageType.Error, (Int32)error, line, col);
			Error.File = file;
			CallStack = callStack;
			ErrorObject = errObj;
		}
		
		public override string ToString()
		{
			return String.Format(EXC_FORMAT,
				Error != null ? Error.ToString() : String.Empty,
				FormatCallStack(CallStack));
		}
        
		private string FormatCallStack(IEnumerable<CallFrame> points)
		{
			var sb = new StringBuilder();

			foreach (var s in points)
				sb.AppendLine(s.ToString());

			return sb.ToString();
		}

		public ElaMessage Error { get; private set; }

		public IEnumerable<CallFrame> CallStack { get; private set; }

		internal ElaError ErrorObject { get; private set; }
	}
}