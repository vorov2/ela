using System;

namespace Ela.CodeModel
{
	public enum ElaNodeType
	{
		None = 0,


		ListLiteral = 1,

		RecordLiteral = 2,

		TupleLiteral = 3,
		
		UnitLiteral = 4,

		Lambda = 5,

		LazyLiteral = 6,

		// = 7,
		
		
        Header,

		Context,
        
        Builtin,
        		
		Juxtaposition,

		FieldDeclaration,

		Primitive,

		NameReference,

        FieldReference,

		ImportedVariable,
		
		Placeholder,

		As,

		Condition,

		Try,

		Equation,

		Raise,

		EquationSet,

		Match,

		Range,

		ModuleInclude,

		Comprehension,

		Generator,

        LetBinding,

        TypeCheck,

			

        TypeClass,

        ClassInstance,

        ClassMember,

        Newtype
	}
}
