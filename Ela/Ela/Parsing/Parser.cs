
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Ela.CodeModel;


namespace Ela.Parsing {

internal sealed partial class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _variantTok = 2;
	public const int _intTok = 3;
	public const int _realTok = 4;
	public const int _stringTok = 5;
	public const int _charTok = 6;
	public const int _operatorTok1 = 7;
	public const int _operatorTok2 = 8;
	public const int _operatorTok3 = 9;
	public const int _operatorTok4 = 10;
	public const int _operatorTok5 = 11;
	public const int _operatorTok6 = 12;
	public const int _operatorTok7 = 13;
	public const int _operatorTok8 = 14;
	public const int _operatorTok9 = 15;
	public const int _operatorTok10 = 16;
	public const int _operatorTok11 = 17;
	public const int _operatorTok12 = 18;
	public const int _LBRA = 19;
	public const int _RBRA = 20;
	public const int _LILB = 21;
	public const int _LIRB = 22;
	public const int _PIPE = 23;
	public const int _ARROW = 24;
	public const int _LAMBDA = 25;
	public const int _COMPO = 26;
	public const int _DOT = 27;
	public const int _IN = 28;
	public const int _MATCH = 29;
	public const int _ASAMP = 30;
	public const int _IS = 31;
	public const int _LET = 32;
	public const int _OPEN = 33;
	public const int _WITH = 34;
	public const int _IFS = 35;
	public const int _ELSE = 36;
	public const int _THEN = 37;
	public const int _RAISE = 38;
	public const int _TRY = 39;
	public const int _TRUE = 40;
	public const int _FALSE = 41;
	public const int _FAIL = 42;
	public const int _WHERE = 43;
	public const int _INSTANCE = 44;
	public const int _TYPE = 45;
	public const int _CLASS = 46;
	public const int _IMPORT = 47;
	public const int _DATA = 48;
	public const int _OPENTYPE = 49;
	public const int _EBLOCK = 50;
	public const int maxT = 67;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;
	internal int errorCount;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;



	public Parser(Scanner scanner) {
		this.scanner = scanner;
		errors = new Errors(this);
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}
	
	
	void AddError(ElaParserError error, params object[] args)
	{
		errors.AddErr(t.line, t.col, error, args);
	}
	
	
	void Get () {
		for (;;) {
			t = la;
			la = scanner.Scan();
			if (la.kind <= maxT) { ++errDist; break; }

			la = t;
		}
	}
	
	void Expect (int n) {
		if (la.kind==n) Get(); else { SynErr(n); }
	}
	
	bool StartOf (int s) {
		return set[s, la.kind];
	}
	
	void ExpectWeak (int n, int follow) {
		if (la.kind == n) Get();
		else {
			SynErr(n);
			while (!StartOf(follow)) Get();
		}
	}


	bool WeakSeparator(int n, int syFol, int repFol) {
		int kind = la.kind;
		if (kind == n) {Get(); return true;}
		else if (StartOf(repFol)) {return false;}
		else {
			SynErr(n);
			while (!(set[syFol, kind] || set[repFol, kind] || set[0, kind])) {
				Get();
				kind = la.kind;
			}
			return StartOf(syFol);
		}
	}

	
	void EndBlock() {
		Expect(50);
		if (!t.virt) scanner.PopIndent(); 
	}

	void Literal(out ElaExpression exp) {
		exp = null; 
		if (StartOf(1)) {
			Primitive(out exp);
		} else if (la.kind == 19) {
			RecordLiteral(out exp);
		} else if (la.kind == 21 || la.kind == 59) {
			ListLiteral(out exp);
		} else if (la.kind == 54) {
			TupleLiteral(out exp);
		} else if (StartOf(2)) {
			SimpleExpr(out exp);
		} else SynErr(68);
		if (la.kind == 30) {
			As(ref exp);
		}
	}

	void Primitive(out ElaExpression exp) {
		exp = null; 
		switch (la.kind) {
		case 3: {
			Get();
			exp = new ElaPrimitive(t) { Value = ParseInt(t.val) };	
			break;
		}
		case 4: {
			Get();
			exp = new ElaPrimitive(t) { Value = ParseReal(t.val) }; 
			break;
		}
		case 5: {
			Get();
			exp = new ElaPrimitive(t) { Value = ParseString(t.val) }; 
			break;
		}
		case 6: {
			Get();
			exp = new ElaPrimitive(t) { Value = ParseChar(t.val) }; 
			break;
		}
		case 40: {
			Get();
			exp = new ElaPrimitive(t) { Value = new ElaLiteralValue(true) }; 
			break;
		}
		case 41: {
			Get();
			exp = new ElaPrimitive(t) { Value = new ElaLiteralValue(false) }; 
			break;
		}
		default: SynErr(69); break;
		}
	}

	void RecordLiteral(out ElaExpression exp) {
		exp = null; 
		var fld = default(ElaFieldDeclaration);
		
		Expect(19);
		var rec = new ElaRecordLiteral(t); exp = rec; 
		RecordField(out fld);
		rec.Fields.Add(fld); 
		while (la.kind == 56) {
			Get();
			RecordField(out fld);
			rec.Fields.Add(fld); 
		}
		Expect(20);
	}

	void ListLiteral(out ElaExpression exp) {
		exp = null; 
		if (la.kind == 21) {
			Get();
			var list = default(List<ElaExpression>);
			var comp = default(ElaComprehension);
			var rng = default(ElaRange);
			var ot = t;
			
			exp = null;
			
			if (StartOf(3)) {
				ParamList(out list, out comp, out rng);
				if (list != null)
				{
				    var listExp = new ElaListLiteral(ot) { Values = list };
				 exp = listExp;
				}	
				else if (comp != null)
				    exp = comp;
				else if (rng != null)
				    exp = rng;
				
			}
			if (exp == null)
			exp = new ElaListLiteral(ot);
			
			Expect(22);
		} else if (la.kind == 59) {
			Get();
			var comp = default(ElaComprehension);
			var ot = t;        				
			exp = null;
			
			var cexp = default(ElaExpression); 
			Expr(out cexp);
			ComprehensionExpr(cexp, out comp);
			comp.Lazy = true;
			exp = comp;
			
			Expect(22);
		} else SynErr(70);
	}

	void TupleLiteral(out ElaExpression exp) {
		var ot = default(Token);
		exp = null; 
		
		Expect(54);
		ot = t; 
		if (StartOf(3)) {
			GroupExpr(out exp);
		}
		if (exp != null)
		   exp.Parens = true;
		
		Expect(55);
		if (exp == null)
		exp = new ElaUnitLiteral(ot);
		
	}

	void SimpleExpr(out ElaExpression exp) {
		VariableReference(out exp);
	}

	void As(ref ElaExpression exp) {
		Expect(30);
		var @as = new ElaAs(t) { Expression = exp }; 
		exp = @as; 
		
		if (la.kind == 1) {
			Get();
			@as.Name = t.val; 
		} else if (la.kind == 54) {
			Get();
			Operators();
			@as.Name = t.val; 
			Expect(55);
		} else SynErr(71);
	}

	void Attribute(ref ElaExpression head) {
		scanner.InjectBlock(); 
		var flags = ElaVariableFlags.None;
		
		if (head.Type != ElaNodeType.NameReference)
		    AddError(ElaParserError.InvalidAttribute, head.ToString());
		
		Expect(51);
		Expect(1);
		flags = ProcessAttribute(t.val,flags); 
		while (la.kind == 1) {
			Get();
			flags = ProcessAttribute(t.val,flags); 
		}
		EndBlock();
		var h = new ElaHeader { Name = head.GetName(), Attributes = flags };
		h.SetLinePragma(head.Line, head.Column);
		head = h;
		
	}

	void VariableReference(out ElaExpression exp) {
		exp = null; 
		if (la.kind == 1 || la.kind == 2 || la.kind == 52) {
			if (la.kind == 1) {
				Get();
				exp = new ElaNameReference(t) { Name = t.val }; 
			} else if (la.kind == 2) {
				Get();
				exp = new ElaNameReference(t) { Name = t.val, Uppercase = true }; 
			} else {
				Get();
				Expect(1);
				exp = new ElaNameReference(t) { Name = t.val, Bang = true }; 
			}
		} else if (la.kind == 53) {
			Get();
			exp = new ElaPlaceholder(t); 
		} else SynErr(72);
	}

	void GroupExpr(out ElaExpression exp) {
		exp = null; 
		var cexp = default(ElaExpression);
		var ot = t;
		
		Expr(out exp);
		if (la.kind == 56) {
			var tuple = new ElaTupleLiteral(ot); 
			tuple.Parameters.Add(exp);
			exp = tuple; 
			
			Get();
			if (StartOf(3)) {
				Expr(out cexp);
				tuple.Parameters.Add(cexp); 
			}
			while (la.kind == 56) {
				Get();
				Expr(out cexp);
				tuple.Parameters.Add(cexp); 
			}
		}
	}

	void Expr(out ElaExpression exp) {
		exp = null; 
		if (StartOf(4)) {
			EmbExpr(out exp);
		} else if (la.kind == 32) {
			LetBinding(out exp);
		} else SynErr(73);
	}

	void RecordField(out ElaFieldDeclaration fld) {
		fld = null; 
		var cexp = default(ElaExpression);
		
		if (la.kind == 1) {
			Get();
			fld = new ElaFieldDeclaration(t) { FieldName = t.val }; 
			if (la.kind == 57) {
				Get();
				Expr(out cexp);
				fld.FieldValue = cexp; 
			}
			if (fld.FieldValue == null)
			fld.FieldValue = new ElaNameReference(t) { Name = t.val };
			
		} else if (la.kind == 5) {
			Get();
			fld = new ElaFieldDeclaration(t) { FieldName = ReadString(t.val) }; 
			Expect(57);
			Expr(out cexp);
			fld.FieldValue = cexp; 
		} else if (la.kind == 54) {
			string name; 
			Get();
			Operators();
			name=t.val; 
			Expect(55);
			fld = new ElaFieldDeclaration(t) { FieldName = name }; 
			if (la.kind == 57) {
				Get();
				Expr(out cexp);
				fld.FieldValue = cexp; 
			}
			if (fld.FieldValue == null)
			fld.FieldValue = new ElaNameReference(t) { Name = t.val };
			
		} else if (la.kind == 20 || la.kind == 56) {
		} else SynErr(74);
	}

	void Operators() {
		switch (la.kind) {
		case 7: {
			Get();
			break;
		}
		case 8: {
			Get();
			break;
		}
		case 9: {
			Get();
			break;
		}
		case 10: {
			Get();
			break;
		}
		case 11: {
			Get();
			break;
		}
		case 12: {
			Get();
			break;
		}
		case 13: {
			Get();
			break;
		}
		case 14: {
			Get();
			break;
		}
		case 15: {
			Get();
			break;
		}
		case 16: {
			Get();
			break;
		}
		case 17: {
			Get();
			break;
		}
		case 18: {
			Get();
			break;
		}
		default: SynErr(75); break;
		}
	}

	void RangeExpr(ElaExpression first, ElaExpression sec, out ElaRange rng) {
		rng = new ElaRange(t) { First = first, Second = sec };
		var cexp = default(ElaExpression);
		
		Expect(58);
		if (StartOf(3)) {
			Expr(out cexp);
			rng.Last = cexp; 
		}
	}

	void ParamList(out List<ElaExpression> list, out ElaComprehension comp, out ElaRange rng ) {
		var exp = default(ElaExpression); 
		list = null;
		comp = null;
		rng = null;
		
		Expr(out exp);
		if (la.kind == 26 || la.kind == 56 || la.kind == 58) {
			if (la.kind == 26) {
				ComprehensionExpr(exp, out comp);
			} else if (la.kind == 58) {
				RangeExpr(exp, null, out rng);
			} else {
				var oexp = exp; 
				Get();
				Expr(out exp);
				if (la.kind == 58) {
					RangeExpr(oexp, exp, out rng);
				} else if (la.kind == 22 || la.kind == 56) {
					list = new List<ElaExpression>();
					list.Add(oexp);
					list.Add(exp);
					
					while (la.kind == 56) {
						Get();
						Expr(out exp);
						list.Add(exp); 
					}
				} else SynErr(76);
			}
		}
		if (list == null && comp == null && rng == null && exp != null)
		{
			list = new List<ElaExpression>();
			list.Add(exp);
		}
		
	}

	void ComprehensionExpr(ElaExpression sel, out ElaComprehension exp) {
		var it = default(ElaGenerator); 
		var ot = t;		
		
		Expect(26);
		ComprehensionEntry(sel, out it);
		exp = new ElaComprehension(ot) { Generator = it }; 
	}

	void IfExpr(out ElaExpression exp) {
		Expect(35);
		var cond = new ElaCondition(t); 
		var cexp = default(ElaExpression);	
		exp = cond;
		
		Expr(out cexp);
		cond.Condition = cexp; 
		Expect(37);
		Expr(out cexp);
		cond.True = cexp; 
		ExpectWeak(36, 5);
		Expr(out cexp);
		cond.False = cexp; 
	}

	void MatchExpr(out ElaExpression exp) {
		scanner.InjectBlock(); 
		bindings.Push(unit);
		
		while (!(la.kind == 0 || la.kind == 29)) {SynErr(77); Get();}
		Expect(29);
		var match = new ElaMatch(t);
		exp = match; 
		var block = default(ElaEquationSet);
		var cexp = default(ElaExpression);
		
		Expr(out cexp);
		match.Expression = cexp; 
		Expect(34);
		BindingChain(out block);
		match.Entries = block; 
		bindings.Pop();
		
		EndBlock();
	}

	void BindingChain(out ElaEquationSet block) {
		block = new ElaEquationSet(); 
		scanner.InjectBlock();
		
		Binding(block);
		if (RequireEndBlock()) 
		EndBlock();
		while (StartOf(3)) {
			scanner.InjectBlock(); 
			Binding(block);
			if (RequireEndBlock()) 
			EndBlock();
		}
	}

	void TryExpr(out ElaExpression exp) {
		scanner.InjectBlock(); 
		bindings.Push(unit);
		
		while (!(la.kind == 0 || la.kind == 39)) {SynErr(78); Get();}
		Expect(39);
		var match = new ElaTry(t);
		exp = match; 
		var block = default(ElaEquationSet);
		var cexp = default(ElaExpression);
		
		Expr(out cexp);
		match.Expression = cexp; 
		Expect(34);
		BindingChain(out block);
		match.Entries = block; 
		bindings.Pop();
		
		EndBlock();
	}

	void OpExpr1(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(6)) {
			OpExpr2(out exp);
			while (la.kind == 15) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(7)) {
					OpExpr1(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 15) {
			Get();
			op = t.val; 
			if (StartOf(6)) {
				OpExpr2(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(79);
	}

	void OpExpr2(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(8)) {
			OpExpr3(out exp);
			while (la.kind == 16) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(8)) {
					OpExpr3(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 16) {
			Get();
			op = t.val; 
			if (StartOf(8)) {
				OpExpr3(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = op };
			
		} else SynErr(80);
	}

	void OpExpr3(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(9)) {
			OpExpr4(out exp);
			while (la.kind == 17) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(9)) {
					OpExpr4(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 17) {
			Get();
			op = t.val; 
			if (StartOf(9)) {
				OpExpr4(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = op };
			
		} else SynErr(81);
	}

	void OpExpr4(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(10)) {
			OpExpr5(out exp);
			while (la.kind == 18) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(10)) {
					OpExpr5(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 18) {
			Get();
			op = t.val; 
			if (StartOf(10)) {
				OpExpr5(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = op };
			
		} else SynErr(82);
	}

	void OpExpr5(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(11)) {
			OpExpr6(out exp);
			while (la.kind == 7) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(11)) {
					OpExpr6(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 7) {
			Get();
			op = t.val; 
			if (StartOf(11)) {
				OpExpr6(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = op };
			
		} else SynErr(83);
	}

	void OpExpr6(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(12)) {
			OpExpr7(out exp);
			while (la.kind == 8) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(12)) {
					OpExpr7(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 8) {
			Get();
			op = t.val; 
			if (StartOf(12)) {
				OpExpr7(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(84);
	}

	void OpExpr7(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(13)) {
			OpExpr8(out exp);
			while (la.kind == 9) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(12)) {
					OpExpr7(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 9) {
			Get();
			op = t.val; 
			if (StartOf(13)) {
				OpExpr8(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(85);
	}

	void OpExpr8(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(14)) {
			OpExpr9(out exp);
			while (la.kind == 10) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(14)) {
					OpExpr9(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 10) {
			Get();
			op = t.val; 
			if (StartOf(14)) {
				OpExpr9(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(86);
	}

	void OpExpr9(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(15)) {
			InfixExpr(out exp);
			while (la.kind == 11) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(15)) {
					InfixExpr(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 11) {
			Get();
			op = t.val; 
			if (StartOf(15)) {
				InfixExpr(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(87);
	}

	void InfixExpr(out ElaExpression exp) {
		exp = null;
		var ot = t; 
		var funexp = default(ElaExpression);
		
		if (StartOf(16)) {
			OpExpr10(out exp);
			while (la.kind == 60) {
				var cexp = default(ElaExpression); 
				ot = t;
				
				Get();
				OpExpr11(out funexp);
				Expect(60);
				if (StartOf(16)) {
					OpExpr10(out cexp);
				}
				var fc = new ElaJuxtaposition(ot) { 
					Target = funexp
				};
				fc.Parameters.Add(exp);			
				
				if (cexp != null)
					fc.Parameters.Add(cexp);
								
				exp = fc;
				
			}
		} else if (la.kind == 60) {
			Get();
			OpExpr10(out funexp);
			Expect(60);
			if (StartOf(16)) {
				OpExpr10(out exp);
				exp = GetPrefixFun(funexp, exp, true);	
			}
			if (exp == null)
			exp = funexp;
			
		} else SynErr(88);
	}

	void OpExpr10(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(17)) {
			OpExpr11(out exp);
			while (la.kind == 12) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(16)) {
					OpExpr10(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 12) {
			Get();
			op = t.val; 
			if (StartOf(17)) {
				OpExpr11(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(89);
	}

	void OpExpr11(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(18)) {
			OpExpr12(out exp);
			while (la.kind == 13) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(18)) {
					OpExpr12(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 13) {
			Get();
			op = t.val; 
			if (StartOf(18)) {
				OpExpr12(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(90);
	}

	void OpExpr12(out ElaExpression exp) {
		var op = String.Empty; 
		exp = null;
		var ot = t; 
		
		if (StartOf(19)) {
			Application(out exp);
			while (la.kind == 14) {
				var cexp = default(ElaExpression); 
				Get();
				op = t.val; 
				if (StartOf(19)) {
					Application(out cexp);
				}
				exp = GetOperatorFun(op, exp, cexp); 
			}
		} else if (la.kind == 14) {
			Get();
			op = t.val; 
			if (StartOf(19)) {
				Application(out exp);
				exp = GetOperatorFun(op, null, exp); 
			}
			if (exp == null)
			exp = new ElaNameReference(ot) { Name = t.val };
			
		} else SynErr(91);
	}

	void Application(out ElaExpression exp) {
		exp = null; 
		AccessExpr(out exp);
		var ot = t;
		var mi = default(ElaJuxtaposition); 
		var cexp = default(ElaExpression);
		
		if (exp.Type == ElaNodeType.Juxtaposition)
		    mi = (ElaJuxtaposition)exp;
		
		while (StartOf(19)) {
			AccessExpr(out cexp);
			if (mi == null)
			{
			    mi = new ElaJuxtaposition(ot) { Target = exp };
				exp = mi;
			}
			else
			    mi.Parens = false;
			
			    if (mi != null)
				    mi.Parameters.Add(cexp); 
			
		}
		if (la.kind == 61) {
			Get();
			var ctx = new ElaContext(t) { Expression = exp }; 
			var cexp2 = default(ElaExpression);
			
			AccessExpr(out cexp2);
			ctx.Context = cexp2; 
			exp = ctx;
			
		}
	}

	void AccessExpr(out ElaExpression exp) {
		Literal(out exp);
		while (la.kind == 27) {
			Get();
			if (la.kind == 54) {
				Get();
				if (la.kind == 1) {
					Get();
				} else if (la.kind == 2) {
					Get();
				} else if (StartOf(20)) {
					Operators();
				} else SynErr(92);
				exp = new ElaFieldReference(t) { FieldName = t.val, TargetObject = exp }; 
				Expect(55);
			} else if (la.kind == 1 || la.kind == 2) {
				if (la.kind == 1) {
					Get();
				} else {
					Get();
				}
				exp = new ElaFieldReference(t) { FieldName = t.val, TargetObject = exp }; 
			} else SynErr(93);
		}
	}

	void EmbExpr(out ElaExpression exp) {
		exp = null; 
		switch (la.kind) {
		case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 9: case 10: case 11: case 12: case 13: case 14: case 15: case 16: case 17: case 18: case 19: case 21: case 40: case 41: case 52: case 53: case 54: case 59: case 60: {
			OpExpr1(out exp);
			if (la.kind == 31) {
				TypeCheckExpr(ref exp);
			}
			break;
		}
		case 35: {
			IfExpr(out exp);
			break;
		}
		case 63: {
			LazyExpr(out exp);
			break;
		}
		case 62: {
			Builtin(out exp);
			break;
		}
		case 25: {
			LambdaExpr(out exp);
			break;
		}
		case 42: {
			FailExpr(out exp);
			break;
		}
		case 29: {
			MatchExpr(out exp);
			break;
		}
		case 39: {
			TryExpr(out exp);
			break;
		}
		case 66: {
			DoBlock(out exp);
			break;
		}
		default: SynErr(94); break;
		}
	}

	void TypeCheckExpr(ref ElaExpression pat) {
		Expect(31);
		var eis = new ElaTypeCheck(t) { Expression = pat };
		pat = eis; 			
		var fst = default(String); 
		var snd = default(String);
		
		while (la.kind == 1 || la.kind == 2) {
			if (la.kind == 1) {
				Get();
			} else {
				Get();
			}
			fst = t.val; snd = null; 
			if (la.kind == 27) {
				Get();
				if (la.kind == 1) {
					Get();
				} else if (la.kind == 2) {
					Get();
				} else SynErr(95);
				snd = t.val; 
			}
			var ti = new TraitInfo(snd != null ? fst : null, snd != null ? snd : fst);
			eis.Traits.Add(ti);
			
		}
	}

	void LazyExpr(out ElaExpression exp) {
		Expect(63);
		var lazy = new ElaLazyLiteral(t); 
		Expr(out exp);
		lazy.Expression = exp;
		exp = lazy;
		
	}

	void Builtin(out ElaExpression exp) {
		Expect(62);
		Expect(1);
		exp = new ElaBuiltin(t) { Kind = Builtins.Kind(t.val) }; 
	}

	void LambdaExpr(out ElaExpression exp) {
		while (!(la.kind == 0 || la.kind == 25)) {SynErr(96); Get();}
		Expect(25);
		var lambda = new ElaLambda(t);
		exp = lambda;
		var left = default(ElaExpression);
		var right = default(ElaExpression);
		
		Expr(out left);
		if (la.kind == 24) {
			Get();
			Expr(out right);
		} else if (la.kind == 23) {
			Get();
			LambdaGuard(out right);
		} else SynErr(97);
		lambda.Left = left;
		lambda.Right = right;
		
	}

	void FailExpr(out ElaExpression exp) {
		Expect(42);
		var r = new ElaRaise(t);
		exp = r;
		var cexp = default(ElaExpression); 
		
		Expr(out cexp);
		r.Expression = cexp; 
		
	}

	void DoBlock(out ElaExpression exp) {
		scanner.InjectBlock(); 			
		var skip = false;
		ElaExpression eqt = new ElaJuxtaposition(t) { Spec = true };
		exp = eqt;
		
		while (!(la.kind == 0 || la.kind == 66)) {SynErr(98); Get();}
		Expect(66);
		if (la.kind == 50) {
			EndBlock();
			skip=true; 
		}
		scanner.InjectBlock(); 
		DoBlockStmt(ref eqt);
		if (RequireEndBlock()) 
		EndBlock();
		while (StartOf(3)) {
			scanner.InjectBlock(); 
			DoBlockStmt(ref eqt);
			if (RequireEndBlock()) 
			EndBlock();
		}
		exp = ValidateDoBlock(exp); 
		if (!skip) 
		EndBlock();
	}

	void LetBinding(out ElaExpression exp) {
		exp = null; 
		while (!(la.kind == 0 || la.kind == 32)) {SynErr(99); Get();}
		Expect(32);
		var bid = new ElaLetBinding(t);
		var block = default(ElaEquationSet);
		var cexp = default(ElaExpression);
		exp = bid;
		
		BindingChain(out block);
		Expect(28);
		Expr(out cexp);
		bid.Equations = block;
		bid.Expression = cexp;
		
	}

	void ComprehensionEntry(ElaExpression body, out ElaGenerator it) {
		it = new ElaGenerator(t);
		it.Body = body;
		var cexp = default(ElaExpression);
		var pat = default(ElaExpression);
		
		Expr(out pat);
		Expect(64);
		Expr(out cexp);
		it.Pattern = pat;
		it.Target = cexp;
		
		if (la.kind == 23 || la.kind == 56) {
			if (la.kind == 56) {
				var cit = default(ElaGenerator); 
				Get();
				ComprehensionEntry(body, out cit);
				it.Body = cit; 
			} else {
				Get();
				Expr(out cexp);
				it.Guard = cexp; 
				if (la.kind == 56) {
					var cit = default(ElaGenerator); 
					Get();
					ComprehensionEntry(body, out cit);
					it.Body = cit; 
				}
			}
		}
	}

	void LambdaGuard(out ElaExpression exp) {
		var ifExp = new ElaCondition(t);
		exp = ifExp;
		
		var cond = default(ElaExpression);
		var trueExp = default(ElaExpression);
		var falseExp = default(ElaExpression);
		
		Expr(out cond);
		ifExp.Condition = cond; 
		Expect(24);
		Expr(out trueExp);
		ifExp.True = trueExp; 
		Expect(23);
		if (la.kind == 36) {
			Get();
			Expect(24);
			Expr(out falseExp);
		} else if (StartOf(3)) {
			LambdaGuard(out falseExp);
		} else SynErr(100);
		ifExp.False = falseExp; 
	}

	void WhereBinding(out ElaEquationSet block) {
		scanner.InjectBlock(); 
		bindings.Push(null);
		var skip = false;
		
		while (!(la.kind == 0 || la.kind == 43)) {SynErr(101); Get();}
		Expect(43);
		if (la.kind == 50) {
			EndBlock();
			skip=true; 
		}
		BindingChain(out block);
		bindings.Pop(); 
		if (!skip) 
		EndBlock();
	}

	void Binding(ElaEquationSet block) {
		var bid = default(ElaEquation);
		var left = default(ElaExpression);
		var right = default(ElaExpression);
		
		Expr(out left);
		bid = new ElaEquation(t); 
		if (la.kind == 23 || la.kind == 51 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				Expr(out right);
			} else if (la.kind == 23) {
				Get();
				Guard(out right);
			} else {
				Attribute(ref left);
			}
		}
		if (la.kind == 43) {
			var cb = default(ElaEquationSet); 
			WhereBinding(out cb);
			if (left != null && left.Type == ElaNodeType.Header)
			   AddError(ElaParserError.InvalidAttributeWhere);
			
			    var letb = new ElaLetBinding();                    
			       if (cb != null) letb.SetLinePragma(cb.Line, cb.Column);                    
			       letb.Equations = cb;
			        
			    if (right != null)
			    {
			        letb.Expression = right;
			           right = letb;
			    }
			    else
			    {
			        letb.Expression = left;
			        left = letb;
			    }	                    
			
		}
		ProcessBinding(block, bid, left, right); 
	}

	void Guard(out ElaExpression exp) {
		var ifExp = new ElaCondition(t);
		exp = ifExp;
		
		var cond = default(ElaExpression);
		var trueExp = default(ElaExpression);
		var falseExp = default(ElaExpression);
		
		Expr(out cond);
		ifExp.Condition = cond; 
		Expect(57);
		Expr(out trueExp);
		ifExp.True = trueExp; 
		Expect(23);
		if (la.kind == 36) {
			Get();
			Expect(57);
			Expr(out falseExp);
		} else if (StartOf(3)) {
			Guard(out falseExp);
		} else SynErr(102);
		ifExp.False = falseExp; 
	}

	void IncludeStat() {
		var qual = false; 
		scanner.InjectBlock(); 
		while (!(la.kind == 0 || la.kind == 33 || la.kind == 47)) {SynErr(103); Get();}
		if (la.kind == 33) {
			Get();
		} else if (la.kind == 47) {
			Get();
			qual=true; 
		} else SynErr(104);
		IncludeStatElement(qual);
		EndBlock();
	}

	void IncludeStatElement(bool qual) {
		var inc = new ElaModuleInclude(t) { RequireQualified=qual }; 
		Qualident(inc.Path);
		var name = inc.Path[inc.Path.Count - 1];				
		inc.Path.RemoveAt(inc.Path.Count - 1);				
		inc.Alias = inc.Name = name;				
		Program.Includes.Add(inc);
		
		if (la.kind == 51) {
			Get();
			if (la.kind == 1) {
				Get();
				inc.DllName = t.val; 
			} else if (la.kind == 5) {
				Get();
				inc.DllName = ReadString(t.val); 
			} else SynErr(105);
		}
		if (la.kind == 30) {
			Get();
			Expect(1);
			inc.Alias = t.val; 
		}
		if (la.kind == 54) {
			var imp = default(ElaImportedVariable); 
			Get();
			ImportName(out imp);
			inc.ImportList.Add(imp); 
			while (la.kind == 56) {
				Get();
				ImportName(out imp);
				inc.ImportList.Add(imp); 
			}
			Expect(55);
		}
		if (la.kind == 1 || la.kind == 5) {
			IncludeStatElement(qual);
		}
	}

	void Qualident(List<String> path ) {
		var val = String.Empty; 
		if (la.kind == 1) {
			Get();
			val = t.val; 
		} else if (la.kind == 5) {
			Get();
			val = ReadString(t.val); 
		} else SynErr(106);
		path.Add(val); 
		if (la.kind == 27) {
			Get();
			Qualident(path);
		}
	}

	void ImportName(out ElaImportedVariable imp) {
		imp = new ElaImportedVariable(t); 
		
		Expect(1);
		imp.Name = imp.LocalName = t.val; 
		if (la.kind == 1) {
			Get();
			if (imp.Name != "private")
			   AddError(ElaParserError.UnknownAttribute, imp.Name);
			
			imp.Private = true;
			imp.Name = imp.LocalName = t.val; 
			
		}
		if (la.kind == 57) {
			Get();
			Expect(1);
			imp.Name = t.val; 
		}
	}

	void TypeClass() {
		var tc = default(ElaTypeClass);
		scanner.InjectBlock();
		
		Expect(46);
		tc = new ElaTypeClass(t); 	            
		tc.And = Program.Classes;
		Program.Classes = tc;
		
		Expect(2);
		var nm = default(String);
		tc.Name = t.val;
		
		if (la.kind == 54) {
			Get();
			Expect(2);
			tc.BuiltinName=t.val; 
			Expect(55);
		}
		var targ = String.Empty; 
		Expect(1);
		targ = t.val; 
		Expect(43);
		if (la.kind == 1) {
			Get();
			nm=t.val; 
		} else if (StartOf(20)) {
			Operators();
			nm=t.val; 
		} else if (la.kind == 54) {
			Get();
			if (StartOf(20)) {
				Operators();
			} else if (la.kind == 1) {
				Get();
			} else SynErr(107);
			nm=t.val; 
			Expect(55);
		} else SynErr(108);
		var count = 0;
		var mask = 0;
		
		if (la.kind == 1) {
			Get();
		} else if (la.kind == 53) {
			Get();
		} else SynErr(109);
		BuildMask(ref count, ref mask, t.val, targ); 
		while (la.kind == 24) {
			Get();
			if (la.kind == 1) {
				Get();
			} else if (la.kind == 53) {
				Get();
			} else SynErr(110);
			BuildMask(ref count, ref mask, t.val, targ); 
		}
		tc.Members.Add(new ElaClassMember(t) { Name = nm, Components = count, Mask = mask });
		
		while (StartOf(21)) {
			if (la.kind == 1) {
				Get();
				nm=t.val; 
			} else if (StartOf(20)) {
				Operators();
				nm=t.val; 
			} else {
				Get();
				if (StartOf(20)) {
					Operators();
				} else if (la.kind == 1) {
					Get();
				} else SynErr(111);
				nm=t.val; 
				Expect(55);
			}
			count = 0;
			mask = 0;
			
			if (la.kind == 1) {
				Get();
			} else if (la.kind == 53) {
				Get();
			} else SynErr(112);
			BuildMask(ref count, ref mask, t.val, targ); 
			while (la.kind == 24) {
				Get();
				if (la.kind == 1) {
					Get();
				} else if (la.kind == 53) {
					Get();
				} else SynErr(113);
				BuildMask(ref count, ref mask, t.val, targ); 
			}
			tc.Members.Add(new ElaClassMember(t) { Name = nm, Components = count, Mask = mask });
			
		}
		EndBlock();
	}

	void NewType() {
		scanner.InjectBlock(); 
		var extends = false;
		var opened = false;
		
		if (la.kind == 45) {
			Get();
		} else if (la.kind == 48) {
			Get();
			extends=true; 
		} else if (la.kind == 49) {
			Get();
			opened=true; 
		} else SynErr(114);
		var nt = new ElaNewtype(t) { Extends=extends,Opened=opened }; 
		if (la.kind == 1) {
			Get();
			nt.Prefix = t.val; 
			Expect(27);
			Expect(2);
			nt.Name = t.val; 
		} else if (la.kind == 2) {
			Get();
			nt.Name = t.val; 
		} else SynErr(115);
		nt.And = Program.Types;
		Program.Types = nt;
		
		if (la.kind == 51 || la.kind == 57) {
			if (la.kind == 57) {
				Get();
				if (la.kind == 23) {
					Get();
				}
				NewTypeConstructor(nt);
				while (la.kind == 23) {
					Get();
					NewTypeConstructor(nt);
				}
			} else {
				Get();
				nt.Header = true; 
				Expect(1);
				nt.Flags = ProcessAttribute(t.val,nt.Flags); 
				while (la.kind == 1) {
					Get();
					nt.Flags = ProcessAttribute(t.val,nt.Flags); 
				}
			}
		}
		if (la.kind == 65) {
			Get();
			var tt = t; 
			string pf;
			string nm;
			
			while (la.kind == 1 || la.kind == 2) {
				if (la.kind == 1) {
					Get();
					pf=t.val; 
					Expect(27);
					Expect(2);
					nm=t.val; 
				} else {
					Get();
					pf=null;nm=t.val; 
				}
				var ci = new ElaClassInstance(tt);
				ci.And = Program.Instances;
				Program.Instances = ci;
				ci.TypeName = nt.Name;
				ci.TypePrefix = nt.Prefix;
				ci.TypeClassPrefix = pf;
				ci.TypeClassName = nm;
				
			}
		}
		EndBlock();
	}

	void NewTypeConstructor(ElaNewtype nt) {
		var flags = ElaVariableFlags.None; 
		var exp = default(ElaExpression);
		
		OpExpr1(out exp);
		if (la.kind == 51) {
			Get();
			Expect(1);
			flags = ProcessAttribute(t.val,flags); 
			while (la.kind == 1) {
				Get();
				flags = ProcessAttribute(t.val,flags); 
			}
		}
		nt.Constructors.Add(exp); 
		nt.ConstructorFlags.Add(flags);
		
	}

	void ClassInstance() {
		var block = default(ElaEquationSet);
		var ot = t;
		var list = default(List<ElaClassInstance>);
		
		Expect(44);
		var ci = new ElaClassInstance(t);	            
		ci.And = Program.Instances;
		Program.Instances = ci;
		ot = t;
		
		if (la.kind == 1) {
			Get();
			ci.TypeClassPrefix=t.val; 
			Expect(27);
			Expect(2);
			ci.TypeClassName=t.val; 
		} else if (la.kind == 2) {
			Get();
			ci.TypeClassName=t.val; 
		} else SynErr(116);
		while (la.kind == 1 || la.kind == 2) {
			if (ci.TypeName != null)
			{
			    if (list == null)
			    {
			        list = new List<ElaClassInstance>();
			        list.Add(ci);
			    }
			    
			    ci = new ElaClassInstance { TypeClassPrefix = ci.TypeClassPrefix, TypeClassName = ci.TypeClassName };
			    ci.SetLinePragma(ot.line, ot.col);
			    ci.And = Program.Instances;
			    Program.Instances = ci;
			    list.Add(ci);
			}
			
			if (la.kind == 1) {
				Get();
				ci.TypePrefix = t.val; 
				Expect(27);
				Expect(2);
				ci.TypeName=t.val; 
			} else {
				Get();
				ci.TypeName=t.val; 
			}
		}
		if (la.kind == 43) {
			WhereBinding(out block);
			if (list == null)
			   ci.Where = block; 
			else
			{
			    for (var i = 0; i < list.Count; i++)
			        list[i].Where = block;
			}
			
		}
	}

	void DoBlockStmt(ref ElaExpression eqt) {
		var cexp1 = default(ElaExpression);
		var cexp2 = default(ElaExpression);
		
		if (StartOf(4)) {
			EmbExpr(out cexp1);
			if (la.kind == 64) {
				Get();
				EmbExpr(out cexp2);
			}
			ProcessDoBlock(cexp1, cexp2, ref eqt); 
		} else if (la.kind == 32) {
			scanner.InjectBlock(); 
			var eqs = new ElaEquationSet();
			
			Get();
			Binding(eqs);
			if (eqt.Type == ElaNodeType.LetBinding)
			   ((ElaLetBinding)eqt).Equations.Equations.AddRange(eqs.Equations);
			else
			{
			    var lt = new ElaLetBinding(t);
			    lt.Equations = eqs;
			    if (eqt.Type == ElaNodeType.Lambda)
			        ((ElaLambda)eqt).Right = lt;
			    else
			        lt.Expression = eqt;
			    eqt = lt;
			}
			
			EndBlock();
		} else SynErr(117);
	}

	void TopLevel() {
		scanner.InjectBlock(); 
		if (StartOf(3)) {
			Binding(Program.TopLevel);
		} else if (la.kind == 33 || la.kind == 47) {
			IncludeStat();
		} else if (la.kind == 46) {
			TypeClass();
		} else if (la.kind == 45 || la.kind == 48 || la.kind == 49) {
			NewType();
		} else if (la.kind == 44) {
			ClassInstance();
		} else SynErr(118);
		EndBlock();
		if (StartOf(22)) {
			TopLevel();
		}
	}

	void Ela() {
		Program = new ElaProgram(); 
		TopLevel();
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Ela();

    Expect(0);
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,T,x,x, x,T,x,x, T,T,x,x, x,x,x,T, x,x,x,T, x,x,x,T, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x},
		{x,x,x,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,x, x,T,x,x, x,T,x,x, T,x,x,T, x,x,x,T, T,T,T,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,T,T, x,x,T,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,x, x,T,x,x, x,T,x,x, x,x,x,T, x,x,x,T, T,T,T,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,T,T, x,x,T,x, x},
		{T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,x, x,T,x,x, x,T,x,x, T,T,x,T, x,x,x,T, T,T,T,T, x,x,x,T, x,x,x,x, T,T,T,x, x,x,x,T, T,x,T,T, x,x,T,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,T,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,T,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, T,T,T,T, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,T,T,T, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,T,T, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,T, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,x, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, T,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,x, T,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,x, x,T,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,x, x,x,T,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,T, x,T,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, T,T,x,x, x,x,x,x, x,x,x,x, T,T,T,x, x,x,x,T, x,x,x,x, x,x,x,x, x},
		{x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x,T,x,x, x,T,x,x, x,T,x,x, T,T,x,T, x,x,x,T, T,T,T,x, T,T,T,T, T,T,x,x, T,T,T,x, x,x,x,T, T,x,T,T, x,x,T,x, x}

	};
} // end Parser


internal sealed class Errors {
	private Parser parser;

	internal Errors(Parser parser)
	{
		this.parser = parser;
		ErrorList = new List<ElaMessage>();
	}
  
	internal void SynErr (int line, int col, int n) {
		string s;
		switch (n) {
			case 0: s = "EOF expected"; break;
			case 1: s = "ident expected"; break;
			case 2: s = "variantTok expected"; break;
			case 3: s = "intTok expected"; break;
			case 4: s = "realTok expected"; break;
			case 5: s = "stringTok expected"; break;
			case 6: s = "charTok expected"; break;
			case 7: s = "operatorTok1 expected"; break;
			case 8: s = "operatorTok2 expected"; break;
			case 9: s = "operatorTok3 expected"; break;
			case 10: s = "operatorTok4 expected"; break;
			case 11: s = "operatorTok5 expected"; break;
			case 12: s = "operatorTok6 expected"; break;
			case 13: s = "operatorTok7 expected"; break;
			case 14: s = "operatorTok8 expected"; break;
			case 15: s = "operatorTok9 expected"; break;
			case 16: s = "operatorTok10 expected"; break;
			case 17: s = "operatorTok11 expected"; break;
			case 18: s = "operatorTok12 expected"; break;
			case 19: s = "LBRA expected"; break;
			case 20: s = "RBRA expected"; break;
			case 21: s = "LILB expected"; break;
			case 22: s = "LIRB expected"; break;
			case 23: s = "PIPE expected"; break;
			case 24: s = "ARROW expected"; break;
			case 25: s = "LAMBDA expected"; break;
			case 26: s = "COMPO expected"; break;
			case 27: s = "DOT expected"; break;
			case 28: s = "IN expected"; break;
			case 29: s = "MATCH expected"; break;
			case 30: s = "ASAMP expected"; break;
			case 31: s = "IS expected"; break;
			case 32: s = "LET expected"; break;
			case 33: s = "OPEN expected"; break;
			case 34: s = "WITH expected"; break;
			case 35: s = "IFS expected"; break;
			case 36: s = "ELSE expected"; break;
			case 37: s = "THEN expected"; break;
			case 38: s = "RAISE expected"; break;
			case 39: s = "TRY expected"; break;
			case 40: s = "TRUE expected"; break;
			case 41: s = "FALSE expected"; break;
			case 42: s = "FAIL expected"; break;
			case 43: s = "WHERE expected"; break;
			case 44: s = "INSTANCE expected"; break;
			case 45: s = "TYPE expected"; break;
			case 46: s = "CLASS expected"; break;
			case 47: s = "IMPORT expected"; break;
			case 48: s = "DATA expected"; break;
			case 49: s = "OPENTYPE expected"; break;
			case 50: s = "EBLOCK expected"; break;
			case 51: s = "\"#\" expected"; break;
			case 52: s = "\"!\" expected"; break;
			case 53: s = "\"_\" expected"; break;
			case 54: s = "\"(\" expected"; break;
			case 55: s = "\")\" expected"; break;
			case 56: s = "\",\" expected"; break;
			case 57: s = "\"=\" expected"; break;
			case 58: s = "\"..\" expected"; break;
			case 59: s = "\"[&\" expected"; break;
			case 60: s = "\"`\" expected"; break;
			case 61: s = "\":::\" expected"; break;
			case 62: s = "\"__internal\" expected"; break;
			case 63: s = "\"&\" expected"; break;
			case 64: s = "\"<-\" expected"; break;
			case 65: s = "\"deriving\" expected"; break;
			case 66: s = "\"do\" expected"; break;
			case 67: s = "??? expected"; break;
			case 68: s = "invalid Literal"; break;
			case 69: s = "invalid Primitive"; break;
			case 70: s = "invalid ListLiteral"; break;
			case 71: s = "invalid As"; break;
			case 72: s = "invalid VariableReference"; break;
			case 73: s = "invalid Expr"; break;
			case 74: s = "invalid RecordField"; break;
			case 75: s = "invalid Operators"; break;
			case 76: s = "invalid ParamList"; break;
			case 77: s = "this symbol not expected in MatchExpr"; break;
			case 78: s = "this symbol not expected in TryExpr"; break;
			case 79: s = "invalid OpExpr1"; break;
			case 80: s = "invalid OpExpr2"; break;
			case 81: s = "invalid OpExpr3"; break;
			case 82: s = "invalid OpExpr4"; break;
			case 83: s = "invalid OpExpr5"; break;
			case 84: s = "invalid OpExpr6"; break;
			case 85: s = "invalid OpExpr7"; break;
			case 86: s = "invalid OpExpr8"; break;
			case 87: s = "invalid OpExpr9"; break;
			case 88: s = "invalid InfixExpr"; break;
			case 89: s = "invalid OpExpr10"; break;
			case 90: s = "invalid OpExpr11"; break;
			case 91: s = "invalid OpExpr12"; break;
			case 92: s = "invalid AccessExpr"; break;
			case 93: s = "invalid AccessExpr"; break;
			case 94: s = "invalid EmbExpr"; break;
			case 95: s = "invalid TypeCheckExpr"; break;
			case 96: s = "this symbol not expected in LambdaExpr"; break;
			case 97: s = "invalid LambdaExpr"; break;
			case 98: s = "this symbol not expected in DoBlock"; break;
			case 99: s = "this symbol not expected in LetBinding"; break;
			case 100: s = "invalid LambdaGuard"; break;
			case 101: s = "this symbol not expected in WhereBinding"; break;
			case 102: s = "invalid Guard"; break;
			case 103: s = "this symbol not expected in IncludeStat"; break;
			case 104: s = "invalid IncludeStat"; break;
			case 105: s = "invalid IncludeStatElement"; break;
			case 106: s = "invalid Qualident"; break;
			case 107: s = "invalid TypeClass"; break;
			case 108: s = "invalid TypeClass"; break;
			case 109: s = "invalid TypeClass"; break;
			case 110: s = "invalid TypeClass"; break;
			case 111: s = "invalid TypeClass"; break;
			case 112: s = "invalid TypeClass"; break;
			case 113: s = "invalid TypeClass"; break;
			case 114: s = "invalid NewType"; break;
			case 115: s = "invalid NewType"; break;
			case 116: s = "invalid ClassInstance"; break;
			case 117: s = "invalid DoBlockStmt"; break;
			case 118: s = "invalid TopLevel"; break;

			default: s = "error " + n; break;
		}
		
		parser.errorCount++;
		
		if (parser.la.val == "\t" || parser.t.val == "\t")
			n = -1;
		
		ErrorList.Add(ErrorReporter.CreateMessage(n, s, line, col, parser.la));
	}
	
	
	internal void AddErr (int line, int col, ElaParserError err, params object[] args) {
		parser.errorCount++;
		var str = Strings.GetMessage(err, args);		
		ErrorList.Add(new ElaMessage(str, MessageType.Error, (Int32)err, line, col));
	}

	/*internal void SemErr (int line, int col, string s) {
		parser.errorCount++;
		ErrorList.Add(new ElaMessage(s, MessageType.Error, (Int32)ElaParserError.InvalidSyntax, line, col));
	}
	
	internal void SemErr (string s) {
		parser.errorCount++;
		ErrorList.Add(new ElaMessage(s, MessageType.Error, (Int32)ElaParserError.InvalidSyntax, 0, 0));
	}*/
	
	public List<ElaMessage> ErrorList { get; private set; }
} // Errors

}

