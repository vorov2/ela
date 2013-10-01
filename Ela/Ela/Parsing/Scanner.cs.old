
using System;
using System.IO;
using System.Collections;
using System.Text;


namespace Ela.Parsing {

internal sealed class Token {
	public int kind;    // token kind
	public int pos;     // token position in the source text (starting at 0)
	public int col;     // token column (starting at 1)
	public int line;    // token line (starting at 1)
	public string val;  // token value
	public bool virt;
	public Token next;  // ML 2005-03-11 Tokens are kept in linked list
}

//-----------------------------------------------------------------------------------
// Buffer
//-----------------------------------------------------------------------------------
internal class Buffer : IDisposable {
	// This Buffer supports the following cases:
	// 1) seekable stream (file)
	//    a) whole stream in buffer
	//    b) part of stream in buffer
	// 2) non seekable stream (network, console)

	public const int EOF = char.MaxValue + 1;
	const int MIN_BUFFER_LENGTH = 1024; // 1KB
	const int MAX_BUFFER_LENGTH = MIN_BUFFER_LENGTH * 64; // 64KB
	byte[] buf;         // input buffer
	int bufStart;       // position of first byte in buffer relative to input stream
	int bufLen;         // length of buffer
	int fileLen;        // length of input stream (may change if the stream is no file)
	int bufPos;         // current position in buffer
	Stream stream;      // input stream (seekable)
	bool isUserStream;  // was the stream opened by the user?
	
	public Buffer (Stream s, bool isUserStream) {
		stream = s; this.isUserStream = isUserStream;
		
		if (stream.CanSeek) {
			fileLen = (int) stream.Length;
			bufLen = Math.Min(fileLen, MAX_BUFFER_LENGTH);
			bufStart = Int32.MaxValue; // nothing in the buffer so far
		} else {
			fileLen = bufLen = bufStart = 0;
		}

		buf = new byte[(bufLen>0) ? bufLen : MIN_BUFFER_LENGTH];
		if (fileLen > 0) Pos = 0; // setup buffer to position 0 (start)
		else bufPos = 0; // index 0 is already after the file, thus Pos = 0 is invalid
		if (bufLen == fileLen && stream.CanSeek) Close();
	}
		
	protected Buffer(Buffer b) { // called in UTF8Buffer constructor
		buf = b.buf;
		bufStart = b.bufStart;
		bufLen = b.bufLen;
		fileLen = b.fileLen;
		bufPos = b.bufPos;
		stream = b.stream;
		// keep destructor from closing the stream
		b.stream = null;
		isUserStream = b.isUserStream;
	}

	~Buffer() { Close(); }
	
	void IDisposable.Dispose() {
		Close();
	}
	
	protected void Close() {
		if (!isUserStream && stream != null) {
			stream.Close();
			stream = null;
			GC.SuppressFinalize(this);
		}
	}
	
	public virtual int Read () {
		if (bufPos < bufLen) {
			return buf[bufPos++];
		} else if (Pos < fileLen) {
			Pos = Pos; // shift buffer start to Pos
			return buf[bufPos++];
		} else if (stream != null && !stream.CanSeek && ReadNextStreamChunk() > 0) {
			return buf[bufPos++];
		} else {
			return EOF;
		}
	}

	public int Peek () {
		int curPos = Pos;
		int ch = Read();
		Pos = curPos;
		return ch;
	}
	
	public string GetString (int beg, int end) {
		int len = 0;
		char[] buf = new char[end - beg];
		int oldPos = Pos;
		Pos = beg;
		while (Pos < end) buf[len++] = (char) Read();
		Pos = oldPos;
		return new String(buf, 0, len);
	}

	public int Pos {
		get { return bufPos + bufStart; }
		set {
			if (value >= fileLen && stream != null && !stream.CanSeek) {
				// Wanted position is after buffer and the stream
				// is not seek-able e.g. network or console,
				// thus we have to read the stream manually till
				// the wanted position is in sight.
				while (value >= fileLen && ReadNextStreamChunk() > 0);
			}

			if (value < 0 || value > fileLen) {
				throw new ElaParserException("Buffer out of bounds access, position: " + value);
			}

			if (value >= bufStart && value < bufStart + bufLen) { // already in buffer
				bufPos = value - bufStart;
			} else if (stream != null) { // must be swapped in
				stream.Seek(value, SeekOrigin.Begin);
				bufLen = stream.Read(buf, 0, buf.Length);
				bufStart = value; bufPos = 0;
			} else {
				// set the position to the end of the file, Pos will return fileLen.
				bufPos = fileLen - bufStart;
			}
		}
	}
	
	// Read the next chunk of bytes from the stream, increases the buffer
	// if needed and updates the fields fileLen and bufLen.
	// Returns the number of bytes read.
	private int ReadNextStreamChunk() {
		int free = buf.Length - bufLen;
		if (free == 0) {
			// in the case of a growing input stream
			// we can neither seek in the stream, nor can we
			// foresee the maximum length, thus we must adapt
			// the buffer size on demand.
			byte[] newBuf = new byte[bufLen * 2];
			Array.Copy(buf, newBuf, bufLen);
			buf = newBuf;
			free = bufLen;
		}
		int read = stream.Read(buf, bufLen, free);
		if (read > 0) {
			fileLen = bufLen = (bufLen + read);
			return read;
		}
		// end of stream reached
		return 0;
	}
}

//-----------------------------------------------------------------------------------
// UTF8Buffer
//-----------------------------------------------------------------------------------
internal sealed class UTF8Buffer: Buffer {
	public UTF8Buffer(Buffer b): base(b) {}

	public override int Read() {
		int ch;
		do {
			ch = base.Read();
			// until we find a utf8 start (0xxxxxxx or 11xxxxxx)
		} while ((ch >= 128) && ((ch & 0xC0) != 0xC0) && (ch != EOF));
		if (ch < 128 || ch == EOF) {
			// nothing to do, first 127 chars are the same in ascii and utf8
			// 0xxxxxxx or end of file character
		} else if ((ch & 0xF0) == 0xF0) {
			// 11110xxx 10xxxxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x07; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F; ch = base.Read();
			int c4 = ch & 0x3F;
			ch = (((((c1 << 6) | c2) << 6) | c3) << 6) | c4;
		} else if ((ch & 0xE0) == 0xE0) {
			// 1110xxxx 10xxxxxx 10xxxxxx
			int c1 = ch & 0x0F; ch = base.Read();
			int c2 = ch & 0x3F; ch = base.Read();
			int c3 = ch & 0x3F;
			ch = (((c1 << 6) | c2) << 6) | c3;
		} else if ((ch & 0xC0) == 0xC0) {
			// 110xxxxx 10xxxxxx
			int c1 = ch & 0x1F; ch = base.Read();
			int c2 = ch & 0x3F;
			ch = (c1 << 6) | c2;
		}
		return ch;
	}
}

//-----------------------------------------------------------------------------------
// Scanner
//-----------------------------------------------------------------------------------
internal sealed class Scanner {
	const char EOL = '\n';
	const int eofSym = 0; /* pdt */
	
	const int maxT = 67;
	const int noSym = 67;


	public Buffer buffer; // scanner buffer
	
	Token t;          // current token
	int ch;           // current input character
	int pos;          // byte position of current character
	int col;          // column number of current character
	int line;         // line number of current character
	int oldEols;      // EOLs that appeared in a comment;
	static readonly Hashtable start; // maps first token character to start state

	Token tokens;     // list of tokens already peeked (first token is a dummy)
	Token pt;         // current peek token
	
	char[] tval = new char[128]; // text of current token
	int tlen;         // length of current token
	
	static Scanner() {
		start = new Hashtable(128);
		for (int i = 97; i <= 122; ++i) start[i] = 1;
		for (int i = 65; i <= 90; ++i) start[i] = 4;
		for (int i = 49; i <= 57; ++i) start[i] = 66;
		start[95] = 94; 
		start[39] = 67; 
		start[48] = 68; 
		start[45] = 69; 
		start[46] = 70; 
		start[34] = 21; 
		start[60] = 71; 
		start[61] = 44; 
		start[62] = 72; 
		start[94] = 47; 
		start[64] = 48; 
		start[43] = 49; 
		for (int i = 37; i <= 37; ++i) start[i] = 51;
		for (int i = 42; i <= 42; ++i) start[i] = 51;
		for (int i = 47; i <= 47; ++i) start[i] = 51;
		start[58] = 53; 
		for (int i = 33; i <= 33; ++i) start[i] = 55;
		for (int i = 63; i <= 63; ++i) start[i] = 55;
		for (int i = 126; i <= 126; ++i) start[i] = 55;
		start[124] = 73; 
		start[36] = 58; 
		start[38] = 60; 
		start[123] = 61; 
		start[125] = 62; 
		start[91] = 95; 
		start[93] = 63; 
		start[92] = 74; 
		start[59] = 65; 
		start[35] = 88; 
		start[40] = 89; 
		start[41] = 90; 
		start[44] = 91; 
		start[96] = 93; 
		start[Buffer.EOF] = -1;

	}
	
	public Scanner (string fileName) {
		try {
			Stream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
			buffer = new Buffer(stream, false);
			Init();
		} catch (IOException) {
			throw new ElaParserException("Cannot open file " + fileName);
		}
	}
		
	
	public Scanner (Stream s) {
		buffer = new Buffer(s, true);
		Init();
	}
	
	void Init() {
		pos = -1; line = 1; col = 0;
		oldEols = 0;
		NextCh();
		if (ch == 0xEF) { // check optional byte order mark for UTF-8
			NextCh(); int ch1 = ch;
			NextCh(); int ch2 = ch;
			if (ch1 != 0xBB || ch2 != 0xBF) {
				throw new ElaParserException(String.Format("Illegal byte order mark: EF {0,2:X} {1,2:X}", ch1, ch2));
			}
			buffer = new UTF8Buffer(buffer); col = 0;
			NextCh();
		}
		pt = tokens = new Token();  // first token is a dummy
	}
	
	void NextCh() {
		if (oldEols > 0) { ch = EOL; oldEols--; } 
		else {
			pos = buffer.Pos;
			ch = buffer.Read(); col++;
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
		int level = 1, pos0 = pos, line0 = line, col0 = col;
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
			buffer.Pos = pos0; NextCh(); line = line0; col = col0;
		}
		return false;
	}

	bool Comment1() {
		int level = 1, pos0 = pos, line0 = line, col0 = col;
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
			buffer.Pos = pos0; NextCh(); line = line0; col = col0;
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
			case "raise": t.kind = 38; break;
			case "try": t.kind = 39; break;
			case "true": t.kind = 40; break;
			case "false": t.kind = 41; break;
			case "fail": t.kind = 42; break;
			case "where": t.kind = 43; break;
			case "instance": t.kind = 44; break;
			case "type": t.kind = 45; break;
			case "class": t.kind = 46; break;
			case "import": t.kind = 47; break;
			case "data": t.kind = 48; break;
			case "opentype": t.kind = 49; break;
			case "!": t.kind = 52; break;
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
			ch == 10 || ch == 13
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
		int apx = 0;
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
		t.pos = pos; t.col = col; t.line = line; 
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
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 75;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 76;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {goto case 0;}
			case 4:
				recEnd = pos; recKind = 2;
				if (ch == 39 || ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 4;}
				else {t.kind = 2; break;}
			case 5:
				{
					tlen -= apx;
					SetScannerBehindT();
					t.kind = 3; break;}
			case 6:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 7;}
				else {goto case 0;}
			case 7:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 7;}
				else if (ch == 'L' || ch == 'l') {AddCh(); goto case 8;}
				else {t.kind = 3; break;}
			case 8:
				{t.kind = 3; break;}
			case 9:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else if (ch == 'D' || ch == 'F' || ch == 'd' || ch == 'f') {AddCh(); goto case 20;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 10;}
				else {t.kind = 4; break;}
			case 10:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 12;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 11;}
				else {goto case 0;}
			case 11:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 12;}
				else {goto case 0;}
			case 12:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 12;}
				else if (ch == 'D' || ch == 'F' || ch == 'd' || ch == 'f') {AddCh(); goto case 20;}
				else {t.kind = 4; break;}
			case 13:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 13;}
				else if (ch == 'D' || ch == 'F' || ch == 'd' || ch == 'f') {AddCh(); goto case 20;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 14;}
				else {t.kind = 4; break;}
			case 14:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 16;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 15;}
				else {goto case 0;}
			case 15:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 16;}
				else {goto case 0;}
			case 16:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 16;}
				else if (ch == 'D' || ch == 'F' || ch == 'd' || ch == 'f') {AddCh(); goto case 20;}
				else {t.kind = 4; break;}
			case 17:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 19;}
				else if (ch == '+' || ch == '-') {AddCh(); goto case 18;}
				else {goto case 0;}
			case 18:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 19;}
				else {goto case 0;}
			case 19:
				recEnd = pos; recKind = 4;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 19;}
				else if (ch == 'D' || ch == 'F' || ch == 'd' || ch == 'f') {AddCh(); goto case 20;}
				else {t.kind = 4; break;}
			case 20:
				{t.kind = 4; break;}
			case 21:
				if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '!' || ch >= '#' && ch <= '[' || ch >= ']' && ch <= 65535) {AddCh(); goto case 21;}
				else if (ch == '"') {AddCh(); goto case 32;}
				else if (ch == 92) {AddCh(); goto case 77;}
				else {goto case 0;}
			case 22:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 23;}
				else {goto case 0;}
			case 23:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 24;}
				else {goto case 0;}
			case 24:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 25;}
				else {goto case 0;}
			case 25:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 21;}
				else {goto case 0;}
			case 26:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 27;}
				else {goto case 0;}
			case 27:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 28;}
				else {goto case 0;}
			case 28:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 29;}
				else {goto case 0;}
			case 29:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 30;}
				else {goto case 0;}
			case 30:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 21;}
				else {goto case 0;}
			case 31:
				if (ch <= 92 || ch >= '^' && ch <= 65535) {AddCh(); goto case 31;}
				else if (ch == ']') {AddCh(); goto case 78;}
				else {goto case 0;}
			case 32:
				{t.kind = 5; break;}
			case 33:
				if (ch == 39) {AddCh(); goto case 43;}
				else {goto case 0;}
			case 34:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 35;}
				else {goto case 0;}
			case 35:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 36;}
				else {goto case 0;}
			case 36:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 37;}
				else {goto case 0;}
			case 37:
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'F' || ch >= 'a' && ch <= 'f') {AddCh(); goto case 33;}
				else {goto case 0;}
			case 38:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 39;}
				else {goto case 0;}
			case 39:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 40;}
				else {goto case 0;}
			case 40:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 41;}
				else {goto case 0;}
			case 41:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 42;}
				else {goto case 0;}
			case 42:
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 33;}
				else {goto case 0;}
			case 43:
				{t.kind = 6; break;}
			case 44:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 44;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 45:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 45;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 46:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 46;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 47:
				recEnd = pos; recKind = 8;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 47;}
				else {t.kind = 8; break;}
			case 48:
				recEnd = pos; recKind = 9;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 48;}
				else {t.kind = 9; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 49:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 49;}
				else {t.kind = 10; break;}
			case 50:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 50;}
				else {t.kind = 10; break;}
			case 51:
				recEnd = pos; recKind = 11;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 51;}
				else {t.kind = 11; break;}
			case 52:
				recEnd = pos; recKind = 12;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 52;}
				else {t.kind = 12; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 53:
				recEnd = pos; recKind = 12;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 53;}
				else {t.kind = 12; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 54:
				recEnd = pos; recKind = 13;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 54;}
				else {t.kind = 13; break;}
			case 55:
				recEnd = pos; recKind = 14;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 55;}
				else {t.kind = 14; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 56:
				recEnd = pos; recKind = 14;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 56;}
				else {t.kind = 14; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 57:
				recEnd = pos; recKind = 15;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 57;}
				else {t.kind = 15; break;}
			case 58:
				recEnd = pos; recKind = 16;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 58;}
				else {t.kind = 16; break;}
			case 59:
				recEnd = pos; recKind = 17;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 59;}
				else {t.kind = 17; break;}
			case 60:
				recEnd = pos; recKind = 18;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 60;}
				else {t.kind = 18; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 61:
				{t.kind = 19; break;}
			case 62:
				{t.kind = 20; break;}
			case 63:
				{t.kind = 22; break;}
			case 64:
				{t.kind = 26; break;}
			case 65:
				{t.kind = 50; break;}
			case 66:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 66;}
				else if (ch == 'L' || ch == 'l') {AddCh(); goto case 8;}
				else if (ch == '.') {apx++; AddCh(); goto case 81;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 17;}
				else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {AddCh(); goto case 20;}
				else {t.kind = 3; break;}
			case 67:
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 82;}
				else if (ch <= 9 || ch >= 11 && ch <= 12 || ch >= 14 && ch <= '&' || ch >= '(' && ch <= '@' || ch == '[' || ch >= ']' && ch <= '^' || ch == '`' || ch >= '{' && ch <= 65535) {AddCh(); goto case 33;}
				else if (ch == 92) {AddCh(); goto case 79;}
				else {goto case 0;}
			case 68:
				recEnd = pos; recKind = 3;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 66;}
				else if (ch == 'L' || ch == 'l') {AddCh(); goto case 8;}
				else if (ch == '.') {apx++; AddCh(); goto case 81;}
				else if (ch == 'X' || ch == 'x') {AddCh(); goto case 6;}
				else if (ch == 'E' || ch == 'e') {AddCh(); goto case 17;}
				else if (ch == 'D' || ch == 'F' || ch == 'M' || ch == 'd' || ch == 'f' || ch == 'm') {AddCh(); goto case 20;}
				else {t.kind = 3; break;}
			case 69:
				recEnd = pos; recKind = 10;
				if (ch >= '1' && ch <= '9') {AddCh(); goto case 66;}
				else if (ch == '0') {AddCh(); goto case 68;}
				else if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch == '-' || ch == '/' || ch == ':' || ch >= '<' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 80;}
				else if (ch == '.') {AddCh(); goto case 83;}
				else if (ch == '>') {AddCh(); goto case 84;}
				else {t.kind = 10; break;}
			case 70:
				recEnd = pos; recKind = 27;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 56;}
				else {t.kind = 27; break;}
			case 71:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '=' && ch <= '@' || ch == 92 || ch == '^' || ch == '~') {AddCh(); goto case 46;}
				else if (ch == '[') {AddCh(); goto case 31;}
				else if (ch == '<') {AddCh(); goto case 52;}
				else if (ch == '|') {AddCh(); goto case 57;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 72:
				recEnd = pos; recKind = 7;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 45;}
				else if (ch == '>') {AddCh(); goto case 54;}
				else {t.kind = 7; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 73:
				recEnd = pos; recKind = 23;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '=' || ch >= '?' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 59;}
				else if (ch == '>') {AddCh(); goto case 58;}
				else {t.kind = 23; break;}
			case 74:
				recEnd = pos; recKind = 25;
				if (ch == 92) {AddCh(); goto case 64;}
				else {t.kind = 25; break;}
			case 75:
				recEnd = pos; recKind = 1;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 75;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 76;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 76:
				recEnd = pos; recKind = 1;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 75;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 76;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 77:
				if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch == 'b' || ch == 'n' || ch == 'r' || ch == 't') {AddCh(); goto case 21;}
				else if (ch == 'u') {AddCh(); goto case 22;}
				else if (ch == 'x') {AddCh(); goto case 26;}
				else {goto case 0;}
			case 78:
				if (ch <= '=' || ch >= '?' && ch <= 92 || ch >= '^' && ch <= 65535) {AddCh(); goto case 31;}
				else if (ch == ']') {AddCh(); goto case 78;}
				else if (ch == '>') {AddCh(); goto case 85;}
				else {goto case 0;}
			case 79:
				if (ch == '"' || ch == 39 || ch == '0' || ch == 92 || ch == 'b' || ch == 'n' || ch == 'r' || ch == 't') {AddCh(); goto case 33;}
				else if (ch == 'u') {AddCh(); goto case 34;}
				else if (ch == 'x') {AddCh(); goto case 38;}
				else {goto case 0;}
			case 80:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 86;}
				else {t.kind = 10; break;}
			case 81:
				if (ch <= '/' || ch >= ':' && ch <= 65535) {apx++; AddCh(); goto case 5;}
				else if (ch >= '0' && ch <= '9') {apx = 0; AddCh(); goto case 13;}
				else {goto case 0;}
			case 82:
				recEnd = pos; recKind = 1;
				if (ch >= '0' && ch <= '9' || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 3;}
				else if (ch == 39) {AddCh(); goto case 87;}
				else {t.kind = 1; t.val = new String(tval, 0, tlen); CheckLiteral(); return t;}
			case 83:
				recEnd = pos; recKind = 10;
				if (ch >= '0' && ch <= '9') {AddCh(); goto case 9;}
				else if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 86;}
				else {t.kind = 10; break;}
			case 84:
				recEnd = pos; recKind = 24;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 50;}
				else {t.kind = 24; break;}
			case 85:
				recEnd = pos; recKind = 5;
				if (ch <= 92 || ch >= '^' && ch <= 65535) {AddCh(); goto case 31;}
				else if (ch == ']') {AddCh(); goto case 78;}
				else {t.kind = 5; break;}
			case 86:
				recEnd = pos; recKind = 10;
				if (ch == '!' || ch >= '$' && ch <= '&' || ch >= '*' && ch <= '+' || ch >= '-' && ch <= '/' || ch == ':' || ch >= '<' && ch <= '@' || ch == 92 || ch == '^' || ch == '|' || ch == '~') {AddCh(); goto case 86;}
				else {t.kind = 10; break;}
			case 87:
				recEnd = pos; recKind = 6;
				if (ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 75;}
				else if (ch >= '0' && ch <= '9') {AddCh(); goto case 76;}
				else if (ch == 39) {AddCh(); goto case 3;}
				else {t.kind = 6; break;}
			case 88:
				{t.kind = 51; break;}
			case 89:
				{t.kind = 54; break;}
			case 90:
				{t.kind = 55; break;}
			case 91:
				{t.kind = 56; break;}
			case 92:
				{t.kind = 59; break;}
			case 93:
				{t.kind = 60; break;}
			case 94:
				recEnd = pos; recKind = 53;
				if (ch == 39 || ch >= 'A' && ch <= 'Z' || ch == '_' || ch >= 'a' && ch <= 'z') {AddCh(); goto case 2;}
				else {t.kind = 53; break;}
			case 95:
				recEnd = pos; recKind = 21;
				if (ch == '&') {AddCh(); goto case 92;}
				else {t.kind = 21; break;}

		}
		t.val = new String(tval, 0, tlen);			
		return t;
	}
	
	private void SetScannerBehindT() {
		buffer.Pos = t.pos;
		NextCh();
		line = t.line; col = t.col;
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

