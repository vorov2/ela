using System;

namespace Ela.CodeModel
{
	[Flags]
	public enum ElaVariableFlags
	{
		None = 0x00,

		External = 1 << 0,

        Private = 1 << 1,

        Module = 1 << 2,

        Function = 1 << 3,

        ObjectLiteral = 1 << 4,

        SpecialName = 1 << 5,

        Parameter = 1 << 6,

        Builtin = 1 << 7,

        Context = 1 << 8,

        NoInit = 1 << 9,

        Qualified = 1 << 10,

        ClassFun = 1 << 11,

        TypeFun = 1 << 12,

        ClosedType = 1 << 13,

        Polyadric = 1 << 14,

        Self = 1 << 15,

        PartiallyApplied = 1 << 16, //??

        Clean = 1 << 17
	}
}
