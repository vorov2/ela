using System;
using Ela.CodeModel;

namespace Ela.Compilation
{
	public struct ScopeVar
	{
		#region Construction
		internal static readonly ScopeVar Empty = new ScopeVar(ElaVariableFlags.None, -1, -1);

		internal ScopeVar(int address) : this(ElaVariableFlags.None, address, -1)
		{

		}
			
		internal ScopeVar(ElaVariableFlags flags, int address, int data)
		{
			Address = address;
			Flags = flags;
			Data = data;
		}
		#endregion


		#region Methods
		public bool IsEmpty()
		{
			return Address == -1;
		}
		#endregion


		#region Fields
		internal int Address;

		internal readonly ElaVariableFlags Flags;

		internal readonly int Data;
		#endregion


		#region Properties
		public int VariableAddress { get { return Address; } }

		public ElaVariableFlags VariableFlags { get { return Flags; } }

		public int VariableData { get { return Data; } }
		#endregion
	}
}
