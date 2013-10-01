using System;

namespace Ela.Compilation
{
	public enum ElaCompilerError
	{
		None = 0,


        PrivateNameInModule = 300,

		UndefinedNameInModule = 301,

		UndefinedName = 302,

		UndefinedType = 303,

		PlaceholderNotValid = 304,

		TypeAlreadyDeclared = 305,

		InvalidExpression = 306,

        ElseMissing = 307,

		BangPatternNotValid = 308,

		InvalidPattern = 309,

		// = 310,

		// = 311,

		InvalidTypeDefinition = 312,

        PrivateOnlyGlobal = 313,

		InvalidConstructorParameter = 314,

        InvalidBuiltinBinding = 315,

		InvalidConstructor = 316,

        ClassAlreadyDeclared = 317,

        UnknownClass = 318,

        MemberInvalid = 319,

        MemberNoPatterns = 320,

        MemberNotAll = 321,

        InvalidBuiltinClass = 322,

        InvalidBuiltinClassDefinition = 323,

        NoHindingSameScope = 324,

        InvalidQualident = 325,

        InvalidMemberSignature = 326,

        ExtendsNoDefinition = 327,

        InvalidProgram = 328,

        TooManyErrors = 329,


        PatternsTooFew = 330,

        PatternsTooMany = 331,

        InvalidMatchEntry = 332,

        UnableDerive = 333,

        // = 334,

        HeaderNotConnected = 335,

        InvalidFunctionDeclaration = 336,

        UnableExtendOpenType = 337,

        FromEnumNotFound = 338,

        FromEnumToNotFound = 339,

        SuccNotFound = 340,

        TypeHeaderNotConnected = 341,

        DefaultMemberAlreadyExist = 342,
	}
}
