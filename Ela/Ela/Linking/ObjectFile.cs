using System;
using System.IO;

namespace Ela.Linking
{
	public abstract class ObjectFile
	{
		private const int VERSION = 27;

		protected ObjectFile(ModuleFileInfo file)
		{
			File = file;
		}

        public ModuleFileInfo File { get; private set; }

		public int Version { get { return VERSION; } }
	}
}
