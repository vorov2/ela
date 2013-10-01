using System;

namespace Ela.Parsing
{
	public enum ElaParserError
	{
		None = 0,

		InvalidSyntax = 1,

        InvaliDoEnd = 2,

		InvalidIntegerSyntax = 3,

		InvalidRealSyntax = 4,

		InvalidEscapeCode = 5,

		InvalidVariantLiteral = 6,

		InvalidFunctionDeclaration = 7,

		InvalidFunctionSignature = 8,

		ComprehensionOpInvalidOperand = 9,

		TabNotAllowed = 10,

		IncorrectIndentation = 11,

		InvalidSyntaxUnexpectedSymbol = 12,

		InvalidIndentationUnexpectedSymbol = 13,

        UnknownAttribute = 14,

        InvalidAttribute = 15,

        InvalidAttributeWhere = 16,


		ExpectedToken = 100,

		ExpectedEof = 101,

		ExpectedIdentifierToken = 102,

		ExpectedIntToken = 103,

		ExpectedRealToken = 104,

		ExpectedStringToken = 105,

		ExpectedCharToken = 106,

		ExpectedOperatorToken = 107,

		ExpectedPipe = 108,

		ExpectedBooleanToken = 109,

		// = 110,

		ExpectedCurlyBrace = 111,

		ExpectedArrow = 112,

		ExpectedKeywordIn = 113,

		ExpectedKeywordImport = 114,

		ExpectedKeywordType = 115,

		ExpectedKeywordWith = 116,

		ExpectedKeywordInstance = 117,

		ExpectedKeywordClass = 118,

		ExpectedKeywordMatch = 119,

		ExpectedSquareBrace = 120,

		ExpectedComprehensionSlash = 121,

		ExpectedIsOperator = 122,

		ExpectedKeywordLet = 123,

        ExpectedKeywordOpentype = 124,

		ExpectedKeywordOpen = 125,

		// = 126,

		// = 127,

		// = 128,

		// = 129,

		// = 130,

		// = 131,

		// = 132,

		// = 133,
		
		// = 134,

		ExpectedKeywordIf = 135,

		ExpectedKeywordElse = 136,

		// = 137,

		// = 138,

		// = 139,

		// = 140,

		// = 141,

		ExpectedKeywordRaise = 142,

		// = 143,

		// = 144,

		// = 145,

		// = 146,

		ExpectedLambda = 147,

		ExpectedDot = 148,

		ExpectedKeywordThen = 149,

		ExpectedKeywordTry = 150,

		ExpectedKeywordFail = 151,

		ExpectedKeywordWhere = 152,

		// = 153,

		// = 154,

		// = 155,

		ExpectedKeywordAsAmp = 156,

		// = 157,



		InvalidProduction = 200,

		InvalidLiteral = 201,

		InvalidPrimitive = 202,

		InvalidExpression = 203,

		InvalidTypeCheck = 204,

		InvalidDoBlock = 205,

		InvalidOperator = 206,

		// = 207,

		// = 208,

		// = 209,

		InvalidRoot = 210,

		// = 211,

		// = 212,

		InvalidIf = 213,

		// = 214,

		// = 215,

		InvalidLazy = 216,

		InvalidFail = 217,

		InvalidTry = 218,

		InvalidClassInstance = 219,

		InvalidNewType = 220,

		InvalidTypeClass = 221,

		// = 222,

		InvalidSimpleExpression = 223,

		InvalidVariant = 224,

		InvalidVariableReference = 225,

		InvalidTuple = 226,

		InvalidGrouping = 227,

		InvalidMemberAccess = 228,

		// = 229,

		// = 230,

		InvalidMatch = 231,

		// = 232,

		// = 233,

		// = 234,

		// = 235,

		// = 236,

		// = 237,

		// = 238,

		// = 239,

		// = 240,

		// = 241,

		// = 242,

		InvalidRecord = 243,

		InvalidRange = 244,

		InvalidList = 245,

		InvalidParamList = 246,

		InvalidOperation = 247,

		InvalidBinding = 248,

		InvalidWhereBinding = 249,

		InvalidLambda = 250,

		InvalidInclude = 251,

        InvalidHeader = 252,

		// = 253,

		// = 254,

        InvalidAs = 255,

		// = 256,

		// = 257,

		// = 258,

		// = 259,

		// = 260,

		InvalidInfix = 261,

		// = 262,

		// = 263,

		// = 264,

		// = 265,

		InvalidApplication = 266,

		// = 267,

		InvalidComprehension = 268,

		InvalidGuard = 269
	}
}
