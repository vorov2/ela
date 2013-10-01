
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;


namespace Eladoc.Lexers {

internal sealed partial class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _variantTok = 2;
	public const int _intTok = 3;
	public const int _realTok = 4;
	public const int _stringTok = 5;
	public const int _charTok = 6;
	public const int _operatorTok = 7;
	public const int _parenTok = 8;
	public const int _NL = 9;
	public const int maxT = 43;

	const bool T = true;
	const bool x = false;
	const int minErrDist = 2;
	
	public Scanner scanner;
	public Errors  errors;

	public Token t;    // last recognized token
	public Token la;   // lookahead token
	int errDist = minErrDist;






	public Parser(Scanner scanner) {
		ErrorCount = 0;
		this.scanner = scanner;
		errors = new Errors(this);
	}

	void SynErr (int n) {
		if (errDist >= minErrDist) errors.SynErr(la.line, la.col, n);
		errDist = 0;
	}

	public void SemErr (string msg) {
		if (errDist >= minErrDist) errors.SemErr(t.line, t.col, msg);
		errDist = 0;
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

	
	void Comment() {
		Expect(10);
		var pos = t.pos; 
		while (StartOf(1)) {
			Get();
		}
		Expect(11);
		Add(pos, t.pos - pos + t.val.Length, ElaStyle.MultilineComment); 
		
	}

	void SingleLineComment() {
		Expect(12);
		var pos = t.pos; 
		while (StartOf(2)) {
			Get();
		}
		if (la.kind == 9) {
			Get();
			Add(pos, t.pos - pos + t.val.Length - 1, ElaStyle.Comment); 
		} else if (la.kind == 0) {
			Get();
			Add(pos, t.pos - pos + t.val.Length, ElaStyle.Comment); 
		} else SynErr(44);
	}

	void VerbatimString() {
		Expect(13);
		var pos = t.pos; 
		while (StartOf(3)) {
			Get();
		}
		Expect(14);
		Add(pos, t.pos - pos + t.val.Length, ElaStyle.String); 
	}

	void Primary() {
		switch (la.kind) {
		case 1: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Identifier); 
			break;
		}
		case 2: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Variant); 
			break;
		}
		case 3: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Number); 
			break;
		}
		case 4: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Number); 
			break;
		}
		case 5: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.String); 
			break;
		}
		case 6: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Char); 
			break;
		}
		case 7: case 11: case 15: case 16: {
			if (la.kind == 7) {
				Get();
			} else if (la.kind == 15) {
				Get();
			} else if (la.kind == 16) {
				Get();
			} else {
				Get();
			}
			Add(t.pos, t.val.Length, ElaStyle.Operator); 
			break;
		}
		case 17: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Literal); 
			break;
		}
		case 18: {
			Get();
			Add(t.pos, t.val.Length, ElaStyle.Literal); 
			break;
		}
		case 8: {
			Get();
			break;
		}
		case 19: {
			Get();
			break;
		}
		case 20: {
			Get();
			break;
		}
		case 21: {
			Get();
			break;
		}
		default: SynErr(45); break;
		}
	}

	void Keywords() {
		switch (la.kind) {
		case 22: {
			Get();
			break;
		}
		case 23: {
			Get();
			break;
		}
		case 24: {
			Get();
			break;
		}
		case 25: {
			Get();
			break;
		}
		case 26: {
			Get();
			break;
		}
		case 27: {
			Get();
			break;
		}
		case 28: {
			Get();
			break;
		}
		case 29: {
			Get();
			break;
		}
		case 30: {
			Get();
			break;
		}
		case 31: {
			Get();
			break;
		}
		case 32: {
			Get();
			break;
		}
		case 33: {
			Get();
			break;
		}
		case 34: {
			Get();
			break;
		}
		case 35: {
			Get();
			break;
		}
		case 36: {
			Get();
			break;
		}
		case 37: {
			Get();
			break;
		}
		case 38: {
			Get();
			break;
		}
		case 39: {
			Get();
			break;
		}
		case 40: {
			Get();
			break;
		}
		case 41: {
			Get();
			break;
		}
		case 42: {
			Get();
			break;
		}
		default: SynErr(46); break;
		}
		Add(t.pos, t.val.Length, ElaStyle.Keyword); 
	}

	void Code() {
		switch (la.kind) {
		case 22: case 23: case 24: case 25: case 26: case 27: case 28: case 29: case 30: case 31: case 32: case 33: case 34: case 35: case 36: case 37: case 38: case 39: case 40: case 41: case 42: {
			Keywords();
			break;
		}
		case 13: {
			VerbatimString();
			break;
		}
		case 10: {
			Comment();
			break;
		}
		case 12: {
			SingleLineComment();
			break;
		}
		case 1: case 2: case 3: case 4: case 5: case 6: case 7: case 8: case 11: case 15: case 16: case 17: case 18: case 19: case 20: case 21: {
			Primary();
			break;
		}
		case 9: {
			Get();
			break;
		}
		default: SynErr(47); break;
		}
	}

	void Ela() {
		Code();
		while (StartOf(4)) {
			Code();
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Ela();

    Expect(0);
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,T, T,T,T,x, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
		{x,T,T,T, T,T,T,T, T,x,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
		{x,T,T,T, T,T,T,T, T,T,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x}

	};
} // end Parser


internal sealed class Errors {
	private Parser p;

	internal Errors(Parser p)
	{
		this.p = p;
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
			case 7: s = "operatorTok expected"; break;
			case 8: s = "parenTok expected"; break;
			case 9: s = "NL expected"; break;
			case 10: s = "\"/*\" expected"; break;
			case 11: s = "\"*/\" expected"; break;
			case 12: s = "\"//\" expected"; break;
			case 13: s = "\"<[\" expected"; break;
			case 14: s = "\"]>\" expected"; break;
			case 15: s = "\"*\" expected"; break;
			case 16: s = "\"/\" expected"; break;
			case 17: s = "\"true\" expected"; break;
			case 18: s = "\"false\" expected"; break;
			case 19: s = "\"_\" expected"; break;
			case 20: s = "\";\" expected"; break;
			case 21: s = "\"#\" expected"; break;
			case 22: s = "\"let\" expected"; break;
			case 23: s = "\"do\" expected"; break;
			case 24: s = "\"where\" expected"; break;
			case 25: s = "\"open\" expected"; break;
			case 26: s = "\"import\" expected"; break;
			case 27: s = "\"in\" expected"; break;
			case 28: s = "\"is\" expected"; break;
			case 29: s = "\"if\" expected"; break;
			case 30: s = "\"then\" expected"; break;
			case 31: s = "\"else\" expected"; break;
			case 32: s = "\"match\" expected"; break;
			case 33: s = "\"try\" expected"; break;
			case 34: s = "\"with\" expected"; break;
			case 35: s = "\"__internal\" expected"; break;
			case 36: s = "\"deriving\" expected"; break;
			case 37: s = "\"fail\" expected"; break;
			case 38: s = "\"instance\" expected"; break;
			case 39: s = "\"class\" expected"; break;
			case 40: s = "\"type\" expected"; break;
			case 41: s = "\"data\" expected"; break;
			case 42: s = "\"opentype\" expected"; break;
			case 43: s = "??? expected"; break;
			case 44: s = "invalid SingleLineComment"; break;
			case 45: s = "invalid Primary"; break;
			case 46: s = "invalid Keywords"; break;
			case 47: s = "invalid Code"; break;

			default: s = "error " + n; break;
		}
		
		p.ErrorCount++;
		//ErrorList.Add(new ElaError(s, ElaErrorType.Parser_SyntaxError, new ElaLinePragma(line, col)));
	}

	internal void SemErr (int line, int col, string s) {
		//ErrorList.Add(new ElaError(s, ElaErrorType.Parser_SemanticError, new ElaLinePragma(line, col)));
		p.ErrorCount++;
	}
	
	internal void SemErr (string s) {
		//ErrorList.Add(new ElaError(s, ElaErrorType.Parser_SemanticError, null));
		p.ErrorCount++;
	}
	
	
} // Errors


public class FatalError: Exception {
	public FatalError(string m): base(m) {}
}

}

