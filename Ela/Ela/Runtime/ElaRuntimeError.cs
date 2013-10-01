using System;

namespace Ela.Runtime
{
	public enum ElaRuntimeError
	{
		None = 0,

		Unknown = 800,

		CallFailed = 801,

		NotAlgebraicType = 802,

		DivideByZero = 803,

		TuplesLength = 804,

		IndexOutOfRange = 805,

        ExpectedFunction = 806,

		InvalidTypeArgument = 807,

		InvalidType = 808,

		MatchFailed = 809,

		ConstructorSequenceError = 810,

		OperationNotSupported = 811,

		PrivateVariable = 812,

		TooManyParams = 813,

		TooFewParams = 814,

		InvalidFormat = 815,

		ImmutableStructure = 816,

		Cyclic = 817,

		BottomReached = 818,

		UnknownField = 819,

		NoOverload = 820,

		InvalidParameterType = 821,

		UnknownParameterType = 822,

		CallWithNoParams = 823,

		InvalidIndexType = 824,

        InvalidConstructor = 825,

		InvalidOp = 826,

        InvalidTypeCode = 827,

        InvalidConstructorCode = 828,

        NoContext = 829,

        ZeroContext = 830,

        UnableCreateConstructor = 831,


		UserCode = 999
	}
}
