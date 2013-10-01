using System;
using System.IO;

namespace Ela.Linking
{
	public abstract class ObjectFile
	{
		private const int VERSION = 26;

		protected ObjectFile(FileInfo file)
		{
			File = file;
		}

		public FileInfo File { get; private set; }

		public int Version { get { return VERSION; } }
	}
}
