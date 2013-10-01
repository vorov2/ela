
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Elide.Scintilla;


namespace Elide.EilCode.Lexer {

internal sealed partial class Parser {
	public const int _EOF = 0;
	public const int _ident = 1;
	public const int _intTok = 2;
	public const int _address = 3;
	public const int _stringTok = 4;
	public const int _charTok = 5;
	public const int _operatorTok = 6;
	public const int _NL = 7;
	public const int maxT = 107;

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

	
	void SingleLineComment() {
		Expect(8);
		var pos = t.pos; 
		while (StartOf(1)) {
			Get();
		}
		if (la.kind == 7) {
			Get();
		} else if (la.kind == 0) {
			Get();
		} else SynErr(108);
		Add(pos, t.pos - pos + t.val.Length, TextStyle.Style4); 
	}

	void OpNum() {
		Expect(9);
		Add(t.pos, t.val.Length, TextStyle.Style7); 
		Expect(2);
		Add(t.pos, t.val.Length, TextStyle.Style7); 
		Expect(10);
		Add(t.pos, t.val.Length, TextStyle.Style7); 
	}

	void Keyword() {
		switch (la.kind) {
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
		case 43: {
			Get();
			break;
		}
		case 44: {
			Get();
			break;
		}
		case 45: {
			Get();
			break;
		}
		case 46: {
			Get();
			break;
		}
		case 47: {
			Get();
			break;
		}
		case 48: {
			Get();
			break;
		}
		case 49: {
			Get();
			break;
		}
		case 50: {
			Get();
			break;
		}
		case 51: {
			Get();
			break;
		}
		case 52: {
			Get();
			break;
		}
		case 53: {
			Get();
			break;
		}
		case 54: {
			Get();
			break;
		}
		case 55: {
			Get();
			break;
		}
		case 56: {
			Get();
			break;
		}
		case 57: {
			Get();
			break;
		}
		case 58: {
			Get();
			break;
		}
		case 59: {
			Get();
			break;
		}
		case 60: {
			Get();
			break;
		}
		case 61: {
			Get();
			break;
		}
		case 62: {
			Get();
			break;
		}
		case 63: {
			Get();
			break;
		}
		case 64: {
			Get();
			break;
		}
		case 65: {
			Get();
			break;
		}
		case 66: {
			Get();
			break;
		}
		case 67: {
			Get();
			break;
		}
		case 68: {
			Get();
			break;
		}
		case 69: {
			Get();
			break;
		}
		case 70: {
			Get();
			break;
		}
		case 71: {
			Get();
			break;
		}
		case 72: {
			Get();
			break;
		}
		case 73: {
			Get();
			break;
		}
		case 74: {
			Get();
			break;
		}
		case 75: {
			Get();
			break;
		}
		case 76: {
			Get();
			break;
		}
		case 77: {
			Get();
			break;
		}
		case 78: {
			Get();
			break;
		}
		case 79: {
			Get();
			break;
		}
		case 80: {
			Get();
			break;
		}
		case 81: {
			Get();
			break;
		}
		case 82: {
			Get();
			break;
		}
		case 83: {
			Get();
			break;
		}
		case 84: {
			Get();
			break;
		}
		case 85: {
			Get();
			break;
		}
		case 86: {
			Get();
			break;
		}
		case 87: {
			Get();
			break;
		}
		case 88: {
			Get();
			break;
		}
		case 89: {
			Get();
			break;
		}
		case 90: {
			Get();
			break;
		}
		case 91: {
			Get();
			break;
		}
		case 92: {
			Get();
			break;
		}
		case 93: {
			Get();
			break;
		}
		case 94: {
			Get();
			break;
		}
		case 95: {
			Get();
			break;
		}
		case 96: {
			Get();
			break;
		}
		case 97: {
			Get();
			break;
		}
		case 98: {
			Get();
			break;
		}
		case 99: {
			Get();
			break;
		}
		case 100: {
			Get();
			break;
		}
		case 101: {
			Get();
			break;
		}
		case 102: {
			Get();
			break;
		}
		case 103: {
			Get();
			break;
		}
		case 104: {
			Get();
			break;
		}
		case 105: {
			Get();
			break;
		}
		case 106: {
			Get();
			break;
		}
		default: SynErr(109); break;
		}
		Add(t.pos, t.val.Length, TextStyle.Style2); 
	}

	void Value() {
		if (la.kind == 1) {
			Get();
			Add(t.pos, t.val.Length, TextStyle.Style1); 
		} else if (la.kind == 6) {
			Get();
			Add(t.pos, t.val.Length, TextStyle.Style3); 
		} else if (la.kind == 2) {
			Get();
			Add(t.pos, t.val.Length, TextStyle.Style6); 
		} else if (la.kind == 4) {
			Get();
			Add(t.pos, t.val.Length, TextStyle.Style5); 
		} else if (la.kind == 3) {
			Get();
			Add(t.pos, t.val.Length, TextStyle.Style8); 
		} else SynErr(110);
	}

	void Code() {
		if (la.kind == 9) {
			OpNum();
		} else if (StartOf(2)) {
			Keyword();
		} else if (StartOf(3)) {
			Value();
		} else if (la.kind == 8) {
			SingleLineComment();
		} else if (la.kind == 7) {
			Get();
		} else SynErr(111);
	}

	void Eil() {
		Code();
		while (StartOf(4)) {
			Code();
		}
	}



	public void Parse() {
		la = new Token();
		la.val = "";		
		Get();
		Eil();

    Expect(0);
	}
	
	static readonly bool[,] set = {
		{T,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,T,T,x, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, x},
		{x,x,x,x, x,x,x,x, x,x,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x},
		{x,T,T,T, T,x,T,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x,x,x,x, x},
		{x,T,T,T, T,x,T,T, T,T,x,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,T, T,T,T,x, x}

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
			case 2: s = "intTok expected"; break;
			case 3: s = "address expected"; break;
			case 4: s = "stringTok expected"; break;
			case 5: s = "charTok expected"; break;
			case 6: s = "operatorTok expected"; break;
			case 7: s = "NL expected"; break;
			case 8: s = "\"//\" expected"; break;
			case 9: s = "\"[\" expected"; break;
			case 10: s = "\"]\" expected"; break;
			case 11: s = "\"Nop\" expected"; break;
			case 12: s = "\"Len\" expected"; break;
			case 13: s = "\"Pushunit\" expected"; break;
			case 14: s = "\"Pushelem\" expected"; break;
			case 15: s = "\"Pushfld\" expected"; break;
			case 16: s = "\"Hasfld\" expected"; break;
			case 17: s = "\"PushI4_0\" expected"; break;
			case 18: s = "\"PushI1_0\" expected"; break;
			case 19: s = "\"PushI1_1\" expected"; break;
			case 20: s = "\"Pop\" expected"; break;
			case 21: s = "\"Pushstr_0\" expected"; break;
			case 22: s = "\"Genfin\" expected"; break;
			case 23: s = "\"Cons\" expected"; break;
			case 24: s = "\"Tail\" expected"; break;
			case 25: s = "\"Head\" expected"; break;
			case 26: s = "\"Ret\" expected"; break;
			case 27: s = "\"Concat\" expected"; break;
			case 28: s = "\"Add\" expected"; break;
			case 29: s = "\"Mul\" expected"; break;
			case 30: s = "\"Div\" expected"; break;
			case 31: s = "\"Quot\" expected"; break;
			case 32: s = "\"Rem\" expected"; break;
			case 33: s = "\"Mod\" expected"; break;
			case 34: s = "\"Pow\" expected"; break;
			case 35: s = "\"Sub\" expected"; break;
			case 36: s = "\"Shr\" expected"; break;
			case 37: s = "\"Shl\" expected"; break;
			case 38: s = "\"Ceq\" expected"; break;
			case 39: s = "\"Cneq\" expected"; break;
			case 40: s = "\"Clt\" expected"; break;
			case 41: s = "\"Cgt\" expected"; break;
			case 42: s = "\"Cgteq\" expected"; break;
			case 43: s = "\"Clteq\" expected"; break;
			case 44: s = "\"AndBw\" expected"; break;
			case 45: s = "\"OrBw\" expected"; break;
			case 46: s = "\"Xor\" expected"; break;
			case 47: s = "\"Not\" expected"; break;
			case 48: s = "\"Neg\" expected"; break;
			case 49: s = "\"NotBw\" expected"; break;
			case 50: s = "\"Dup\" expected"; break;
			case 51: s = "\"Swap\" expected"; break;
			case 52: s = "\"Newlazy\" expected"; break;
			case 53: s = "\"Newlist\" expected"; break;
			case 54: s = "\"Newtup_2\" expected"; break;
			case 55: s = "\"Stop\" expected"; break;
			case 56: s = "\"NewI8\" expected"; break;
			case 57: s = "\"NewR8\" expected"; break;
			case 58: s = "\"Leave\" expected"; break;
			case 59: s = "\"Flip\" expected"; break;
			case 60: s = "\"LazyCall\" expected"; break;
			case 61: s = "\"Call\" expected"; break;
			case 62: s = "\"Callt\" expected"; break;
			case 63: s = "\"Ctx\" expected"; break;
			case 64: s = "\"Throw\" expected"; break;
			case 65: s = "\"Rethrow\" expected"; break;
			case 66: s = "\"Force\" expected"; break;
			case 67: s = "\"Isnil\" expected"; break;
			case 68: s = "\"Show\" expected"; break;
			case 69: s = "\"Addmbr\" expected"; break;
			case 70: s = "\"Traitch\" expected"; break;
			case 71: s = "\"Skiptag\" expected"; break;
			case 72: s = "\"Newtype\" expected"; break;
			case 73: s = "\"Newtype0\" expected"; break;
			case 74: s = "\"Newtype1\" expected"; break;
			case 75: s = "\"Newtype2\" expected"; break;
			case 76: s = "\"Ctype\" expected"; break;
			case 77: s = "\"Disp\" expected"; break;
			case 78: s = "\"Newconst\" expected"; break;
			case 79: s = "\"Api\" expected"; break;
			case 80: s = "\"Api2\" expected"; break;
			case 81: s = "\"Untag\" expected"; break;
			case 82: s = "\"Reccons\" expected"; break;
			case 83: s = "\"Tupcons\" expected"; break;
			case 84: s = "\"Ctorid\" expected"; break;
			case 85: s = "\"Typeid\" expected"; break;
			case 86: s = "\"Classid\" expected"; break;
			case 87: s = "\"Newfunc\" expected"; break;
			case 88: s = "\"Newmod\" expected"; break;
			case 89: s = "\"Pushext\" expected"; break;
			case 90: s = "\"Newrec\" expected"; break;
			case 91: s = "\"Newtup\" expected"; break;
			case 92: s = "\"Failwith\" expected"; break;
			case 93: s = "\"Start\" expected"; break;
			case 94: s = "\"Pushstr\" expected"; break;
			case 95: s = "\"PushCh\" expected"; break;
			case 96: s = "\"PushI4\" expected"; break;
			case 97: s = "\"PushR4\" expected"; break;
			case 98: s = "\"Pushloc\" expected"; break;
			case 99: s = "\"Pushvar\" expected"; break;
			case 100: s = "\"Poploc\" expected"; break;
			case 101: s = "\"Popvar\" expected"; break;
			case 102: s = "\"Runmod\" expected"; break;
			case 103: s = "\"Br\" expected"; break;
			case 104: s = "\"Brtrue\" expected"; break;
			case 105: s = "\"Brfalse\" expected"; break;
			case 106: s = "\"Newfun\" expected"; break;
			case 107: s = "??? expected"; break;
			case 108: s = "invalid SingleLineComment"; break;
			case 109: s = "invalid Keyword"; break;
			case 110: s = "invalid Value"; break;
			case 111: s = "invalid Code"; break;

			default: s = "error " + n; break;
		}
		
		Console.WriteLine(s + "; line=" + line + ";col=" + col);
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

