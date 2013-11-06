using System;
using System.IO;
using System.Collections.Generic;

namespace Ela.Compilation
{
	public sealed class ModuleReference
	{
		#region Construction
		private const string FORMAT = "{0}[{1}]";

        internal ModuleReference(CodeFrame parent, string moduleName) : this(parent, moduleName, null, null, 0, 0, false, 0)
		{

		}

		internal ModuleReference(string moduleName) : this(null, moduleName, null, null, 0, 0, false, 0)
		{

		}


		internal ModuleReference(CodeFrame parent, string moduleName, string dllName, string[] path, int line, int column, bool requireQualified, int logicalHandle)
		{
			ModuleName = moduleName;
			DllName = dllName;
			Path = path ?? new string[0];
			Line = line;
			Column = column;
            RequireQuailified = requireQualified;
            LogicalHandle = logicalHandle;
            Parent = parent;
		}
		#endregion


		#region Methods
		public override string ToString()
		{
			return DllName == null ? BuildFullName(ModuleName) : 
				BuildFullName(String.Format(FORMAT, ModuleName, DllName));
		}


		public string GetPath()
		{
			return String.Join(System.IO.Path.DirectorySeparatorChar.ToString(), Path);
		}


		private string BuildFullName(string name)
		{
			return Path.Length == 0 ? name :
				String.Concat(GetPath(), System.IO.Path.DirectorySeparatorChar, name);
		}
		#endregion


		#region Fields
		public readonly string ModuleName;

		public readonly string DllName;

		public readonly int Line;

		public readonly int Column;

        public readonly bool RequireQuailified;

		internal readonly string[] Path;
		#endregion


		#region Properties
		internal bool NoPrelude { get; set; }

        internal int LogicalHandle { get; set; }

        internal CodeFrame Parent { get; private set; }
		#endregion
	}
}
