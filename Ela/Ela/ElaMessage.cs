using System;
using System.IO;

namespace Ela
{
	public class ElaMessage
	{
		private const string ERR_FORMAT = "{0}({1},{2}): {3} ELA{4}: {5}";

		internal ElaMessage(string errorMessage, MessageType type, int code, int line, int column)
		{
			Message = errorMessage;
			Code = code;
			Line = line;
			Column = column;
			Type = type;
		}
		
        public override string ToString()
		{
			return String.Format(ERR_FORMAT, GetFileName(), Line, Column, Type.ToString(), Code, Message);
		}

		private string GetFileName()
		{
			return File == null ? String.Empty :
				File == null ? "<memory>" : File.FullName;
		}

		public FileInfo File { get; internal set; }

		public MessageType Type { get; private set; }
		
		public int Code { get; private set; }

		public string Message { get; private set; }

		public int Line { get; private set; }

		public int Column { get; private set; }
	}
}
