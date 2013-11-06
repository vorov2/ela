using System;

namespace Ela.Debug
{
	public sealed class VarSym
	{
		#region Construction
		internal VarSym(string name, int address, int offset, int scope, int flags, int data)
		{
			Name = name;
			Address = address;
			Offset = offset;
			Scope = scope;
            Flags = flags;
            Data = data;
		}
		#endregion


        #region Methods
        public override string ToString()
        {
            return Name;
        }
        #endregion


        #region Properties
        public string Name { get; private set; }

		public int Address { get; private set; }

		public int Offset { get; private set; }

		public int Scope { get; private set; }

        public int Flags { get; internal set; }

        public int Data { get; internal set; }
		#endregion
	}
}
