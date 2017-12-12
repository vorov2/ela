
using System;
using System.IO;
using System.Collections;
using Buffer = Ela.Parsing.StringBuffer;


namespace Ela.Parsing {


//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
internal sealed class Scanner {
	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	const int maxT = 67;
	const int noSym = 67;


	public SourceBuffer buffer; // scanner buffer
	
	Token t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int charPos;      // position by unicode characters starting with 0
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Map start; // maps first token character to start state

	Token tokens;     // list of tokens already peeked (first token is a dummy)
	Token pt;         // current peek token
	
	char[] tval = new char[128]; // text of current token
	int tlen;         // length of current token
	
	static Scanner() {
		start = new Map(128);
		for (int i = 97; i <= 122; ++i) start[i] = 1;
		for (int i = 65; i <= 90; ++i) start[i] = 4;
		for (int i = 49; i <= 57; ++i) start[i] = 60;
		start[95] = 90; 
		start[39] = 61; 
		start[48] = 62; 
		start[45] = 63; 
		start[46] = 64; 
		start[34] = 17; 
		start[60] = 65; 
		start[61] = 40; 
		start[62] = 66; 
		start[94] = 43; 
		start[64] = 44; 
		start[43] = 45; 
		for (int i = 37; i <= 37; ++i) start[i] = 47;
		for (int i = 42; i <= 42; ++i) start[i] = 47;
		for (int i = 47; i <= 47; ++i) start[i] = 47;
		start[58] = 49; 
		for (int i = 33; i <= 33; ++i) start[i] = 51;
		for (int i = 63; i <= 63; ++i) start[i] = 51;
		for (int i = 126; i <= 126; ++i) start[i] = 51;
		start[124] = 67; 
		for (int i = 36; i <= 36; ++i) start[i] = 54;
		for (int i = 44; i <= 44; ++i) start[i] = 54;
		for (int i = 59; i <= 59; ++i) start[i] = 54;
		start[38] = 56; 
		start[123] = 57; 
		start[125] = 58; 
		start[91] = 91; 
		start[93] = 92; 
		start[92] = 68; 
		start[35] = 93; 
		start[40] = 86; 
		start[41] = 87; 
		start[96] = 89; 
		start[Buffer.EOF] = -1;

	}
	
    public Scanner (SourceBuffer buffer) {
        this.buffer = buffer;
        Init();
    }
    
    public Scanner (Stream s) {
        using (var sr = new StreamReader(s))
            buffer = new Buffer(sr.ReadToEnd());
        Init();
    }
    
    void Init() {
        pos = -1; line = 1; col = 0; charPos = -1;
        oldEols = 0;
        NextCh();
        pt = tokens = new Token();  // first token is a dummy
    }
	
	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; } 
		else {
			pos = buffer.Pos;
			// buffer reads unicode chars, if UTF8 has been detected
			ch = buffer.Read(); col++; charPos++;
			// replace isolated '\r' by '\n' in order to make
			// eol handling uniform across Windows, Unix and Mac
			if (ch == '\r' && buffer.Peek() != '\n') ch = EOL;

			if (ch == EOL) { line++; col = 0; }
		}

	}

	void AddCh() {
		if (tlen >= tval.Length) {
			char[] newBuf = new char[2 * tval.Length];
			Array.Copy(tval, 0, newBuf, 0, tval.Length);
			tval = newBuf;
		}
		if (ch != Buffer.EOF) {
			tval[tlen++] = (char) ch;
			NextCh();
		}
	}



	bool Comment0() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '*') {
			NextCh();
			for(;;) {
				if (ch == '*') {
					NextCh();
					if (ch == '/') {
						level--;
						if (level == 0) { oldEols = line - line0; NextCh(); return true; }
						NextCh();
					}
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}

	bool Comment1() {
		int level = 1, pos0 = pos, line0 = line, col0 = col, charPos0 = charPos;
		NextCh();
		if (ch == '/') {
			NextCh();
			for(;;) {
				if (ch == 10) {
					level--;
					if (level == 0) { oldEols = line - line0; NextCh(); return true; }
					NextCh();
				} else if (ch == Buffer.EOF) return false;
				else NextCh();
			}
		} else {
			buffer.Pos = pos0; NextCh(); line = line0; col = col0; charPos = charPos0;
		}
		return false;
	}


	void CheckLiteral() {
		switch (t.val) {
			case "in": t.kind = 28; break;
			case "match": t.kind = 29; break;
			case "@": t.kind = 30; break;
			case "is": t.kind = 31; break;
			case "let": t.kind = 32; break;
			case "open": t.kind = 33; break;
			case "with": t.kind = 34; break;
			case "if": t.kind = 35; break;
			case "else": t.kind = 36; break;
			case "then": t.kind = 37; break;
			case "try": t.kind = 38; break;
			case "true": t.kind = 39; break;
			case "false": t.kind = 40; break;
			case "where": t.kind = 41; break;
			case "instance": t.kind = 42; break;
			case "type": t.kind = 43; break;
			case "class": t.kind = 44; break;
			case "import": t.kind = 45; break;
			case "data": t.kind = 46; break;
			case "opentype": t.kind = 47; break;
			case ";": t.kind = 48; break;
			case "#": t.kind = 51; break;
			case "!": t.kind = 52; break;
			case ",": t.kind = 56; break;
			case "=": t.kind = 57; break;
			case "..": t.kind = 58; break;
			case ":::": t.kind = 61; break;
			case "__internal": t.kind = 62; break;
			case "&": t.kind = 63; break;
			case "<-": t.kind = 64; break;
			case "deriving": t.kind = 65; break;
			case "do": t.kind = 66; break;
			default: break;
		}
	}

    bool nl = true;
	FastStack<Int32> istack = new FastStack<Int32>();
	int expectIndent = 0;
	int indent = -1;
	
	internal void PopIndent() {
		if (istack.Count > 0) {
			istack.Pop();
			expectIndent = istack.Count > 0 ? istack.Peek() : -1;
		}
		else
			expectIndent = -1;		
	}
		
	internal void InjectBlock() {
		istack.Push(t.col);
		expectIndent = t.col;
		indent = -1;
		nl = false;
	}
	
	internal void InjectBlock(int col) {
		istack.Push(col);
		expectIndent = col;
		indent = -1;
		nl = false;
	}

	Token NextToken() {
		while (ch == ' ' ||
			ch == 0 || ch == 10 || ch == 13
		) {
			if (ch == ' ' && nl)
				indent++;
			else if (ch == '\r' || ch == '\n'){			
				nl = true;
				indent = 0;
			}
			else
				nl = false;
			
			NextCh();
		}
		if (ch == '/' && Comment0() ||ch == '/' && Comment1()) return NextToken();
        if (indent > -1 && indent < expectIndent) {
			t = new Token();
			t.pos = pos; t.col = col; t.line = line; t.virt = true;
			t.kind = Parser._EBLOCK;
			istack.Pop();
			
			if (istack.Count > 0)
				expectIndent = istack.Peek();
			else
				indent = expectIndent = -1;
				
			return t;
		}

		int recKind = noSym;
		int recEnd = pos;
		t = new Token();
		t.pos = pos; t.col = col; t.line = line; t.charPos = charPos;
		int state;
		if (start.ContainsKey(ch)) { state = (int) start[ch]; }
		else { state = 0; }
		tlen = 0; AddCh();

        if (state == -1 && istack.Count > 0) {
			t = new Token();
			t.pos = pos; t.col = col; t.line = line; t.virt = true;
			t.kind = Parser._EBLOCK;
			istack.Pop();
			
			if (istack.Count > 0)
				expectIndent = istack.Peek();
			else
				indent = expectIndent = -1;
				
			return t;
		}
		
		switch (state) {
			case -1: { t.kind = eofSym; break; } // NextCh already done
			case 0: {
				if (recKind != noSym) {
					tlen = recEnd - t.pos;
					SetScannerBehindT();
				}
				t.kind = recKind; break;
			} // NextCh already done
			case 1:
				recEnd = pos; recKind = 1;
				if (ch == 39 || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 1;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 2:
				recEnd = pos; recKind = 1;
				if (ch == 39 || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 2;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 3:
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 69;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 70;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {goto case 0;}
			case 4:
				recEnd = pos; recKind = 2;
				if (ch == 39 || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 4;}
				else {t.kind = 2; break;}
			case 5:
				recEnd = pos; recKind = 3;
				if (ch >= 'G' && ch <= 'Z' || ch >= 'g' && ch <= 'z') {AddCh(); goto case 6;}
				else if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 5;}
				else {t.kind = 3; break;}
			case 6:
				{t.kind = 3; break;}
			case 7:
				recEnd = pos; recKind = 4;
				if (ch >= 'A' && ch <= 'D' || ch >= 'F' && ch <= 'Z' || ch >= 'a' && ch <= 'd' || ch >= 'f' && ch <= 'z') {AddCh(); goto case 16;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 7;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 71;}
				else {t.kind = 4; break;}
			case 8:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else {goto case 0;}
			case 9:
				recEnd = pos; recKind = 4;
				if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 16;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else {t.kind = 4; break;}
			case 10:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 11;}
				else {goto case 0;}
			case 11:
				recEnd = pos; recKind = 4;
				if (ch >= 'A' && ch <= 'D' || ch >= 'F' && ch <= 'Z' || ch >= 'a' && ch <= 'd' || ch >= 'f' && ch <= 'z') {AddCh(); goto case 16;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 11;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 72;}
				else {t.kind = 4; break;}
			case 12:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 13;}
				else {goto case 0;}
			case 13:
				recEnd = pos; recKind = 4;
				if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 16;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 13;}
				else {t.kind = 4; break;}
			case 14:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 15;}
				else {goto case 0;}
			case 15:
				recEnd = pos; recKind = 4;
				if (ch >= 'A' && ch <= 'Z' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 16;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 15;}
				else {t.kind = 4; break;}
			case 16:
				{t.kind = 4; break;}
			case 17:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 17;}
				else if (ch == '"') {AddCh(); goto case 28;}
				else if (ch == 92) {AddCh(); goto case 73;}
				else {goto case 0;}
			case 18:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 19;}
				else {goto case 0;}
			case 19:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 20;}
				else {goto case 0;}
			case 20:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 21;}
				else {goto case 0;}
			case 21:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 17;}
				else {goto case 0;}
			case 22:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 23;}
				else {goto case 0;}
			case 23:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 24;}
				else {goto case 0;}
			case 24:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 25;}
				else {goto case 0;}
			case 25:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 26;}
				else {goto case 0;}
			case 26:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 17;}
				else {goto case 0;}
			case 27:
				if (ch <= 92 || ch >= '^' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == ']') {AddCh(); goto case 74;}
				else {goto case 0;}
			case 28:
				{t.kind = 5; break;}
			case 29:
				if (ch == 39) {AddCh(); goto case 39;}
				else {goto case 0;}
			case 30:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 31;}
				else {goto case 0;}
			case 31:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 32;}
				else {goto case 0;}
			case 32:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 33;}
				else {goto case 0;}
			case 33:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 29;}
				else {goto case 0;}
			case 34:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 35;}
				else {goto case 0;}
			case 35:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 36;}
				else {goto case 0;}
			case 36:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 37;}
				else {goto case 0;}
			case 37:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 38;}
				else {goto case 0;}
			case 38:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 29;}
				else {goto case 0;}
			case 39:
				{t.kind = 6; break;}
			case 40:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 40;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 41:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 41;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 42:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 42;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 43:
				recEnd = pos; recKind = 8;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 43;}
				else {t.kind = 8; break;}
			case 44:
				recEnd = pos; recKind = 9;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 44;}
				else {t.kind = 9; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 45:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 45;}
				else {t.kind = 10; break;}
			case 46:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 46;}
				else {t.kind = 10; break;}
			case 47:
				recEnd = pos; recKind = 11;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 47;}
				else {t.kind = 11; break;}
			case 48:
				recEnd = pos; recKind = 12;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 48;}
				else {t.kind = 12; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 49:
				recEnd = pos; recKind = 12;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 49;}
				else {t.kind = 12; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 50:
				recEnd = pos; recKind = 13;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 50;}
				else {t.kind = 13; break;}
			case 51:
				recEnd = pos; recKind = 14;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 51;}
				else {t.kind = 14; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 52:
				recEnd = pos; recKind = 14;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 52;}
				else {t.kind = 14; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 53:
				recEnd = pos; recKind = 15;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 53;}
				else {t.kind = 15; break;}
			case 54:
				recEnd = pos; recKind = 16;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 54;}
				else {t.kind = 16; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 55:
				recEnd = pos; recKind = 17;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 55;}
				else {t.kind = 17; break;}
			case 56:
				recEnd = pos; recKind = 18;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 56;}
				else {t.kind = 18; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 57:
				{t.kind = 19; break;}
			case 58:
				{t.kind = 20; break;}
			case 59:
				{t.kind = 26; break;}
			case 60:
				recEnd = pos; recKind = 3;
				if (ch >= 'A' && ch <= 'D' || ch >= 'F' && ch <= 'Z' || ch >= 'a' && ch <= 'd' || ch >= 'f' && ch <= 'z') {AddCh(); goto case 6;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 60;}
				else if (ch == '.') {AddCh(); goto case 10;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 77;}
				else {t.kind = 3; break;}
			case 61:
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 78;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '@' || ch == '[' || ch >= ']' && ch <= '^' || ch == '`' || ch >= '{' && ch <= 65535) {AddCh(); goto case 29;}
				else if (ch == 92) {AddCh(); goto case 75;}
				else {goto case 0;}
			case 62:
				recEnd = pos; recKind = 3;
				if (ch >= 'A' && ch <= 'D' || ch >= 'F' && ch <= 'W' || ch >= 'Y' && ch <= 'Z' || ch >= 'a' && ch <= 'd' || ch >= 'f' && ch <= 'w' || ch >= 'y' && ch <= 'z') {AddCh(); goto case 6;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 60;}
				else if (ch == 'X' || ch == 'x') {AddCh(); goto case 79;}
				else if (ch == '.') {AddCh(); goto case 10;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 77;}
				else {t.kind = 3; break;}
			case 63:
				recEnd = pos; recKind = 10;
				if (ch >= '1' && ch <= '9') {AddCh(); goto case 60;}
				else if (ch == '0') {AddCh(); goto case 62;}
				else if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '-' || ch == '/' || ch >= ':' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 76;}
				else if (ch == '.') {AddCh(); goto case 80;}
				else if (ch == '>') {AddCh(); goto case 81;}
				else {t.kind = 10; break;}
			case 64:
				recEnd = pos; recKind = 27;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 7;}
				else if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 52;}
				else {t.kind = 27; break;}
			case 65:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= ';' || ch >= '=' && ch <= '@' || ch == 92 || ch == '^' || ch == '~') {AddCh(); goto case 42;}
				else if (ch == '[') {AddCh(); goto case 27;}
				else if (ch == '<') {AddCh(); goto case 48;}
				else if (ch == '|') {AddCh(); goto case 53;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 66:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 41;}
				else if (ch == '>') {AddCh(); goto case 50;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 67:
				recEnd = pos; recKind = 23;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 55;}
				else if (ch == '>') {AddCh(); goto case 54;}
				else {t.kind = 23; break;}
			case 68:
				recEnd = pos; recKind = 25;
				if (ch == 92) {AddCh(); goto case 59;}
				else {t.kind = 25; break;}
			case 69:
				recEnd = pos; recKind = 1;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 69;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 70;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 70:
				recEnd = pos; recKind = 1;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 69;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 70;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 71:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 8;}
				else {t.kind = 4; break;}
			case 72:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 13;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 12;}
				else {t.kind = 4; break;}
			case 73:
				if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch == 'b' || ch == 'n' || ch == 'r' || ch == 't') {AddCh(); goto case 17;}
				else if (ch == 'u') {AddCh(); goto case 18;}
				else if (ch == 'x') {AddCh(); goto case 22;}
				else {goto case 0;}
			case 74:
				if (ch <= '=' || ch >= '?' && ch <= 65535) {AddCh(); goto case 27;}
				else if (ch == '>') {AddCh(); goto case 28;}
				else {goto case 0;}
			case 75:
				if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch == 'b' || ch == 'n' || ch == 'r' || ch == 't') {AddCh(); goto case 29;}
				else if (ch == 'u') {AddCh(); goto case 30;}
				else if (ch == 'x') {AddCh(); goto case 34;}
				else {goto case 0;}
			case 76:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 82;}
				else {t.kind = 10; break;}
			case 77:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 15;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 14;}
				else {t.kind = 3; break;}
			case 78:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 3;}
				else if (ch == 39) {AddCh(); goto case 83;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 79:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 5;}
				else {t.kind = 3; break;}
			case 80:
				recEnd = pos; recKind = 10;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 7;}
				else if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 82;}
				else {t.kind = 10; break;}
			case 81:
				recEnd = pos; recKind = 24;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 46;}
				else {t.kind = 24; break;}
			case 82:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 82;}
				else {t.kind = 10; break;}
			case 83:
				recEnd = pos; recKind = 6;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 69;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 70;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 6; break;}
			case 84:
				{t.kind = 49; break;}
			case 85:
				{t.kind = 50; break;}
			case 86:
				{t.kind = 54; break;}
			case 87:
				{t.kind = 55; break;}
			case 88:
				{t.kind = 59; break;}
			case 89:
				{t.kind = 60; break;}
			case 90:
				recEnd = pos; recKind = 53;
				if (ch == 39 || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 2;}
				else {t.kind = 53; break;}
			case 91:
				recEnd = pos; recKind = 21;
				if (ch == '&') {AddCh(); goto case 88;}
				else {t.kind = 21; break;}
			case 92:
				recEnd = pos; recKind = 22;
				if (ch == '#') {AddCh(); goto case 85;}
				else {t.kind = 22; break;}
			case 93:
				recEnd = pos; recKind = 9;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 44;}
				else if (ch == '#') {AddCh(); goto case 94;}
				else {t.kind = 9; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 94:
				recEnd = pos; recKind = 9;
				if (ch == '!' || ch >= '#' && ch <= '&' || ch >= '*' && ch <= '/' || ch >= ':' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 44;}
				else if (ch == '[') {AddCh(); goto case 84;}
				else {t.kind = 9; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}

		}
		t.val = new String(tval, 0, tlen);
		return t;
	}
	
	private void SetScannerBehindT() {
		buffer.Pos = t.pos;
		NextCh();
		line = t.line; col = t.col; charPos = t.charPos;
		for (int i = 0; i < tlen; i++) NextCh();
	}
	
	// get the next token (possibly a token already seen during peeking)
	public Token Scan () {
		if (tokens.next == null) {
			return NextToken();
		} else {
			pt = tokens = tokens.next;
			return tokens;
		}
	}

	// peek for the next token, ignore pragmas
	public Token Peek () {
		do {
			if (pt.next == null) {
				pt.next = NextToken();
			}
			pt = pt.next;
		} while (pt.kind > maxT); // skip pragmas
	
		return pt;
	}

	// make sure that peeking starts at the current scan position
	public void ResetPeek () { pt = tokens; }

} // end Scanner

}
