using System;
using Ela.CodeModel;

namespace Ela.Parsing
{
	internal static class ErrorReporter
	{
		internal static ElaMessage CreateMessage(int error, string message, int line, int col, Token tok)
		{
			var err = String.IsNullOrEmpty(tok.val) ? ElaParserError.InvalidSyntax :
				ElaParserError.InvalidSyntaxUnexpectedSymbol;
			var msg = tok.val;

			if (error == -1)
				err = ElaParserError.TabNotAllowed;
			else if (error == Parser._EBLOCK)
				err = ElaParserError.IncorrectIndentation;
			else
			{
				var arr = message.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

				if (arr.Length == 2)
				{
					var head = arr[0].Trim('\"');
					var tail = arr[1].Trim('\"');
					var exp = tail == "expected";
					var e = exp ? ExpectToken(error, head) : InvalidToken(error, tail);

					if (e != ElaParserError.InvalidRoot || err != ElaParserError.InvalidSyntaxUnexpectedSymbol)
					{
						err = e;
						msg = exp ? head : tail;
					}
				}
			}

			if (err == ElaParserError.ExpectedEof && !String.IsNullOrEmpty(tok.val))
			{
				err = ElaParserError.InvalidIndentationUnexpectedSymbol;
				msg = tok.val;
			}

			return new ElaMessage(Strings.GetMessage(err, msg), MessageType.Error, (Int32)err, line, col);
		}

		private static ElaParserError InvalidToken(int error, string prod)
		{
			switch (prod)
			{
               case "Literal": return ElaParserError.InvalidLiteral;
                case "Operators": return ElaParserError.InvalidOperator;
				case "SimpleExpr": return ElaParserError.InvalidSimpleExpression;
				case "VariantLiteral": return ElaParserError.InvalidVariant;
				case "Primitive": return ElaParserError.InvalidPrimitive;
                case "VariableReference": return ElaParserError.InvalidVariableReference;
				case "TupleLiteral": return ElaParserError.InvalidTuple;
				case "GroupExpr": return ElaParserError.InvalidGrouping;
				case "MatchExpr": return ElaParserError.InvalidMatch;
				case "Guard": return ElaParserError.InvalidGuard;
                case "LambdaGuard": return ElaParserError.InvalidGuard;
              	case "RecordLiteral": return ElaParserError.InvalidRecord;
				case "RecordField": return ElaParserError.InvalidRecord;
				case "RangeExpr": return ElaParserError.InvalidRange;
				case "ListLiteral": return ElaParserError.InvalidList;
				case "ParamList": return ElaParserError.InvalidParamList;
				case "LetBinding": return ElaParserError.InvalidBinding;
				case "WhereBinding": return ElaParserError.InvalidWhereBinding;
                case "Attribute": return ElaParserError.InvalidHeader;
                case "As": return ElaParserError.InvalidAs;
                case "TypeCheckExpr": return ElaParserError.InvalidTypeCheck;
                case "TypeClass": return ElaParserError.InvalidTypeClass;
                case "Instance": return ElaParserError.InvalidClassInstance;
                case "NewType": return ElaParserError.InvalidNewType;
                case "NewTypeConstructor": return ElaParserError.InvalidNewType;
				
                case "LambdaExpr": return ElaParserError.InvalidLambda;
				case "IncludeStat": return ElaParserError.InvalidInclude;
                case "IncludeStatElement": return ElaParserError.InvalidInclude;
                case "ImportName": return ElaParserError.InvalidInclude;
				case "Qualident": return ElaParserError.InvalidInclude;
				case "IfExpr": return ElaParserError.InvalidIf;
				case "FailExpr": return ElaParserError.InvalidFail;
				case "TryExpr": return ElaParserError.InvalidTry;
                case "OpExpr1": return ElaParserError.InvalidOperation;
                case "OpExpr2": return ElaParserError.InvalidOperation;
                case "OpExpr3": return ElaParserError.InvalidOperation;
                case "OpExpr4": return ElaParserError.InvalidOperation;
                case "OpExpr5": return ElaParserError.InvalidOperation;
                case "OpExpr6": return ElaParserError.InvalidOperation;
                case "OpExpr7": return ElaParserError.InvalidOperation;
                case "OpExpr8": return ElaParserError.InvalidOperation;
                case "OpExpr9": return ElaParserError.InvalidOperation;
                case "OpExpr10": return ElaParserError.InvalidOperation;
                case "OpExpr11": return ElaParserError.InvalidOperation;
                case "OpExpr12": return ElaParserError.InvalidOperation;
                case "InfixExpr": return ElaParserError.InvalidInfix;
				case "Application": return ElaParserError.InvalidApplication;
				case "AccessExpr": return ElaParserError.InvalidMemberAccess;
				case "LazyExpr": return ElaParserError.InvalidLazy;
                case "ComprehensionExpr": return ElaParserError.InvalidComprehension;
				case "ComprehensionEntry": return ElaParserError.InvalidComprehension;
                case "Expr": return ElaParserError.InvalidExpression;
				case "EmbExpr": return ElaParserError.InvalidExpression;
                case "TopLevel": return ElaParserError.InvalidRoot;
				case "Ela": return ElaParserError.InvalidRoot;
                case "DoBlock": return ElaParserError.InvalidDoBlock;
                case "DoBlockStmt": return ElaParserError.InvalidDoBlock;
				default: return ElaParserError.InvalidProduction;
			}
		}
        
		private static ElaParserError ExpectToken(int error, string prod)
		{
			switch (error)
			{
				case Parser._EOF: return ElaParserError.ExpectedEof;
				case Parser._ident: return ElaParserError.ExpectedIdentifierToken;
				case Parser._intTok: return ElaParserError.ExpectedIntToken;
				case Parser._realTok: return ElaParserError.ExpectedRealToken;
				case Parser._stringTok: return ElaParserError.ExpectedStringToken;
				case Parser._charTok: return ElaParserError.ExpectedCharToken;
                case Parser._operatorTok1:
				case Parser._operatorTok2:
				case Parser._operatorTok3:
				case Parser._operatorTok4:
				case Parser._operatorTok5: 
				case Parser._operatorTok6:
                case Parser._operatorTok7:
                case Parser._operatorTok8:
                case Parser._operatorTok9:
                case Parser._operatorTok10:
                case Parser._operatorTok11:
                case Parser._operatorTok12:                 
                    return ElaParserError.ExpectedOperatorToken;
				case Parser._LBRA: return ElaParserError.ExpectedCurlyBrace;
				case Parser._RBRA: return ElaParserError.ExpectedCurlyBrace;
				case Parser._LILB: return ElaParserError.ExpectedSquareBrace;
				case Parser._LIRB: return ElaParserError.ExpectedSquareBrace;
				case Parser._ARROW: return ElaParserError.ExpectedArrow;
				case Parser._LAMBDA: return ElaParserError.ExpectedLambda;
				case Parser._DOT: return ElaParserError.ExpectedDot;
				case Parser._IN: return ElaParserError.ExpectedKeywordIn;
				case Parser._MATCH: return ElaParserError.ExpectedKeywordMatch;
				case Parser._ASAMP: return ElaParserError.ExpectedKeywordAsAmp;
				case Parser._IS: return ElaParserError.ExpectedIsOperator;
				case Parser._LET: return ElaParserError.ExpectedKeywordLet;
				case Parser._OPEN: return ElaParserError.ExpectedKeywordOpen;
				case Parser._WITH: return ElaParserError.ExpectedKeywordWith;
				case Parser._IFS: return ElaParserError.ExpectedKeywordIf;
				case Parser._ELSE: return ElaParserError.ExpectedKeywordElse;
				case Parser._THEN: return ElaParserError.ExpectedKeywordThen;				
				case Parser._RAISE: return ElaParserError.ExpectedKeywordRaise;
				case Parser._TRY: return ElaParserError.ExpectedKeywordTry;
				case Parser._TRUE: return ElaParserError.ExpectedBooleanToken;
				case Parser._FALSE: return ElaParserError.ExpectedBooleanToken;				
				case Parser._FAIL: return ElaParserError.ExpectedKeywordFail;
				case Parser._WHERE: return ElaParserError.ExpectedKeywordWhere;
				case Parser._COMPO: return ElaParserError.ExpectedComprehensionSlash;
                case Parser._INSTANCE: return ElaParserError.ExpectedKeywordInstance;
                case Parser._CLASS: return ElaParserError.ExpectedKeywordClass;
                case Parser._TYPE: return ElaParserError.ExpectedKeywordType;
                case Parser._OPENTYPE: return ElaParserError.ExpectedKeywordOpentype;
                case Parser._IMPORT: return ElaParserError.ExpectedKeywordImport;
                case Parser._PIPE: return ElaParserError.ExpectedPipe;
                
				default: return ElaParserError.ExpectedToken;
			}
		}
	}
}
