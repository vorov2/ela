using System;

namespace Ela.Debug
{
	public sealed class CallFrame
	{
		#region Construction
		private const string FORMAT_NAM = "{0}.{1}";
		private const string FORMAT_LNG = "\tin {0} at line: {1}, col: {2}";
		private const string FORMAT_SHT = "\tin {0} at offset: {1}";

		internal CallFrame(bool global, string moduleName, string name, int offset, LineSym lineSym)
		{
			Global = global;
			Name = name;
			ModuleName = moduleName;
			Offset = offset;
			LinePragma = lineSym;
		}
		#endregion


		#region Methods
		public override string ToString()
		{
			return LinePragma != null ?
				String.Format(FORMAT_LNG, GetFullName(), LinePragma.Line, LinePragma.Column) :
				String.Format(FORMAT_SHT, GetFullName(), Offset);
		}


		private string GetFullName()
		{
			return Name != null ? String.Format(FORMAT_NAM, ModuleName, Name) : ModuleName;
		}
		#endregion


		#region Properties
		public bool Global { get; private set; }

		public string Name { get; private set; }

		public string ModuleName { get; private set; }

		public int Offset { get; private set; }

		public LineSym LinePragma { get; private set; }
		#endregion
	}
}