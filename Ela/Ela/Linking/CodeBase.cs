using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Ela.Linking
{
	public sealed class CodeBase
	{
		#region Construction
		public CodeBase()
		{
			Directories = new List<DirectoryInfo>();
			LookupStartupDirectory = true;
		}
        #endregion


		#region Properties
		public List<DirectoryInfo> Directories { get; private set; }

		public bool LookupStartupDirectory { get; set; }
		#endregion
	}
}
