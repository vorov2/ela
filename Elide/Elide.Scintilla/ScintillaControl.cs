using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Elide.Scintilla.Internal;
using Elide.Scintilla.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using Elide.Scintilla.Printing;
using System.Threading;
	
namespace Elide.Scintilla
{
	public class ScintillaControl : BasicScintillaControl
	{
		#region Methods
        protected override void InternalInitialize()
        {
            base.InternalInitialize();
            
            Indicators = new StandardIndicators(Ref);
            Styles = new StandardStyles(Ref);
            Keyboard = new KeyboardManager(Ref);
            Folding = new FoldingManager(Ref);

            Ref.Send(Sci.SCI_USEPOPUP, false);
            Ref.Send(Sci.SCI_CALLTIPUSESTYLE, 0);
            Ref.Send(Sci.SCI_SETLEXER, 0);
            Ref.Send(Sci.SCI_AUTOCSETMAXHEIGHT, 20);
            Ref.Send(Sci.SCI_SETMOUSEDWELLTIME, 500);

            InitializeMargins();
            InitializeMarkers();    
        }

        public void ExecCommand(int command)
        {
            Ref.Send(command);
        }

        public void ResetStyles()
        {
            Ref.Send(Sci.SCI_SETSTYLEBITS, 8);
            Ref.Send(Sci.SCI_STYLECLEARALL);
            Styles.Hyperlink.Hotspot = true;
            Styles.Invisible.Visible = false;
        }

        private void InitializeMargins()
        {
            Ref.Send(Sci.SCI_SETMARGINTYPEN, Sci.MARGIN_FOLDING, Sci.SC_MARGIN_SYMBOL);
            Ref.Send(Sci.SCI_SETMARGINMASKN, Sci.MARGIN_FOLDING, Sci.SC_MASK_FOLDERS);
            Ref.Send(Sci.SCI_SETMARGINWIDTHN, Sci.MARGIN_FOLDING, 12);

            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDERSUB, Pixmap.FromResource("Vline"));
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDERMIDTAIL, Pixmap.FromResource("Tcorner"));            
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDERTAIL, Pixmap.FromResource("Corner"));            
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDEREND, Pixmap.FromResource("BoxPlusConnected"));
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDEROPENMID, Pixmap.FromResource("BoxMinusConnected"));            
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDER, Pixmap.FromResource("BoxPlus"));
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, Sci.SC_MARKNUM_FOLDEROPEN, Pixmap.FromResource("BoxMinus"));
            
            //Ref.Send(Sci.SCI_SETFOLDFLAGS, 16, 0); // 16  	Draw text below if not expanded
            Ref.Send(Sci.SCI_SETMARGINSENSITIVEN, Sci.MARGIN_FOLDING, Sci.TRUE);
            Ref.Send(Sci.SCI_SETFOLDMARGINCOLOUR, true, Color.White.ToScintillaColor());
            //Ref.Send(Sci.SCI_SETMARGINWIDTHN, 0, 0);
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == NativeConstants.OCM_BASE + NativeConstants.WM_NOTIFY)
            {
                var scn = (ScNotification)Marshal.PtrToStructure(m.LParam, typeof(ScNotification));

                switch (scn.NmHdr.Code)
                {
                    case Sci.SCN_UPDATEUI:
                        OnUpdated();

                        if (MatchBraces)
                            PerformMatchBraces();
                        break;
                    case Sci.SCN_CHARADDED:
                        {
                            var cc = '\n';

                            if (Ref.Send(Sci.SCI_GETEOLMODE) == Sci.SC_EOL_CR)
                                cc = '\r';

                            if ((Char)scn.Ch == cc)
                                PerformIndentation();

                            OnCharAdded(scn);
                        }
                        break;
                    case Sci.SCN_MODIFIED:
                        if ((scn.ModificationTypes & Sci.SC_MOD_DELETETEXT) == Sci.SC_MOD_DELETETEXT ||
                            (scn.ModificationTypes & Sci.SC_MOD_INSERTTEXT) == Sci.SC_MOD_INSERTTEXT ||
                            (scn.ModificationTypes & Sci.SC_MULTILINEUNDOREDO) == Sci.SC_MULTILINEUNDOREDO ||
                            (scn.ModificationTypes & Sci.SC_MULTISTEPUNDOREDO) == Sci.SC_MULTISTEPUNDOREDO)
                        {
                            if ((DocumentStateFlags & STATE_ANNOTATION) == STATE_ANNOTATION)
                            {
                                Ref.Send(Sci.SCI_ANNOTATIONCLEARALL);
                                DocumentStateFlags ^= STATE_ANNOTATION;
                            }

                            if ((DocumentStateFlags & STATE_ARROW) == STATE_ARROW)
                            {
                                Ref.Send(Sci.SCI_MARKERDELETEALL, MARKER_ARROW);
                                DocumentStateFlags ^= STATE_ARROW;
                            }

                            OnTextChanged(EventArgs.Empty);
                        }
                        break;
                    case Sci.SCN_STYLENEEDED:
                        OnStyleNeeded(scn);
                        break;
                    case Sci.SCN_HOTSPOTCLICK:
                        OnLinkClicked(CreateLinkClickedEventArgs(scn));
                        break;
                    case Sci.SCN_HOTSPOTDOUBLECLICK:
                        OnLinkDoubleClicked(CreateLinkClickedEventArgs(scn));
                        break;
                    case Sci.SCN_MARGINCLICK:
                        if (scn.Margin == Sci.MARGIN_FOLDING)
                            Ref.Send(Sci.SCI_TOGGLEFOLD, Ref.Send(Sci.SCI_LINEFROMPOSITION, scn.Position));

                        //Ref.Send(Sci.SCI_MARKERADD, Ref.Send(Sci.SCI_LINEFROMPOSITION, scn.Position), 10);
                        break;
                    case Sci.SCN_SAVEPOINTREACHED:
                        OnSavePointReached();
                        break;
                    case Sci.SCN_SAVEPOINTLEFT:
                        OnSavePointLeft();
                        break;

                    case Sci.SCN_DWELLSTART:
                        OnMouseDwell(new DwellEventArgs(scn.X, scn.Y, scn.Position));
                        break;
                    case Sci.SCN_DWELLEND:
                        OnMouseDwellEnd(new DwellEventArgs(scn.X, scn.Y, scn.Position));
                        break;
                }
            }
            else if (m.Msg != 0x000007f8)
                base.WndProc(ref m);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            //Suppress insert of control characters
            if (e.Control && !e.Alt && !e.Shift && e.KeyValue >= 'A' && e.KeyValue <= 'Z')
                e.SuppressKeyPress = true;            
            
            base.OnKeyDown(e);
        }

        private void PerformIndentation()
        {
            if (IndentMode == IndentMode.Block)
                PerformBlockIndentation();
            else if (IndentMode == IndentMode.Smart)
                OnSmartIndentRequired();
        }

        private void PerformBlockIndentation()
        {
            var last = CurrentLine - 1;

            if (last >= 0)
            {
                var indent = GetLineIndentation(last);
                SetLineIndentation(CurrentLine, indent);
                CaretPosition = GetPositionByColumn(CurrentLine, indent);
            }
        }


        private LinkClickedEventArgs CreateLinkClickedEventArgs(ScNotification scn)
        {
            var sp = GetWordBounds(scn.Position, -1);
            var ep = GetWordBounds(scn.Position, 1);
            var src = GetTextRangeUnicode(sp < 0 ? 0 : sp, ep).Trim();
            return new LinkClickedEventArgs(src);
        }


        const int STATE_ANNOTATION = 0x02;
        const int STATE_ARROW = 0x04;
        protected override void ClearDocumentState()
        {
            base.ClearDocumentState();
            Ref.Send(Sci.SCI_ANNOTATIONCLEARALL);
            Ref.Send(Sci.SCI_MARKERDELETEALL, MARKER_ARROW);                            
        }
		

		private int GetWordBounds(int sp, int inc)
		{
			var c = default(Char);
            var cc = inc < 0 ? '=' : '\0';

			while (!Char.IsWhiteSpace(c = CharAt(sp)) 
                && c != '\0' && c != '\r' && c != '\n' && c != cc && c != '(' 
                && c != ')' && c != '[' && c != ']' && c != '<' && c != '>')
				sp += inc;

			return inc < 0 ? sp + 1 : sp;
		}
		#endregion

        #region Foldings
        public void ToggleFold(int line)
        {
            Ref.Send(Sci.SCI_TOGGLEFOLD, line);
        }

        public void CollapseAllFold()
        {
            ChangeAllFold(false);
        }

        public void ExpandAllFold()
        {
            ChangeAllFold(true);
        }

        private void ChangeAllFold(bool expanded)
        {
            for (var i = 0; i < LineCount; i++)
            {
                var exp = Ref.Send(Sci.SCI_GETLINEVISIBLE, i) == Sci.TRUE;

                if (exp != expanded)
                    ToggleFold(i);
            }
        }
        #endregion

        #region Autocomplete
        public void ShowAutocompleteList(int lenEntered, string words)
        {
            var fn = Styles.Default.Font;
            var fs = Styles.Default.FontSize;
            Styles.Default.Font = "Segoe UI";
            Styles.Default.FontSize = 8;
            Ref.Send(Sci.SCI_AUTOCSHOW, lenEntered, words);
            Styles.Default.Font = fn;
        }

        public void RegisterAutocompleteImage(int imageIndex, Pixmap pxm)
        {
            Ref.Send(Sci.SCI_REGISTERIMAGE, imageIndex, pxm.GetPixmap());
        }

        public void ClearRegisteredImages()
        {
            Ref.Send(Sci.SCI_CLEARREGISTEREDIMAGES);
        }

        public bool AutocompleteIgnoreCase
        {
            get { return Ref.Send(Sci.SCI_AUTOCGETIGNORECASE) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_AUTOCSETIGNORECASE, value); }
        }
        #endregion

        #region Indicators
        public void SetIndicator(Indicator indic, int value, int pos, int length)
        {
            Ref.Send(Sci.SCI_SETINDICATORCURRENT, indic.Number);
            Ref.Send(Sci.SCI_SETINDICATORVALUE, value);
            Ref.Send(Sci.SCI_INDICATORFILLRANGE, pos, length);
        }
        
        public void ClearIndicator(Indicator indic, int pos, int length)
        {
            Ref.Send(Sci.SCI_SETINDICATORCURRENT, indic.Number);
            Ref.Send(Sci.SCI_INDICATORCLEARRANGE, pos, length);
        }

        public void ClearIndicator(Indicator indic)
        {
            Ref.Send(Sci.SCI_SETINDICATORCURRENT, indic.Number);
            Ref.Send(Sci.SCI_INDICATORCLEARRANGE, 0, GetTextLength());
        }

        public int GetIndicatorValue(Indicator indic, int position)
        {
            return Ref.Send(Sci.SCI_INDICATORVALUEAT, indic.Number, position);
        }        
        #endregion

        #region Brace Match
        private void PerformMatchBraces()
        {
            var p = CaretPosition;

            if (IsOpenBrace(CharAt(p)))
                HighlightBrace(p);
            else if (IsCloseBrace(CharAt(p - 1)))
                HighlightBrace(p - 1);
            else
                Ref.Send(Sci.SCI_BRACEHIGHLIGHT, -1, -1);
        }

        private void HighlightBrace(int p)
        {
            var matchPos = Ref.Send(Sci.SCI_BRACEMATCH, p);

            if (matchPos != -1)
            {
                Ref.Send(Sci.SCI_BRACEHIGHLIGHT, p, matchPos);
                var col = Ref.Send(Sci.SCI_GETCOLUMN, p);
                Ref.Send(Sci.SCI_SETHIGHLIGHTGUIDE, col);
            }
            else
                Ref.Send(Sci.SCI_BRACEBADLIGHT, p);
        }

        private bool IsOpenBrace(char ch)
        {
            return ch == '[' || ch == '(' || ch == '{';
        }

        private bool IsCloseBrace(char ch)
        {
            return ch == ']' || ch == ')' || ch == '}';
        }

        public bool MatchBraces { get; set; }
        #endregion

        #region Caret Line
        public Color CaretColor
        {
            get { return SciColor.FromScintillaColor(Ref.Send(Sci.SCI_GETCARETFORE)); }
            set { Ref.Send(Sci.SCI_SETCARETFORE, value.ToScintillaColor()); }
        }
        
        public Color CaretLineColor
        {
            get { return SciColor.FromScintillaColor(Ref.Send(Sci.SCI_GETCARETLINEBACK)); }
            set { Ref.Send(Sci.SCI_SETCARETLINEBACK, value.ToScintillaColor()); }
        }

        public bool CaretLineVisible
        {
            get { return Ref.Send(Sci.SCI_GETCARETLINEVISIBLE) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETCARETLINEVISIBLE, value); }
        }

        public int CaretLineAlpha
        {
            get { return Ref.Send(Sci.SCI_GETCARETLINEBACKALPHA); }
            set { Ref.Send(Sci.SCI_SETCARETLINEBACKALPHA, value); }
        }

        public CaretStyle CaretStyle
        {
            get { return (CaretStyle)Ref.Send(Sci.SCI_GETCARETSTYLE); }
            set { Ref.Send(Sci.SCI_SETCARETSTYLE, (Int32)value); }
        }

        public CaretWidth CaretWidth
        {
            get { return (CaretWidth)Ref.Send(Sci.SCI_GETCARETWIDTH); }
            set { Ref.Send(Sci.SCI_SETCARETWIDTH, (Int32)value); }
        }

        public int CaretPeriod
        {
            get { return Ref.Send(Sci.SCI_GETCARETPERIOD); }
            set { Ref.Send(Sci.SCI_SETCARETPERIOD, value); }
        }

        public bool CaretSticky
        {
            get { return Ref.Send(Sci.SCI_GETCARETSTICKY) != 0; }
            set { Ref.Send(Sci.SCI_SETCARETSTICKY, value ? 1 : 0); }
        }
        #endregion

        #region Zoom
        public void ZoomIn()
        {
            Ref.Send(Sci.SCI_ZOOMIN);
        }

        public void ZoomOut()
        {
            Ref.Send(Sci.SCI_ZOOMOUT);
        }

        public void ResetZoom()
        {
            Ref.Send(Sci.SCI_SETZOOM, 0);
        }
        #endregion

        #region Tips
        public void ShowCallTip(string text)
        {
            ShowCallTip(CaretPosition, text);            
        }
        
        public void ShowCallTip(int pos, string text)
        {
            var buffer = Encoding.UTF8.GetBytes(text);
            Ref.Send(Sci.SCI_CALLTIPSHOW, pos, buffer);
        }

        public void HideCallTip()
        {
            Ref.Send(Sci.SCI_CALLTIPCANCEL);
        }
        #endregion

        #region Printing
        internal unsafe int FormatRange(bool draw, ref RangeToFormat pfr)
        {
            fixed (RangeToFormat* rtfp = &pfr)
                return Ref.Send(Sci.SCI_FORMATRANGE, (IntPtr)(draw ? 1 : 0), (IntPtr)rtfp);
        }

        internal void SetPrintColourMode(int mode)
        {
            Ref.Send(Sci.SCI_SETPRINTCOLOURMODE, mode, 0);
        }

        internal void SetPrintMagnification(int magnification)
        {
            Ref.Send(Sci.SCI_SETPRINTMAGNIFICATION, magnification, 0);
        }

        internal void SetPrintWrapMode(int wrapMode)
        {
            Ref.Send(Sci.SCI_SETPRINTWRAPMODE, wrapMode, 0);
        }
        #endregion

        #region Markers
        private const int MARKER_BOOKMARK = 10;
        private const int MARKER_ARROW = 11;

        private void InitializeMarkers()
        {
            var pixmap = Pixmap.FromBitmap((Bitmap)Bitmaps.Load("Bookmark"));
            Ref.Send(Sci.SCI_MARKERDEFINE, MARKER_BOOKMARK, Sci.SC_MARK_PIXMAP);
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, MARKER_BOOKMARK, pixmap.GetPixmap());

            pixmap = Pixmap.FromBitmap((Bitmap)Bitmaps.Load("Arrow"));
            Ref.Send(Sci.SCI_MARKERDEFINE, MARKER_ARROW, Sci.SC_MARK_PIXMAP);
            Ref.Send(Sci.SCI_MARKERDEFINEPIXMAP, MARKER_ARROW, pixmap.GetPixmap());
        }

        public void PutArrow(int line)
        {
            DocumentStateFlags |= STATE_ARROW;
            Ref.Send(Sci.SCI_MARKERDELETEALL, MARKER_ARROW);
            Ref.Send(Sci.SCI_MARKERADD, line, MARKER_ARROW);
        }

        public void ToggleBookmark()
        {
            var bit = Ref.Send(Sci.SCI_MARKERGET, CurrentLine);

            if (bit >> MARKER_BOOKMARK == 1)
            {
                Ref.Send(Sci.SCI_MARKERDELETE, CurrentLine, MARKER_BOOKMARK);
                OnBookmarkRemoved(CurrentLine);
            }
            else
            {
                Ref.Send(Sci.SCI_MARKERADD, CurrentLine, MARKER_BOOKMARK);
                OnBookmarkAdded(CurrentLine);
            }
        }

        public void RemoveBookmark(int line)
        {
            Ref.Send(Sci.SCI_MARKERDELETE, line, MARKER_BOOKMARK);           
        }

        public bool NextBookmark()
        {
            return GoBookmark(false, CurrentLine);
        }

        public bool PreviousBookmark()
        {
            return GoBookmark(true, CurrentLine);
        }

        public void ClearBookmarks()
        {
            Ref.Send(Sci.SCI_MARKERDELETEALL, MARKER_BOOKMARK);
        }
        
        public IEnumerable<Int32> GetAllBookmarks()
        {
        	var line = 0;
        	
        	while (line != -1)
        	{
        		line = Ref.Send(Sci.SCI_MARKERNEXT, line, 1 << MARKER_BOOKMARK);

                if (line != -1)
                {
                    yield return line;
                    line++;
                }
        	}
        }

        private bool GoBookmark(bool prev, int line)
        {
            var res = Ref.Send(prev ? Sci.SCI_MARKERPREVIOUS : Sci.SCI_MARKERNEXT, line, 1 << MARKER_BOOKMARK);

            if (res == CurrentLine)
                return GoBookmark(prev, prev ? CurrentLine - 1 : CurrentLine + 1);
            else if (res != -1)
            {
                GotoLine(res);
                return true;
            }
            else
                return false;
        }
        
        public event EventHandler<BookmarkEventArgs> BookmarkAdded;
        private void OnBookmarkAdded(int line)
        {
        	var h = BookmarkAdded;
        	
        	if (h != null)
        		h(this, new BookmarkEventArgs(line));
        }
        
        public event EventHandler<BookmarkEventArgs> BookmarkRemoved;
        private void OnBookmarkRemoved(int line)
        {
        	var h = BookmarkRemoved;
        	
        	if (h != null)
        		h(this, new BookmarkEventArgs(line));
        }
        #endregion

        #region Annotations
        public void SetAnnotation(int line, string text, TextStyle style)
        {
            DocumentStateFlags |= STATE_ANNOTATION;
            Ref.Send(Sci.SCI_ANNOTATIONSETVISIBLE, 2); //BOXED
            Ref.Send(Sci.SCI_ANNOTATIONSETSTYLE, line, (Int32)style);
            var buffer = Encoding.UTF8.GetBytes(text);
            Ref.Send(Sci.SCI_ANNOTATIONSETTEXT, line, buffer);
        }
        #endregion

        #region Caret
        public void MoveCaretInsideView()
        {
            Ref.Send(Sci.SCI_MOVECARETINSIDEVIEW);
        }
        
        public void MoveCaretBack()
        {
            Ref.Send(Sci.SCI_CHOOSECARETX);
        }
        
        public void MoveCaretToCurrentPosition()
        {
            Ref.Send(Sci.SCI_SCROLLCARET);
        }
        
        public void ScrollToCaret()
        {
            Ref.Send(Sci.SCI_SETYCARETPOLICY, Sci.CARET_SLOP | Sci.CARET_STRICT | Sci.CARET_EVEN, 50);
            Ref.Send(Sci.SCI_SCROLLCARET);
            Ref.Send(Sci.SCI_SETYCARETPOLICY, Sci.CARET_SLOP | Sci.CARET_STRICT | Sci.CARET_EVEN, 0);
        }
        
        public void ScrollLineUp()
        {
            Ref.Send(Sci.SCI_LINESCROLLUP);
        }
        
        public void ScrollLineDown()
        {
            Ref.Send(Sci.SCI_LINESCROLLDOWN);
        }
        
        public void RestoreCaretPosition()
        {
            Ref.Send(Sci.SCI_CHOOSECARETX);
        }

        public bool Overtype
        {
            get { return Ref.Send(Sci.SCI_GETOVERTYPE) > 0; }
            set { Ref.Send(Sci.SCI_SETOVERTYPE, value); }
        }

        public CursorType CursorType
        {
            get { return (CursorType)Ref.Send(Sci.SCI_GETCURSOR); }
            set { Ref.Send(Sci.SCI_SETCURSOR, (Int32)value); }
        }

        public int ScrollPosition
        {
            get { return Ref.Send(Sci.SCI_GETXOFFSET); }
            set { Ref.Send(Sci.SCI_SETXOFFSET, value); }
        }

        public int CaretPosition
        {
            get { return Ref.Send(Sci.SCI_GETCURRENTPOS); }
            set
            {
                Ref.Send(Sci.SCI_SETCURRENTPOS, value);
                Ref.Send(Sci.SCI_SETSELECTIONSTART, value);
            }
        }
        #endregion
        
        #region Text Management
        public void GotoPosition(int position)
		{
			Ref.Send(Sci.SCI_GOTOPOS, position);
		}

		public void MoveToWordEndPosition(int position, bool onlyWordChar)
		{
			Ref.Send(Sci.SCI_WORDENDPOSITION, position, onlyWordChar);
		}
        
		public void MoveToWordStartPosition(int position, bool onlyWordChar)
		{
			Ref.Send(Sci.SCI_WORDSTARTPOSITION, position, onlyWordChar);
		}
        
		public int GetPositionByColumn(int line, int column)
		{
			return Ref.Send(Sci.SCI_FINDCOLUMN, line, column);
		}

		public int GetPositionFromPoint(Point pt, bool alwaysFind, bool onlyChars)
		{
			var msg = alwaysFind && onlyChars ? Sci.SCI_CHARPOSITIONFROMPOINT :
				!alwaysFind && onlyChars ? Sci.SCI_CHARPOSITIONFROMPOINTCLOSE :
				alwaysFind && !onlyChars ? Sci.SCI_POSITIONFROMPOINT :
				!alwaysFind && !onlyChars ? Sci.SCI_POSITIONFROMPOINTCLOSE :
				Sci.NIL;
			return Ref.Send(msg, pt.X, pt.Y);
		}

		public int GetPositionFromLine(int line)
		{
			return Ref.Send(Sci.SCI_POSITIONFROMLINE, line);
		}

		public Point GetPointFromPosition(int position)
		{
			var x = Ref.Send(Sci.SCI_POINTXFROMPOSITION, Sci.NIL, position);
			var y = Ref.Send(Sci.SCI_POINTYFROMPOSITION, Sci.NIL, position);
			return new Point(x, y);
		}
        		
		public void AppendText(string text, bool moveCaret)
		{
            var os = Ref.Send(Sci.SCI_GETREADONLY);
            Ref.Send(Sci.SCI_SETREADONLY);            
            var msg = moveCaret ? Sci.SCI_ADDTEXT : Sci.SCI_APPENDTEXT;
            Ref.Send(msg, new IntPtr(Encoding.UTF8.GetByteCount(text)), Encoding.UTF8.GetBytes(text));
            Ref.Send(Sci.SCI_SETREADONLY, os);

            if (moveCaret)
                Ref.Send(Sci.SCI_SCROLLCARET);
		}
        
		public void InsertText(string text, int position)
		{
            var os = Ref.Send(Sci.SCI_GETREADONLY);
            Ref.Send(Sci.SCI_SETREADONLY);           
            var buffer = Encoding.UTF8.GetBytes(text);
            Ref.Send(Sci.SCI_INSERTTEXT, position, buffer);
            Ref.Send(Sci.SCI_SETREADONLY, os);
		}

		public void ClearAll()
		{
			Ref.Send(Sci.SCI_CLEARALL);
		}
        
		public char CharAt(int position)
		{
			return (Char)Ref.Send(Sci.SCI_GETCHARAT, position);
		}

        public void ReplaceText(int startPos, int endPos, string text)
        {
            Ref.Send(Sci.SCI_SETTARGETSTART, startPos);
            Ref.Send(Sci.SCI_SETTARGETEND, endPos);
            Ref.Send(Sci.SCI_REPLACETARGET, -1, String.Empty);
            InsertText(text, startPos);
        }
        
		public void CutLine()
        {
            Ref.Send(Sci.SCI_LINECUT);
        }

        public void CopyLine()
        {
            Ref.Send(Sci.SCI_LINECOPY);
        }
        
        public void DeleteLine()
        {
            Ref.Send(Sci.SCI_LINEDELETE);
        }

		public void DuplicateLine()
		{
			Ref.Send(Sci.SCI_LINEDUPLICATE);
		}
        
		public void TransposeLine()
		{
			Ref.Send(Sci.SCI_LINETRANSPOSE);
		}

		public void GotoLine(int line)
		{
			Ref.Send(Sci.SCI_GOTOLINE, line);
		}

		public void GotoPos(int pos)
		{
			Ref.Send(Sci.SCI_GOTOPOS, pos);
		}
        
		public void GotoColumn(int col)
		{
			var start = GetPositionFromLine(CurrentLine);
			GotoPos(start + col);
		}
        
		public void LineDown()
		{
			Ref.Send(Sci.SCI_LINEDOWN);
		}
        
		public void LineUp()
		{
			Ref.Send(Sci.SCI_LINEUP);
		}

        public void MoveLinesDown()
        {
            Ref.Send(Sci.SCI_MOVESELECTEDLINESDOWN);
        }

        public void MoveLinesUp()
        {
            Ref.Send(Sci.SCI_MOVESELECTEDLINESUP);
        }

		public void CharLeft()
		{
			Ref.Send(Sci.SCI_CHARLEFT);
		}

		public void CharRight()
		{
			Ref.Send(Sci.SCI_CHARRIGHT);
        }

        public void DeleteWordLeft()
        {
            Ref.Send(Sci.SCI_DELWORDLEFT);
        }

        public void DeleteWordRight()
        {
            Ref.Send(Sci.SCI_DELWORDRIGHT);
        }

        public string GetWordAt(int position)
        {
            var sp = Ref.Send(Sci.SCI_WORDSTARTPOSITION, position, Sci.TRUE);
            var ep = Ref.Send(Sci.SCI_WORDENDPOSITION, position, Sci.TRUE);

            if (sp >= 0 && ep >= 0 && ep > sp)
                return GetTextRangeUnicode(sp, ep);
            else
                return null;
        }

		public void WordLeft()
		{
			Ref.Send(Sci.SCI_WORDLEFT);
		}

		public void WordRight()
		{
			Ref.Send(Sci.SCI_WORDRIGHT);
		}
        
		public void WordPartLeft()
		{
			Ref.Send(Sci.SCI_WORDPARTLEFT);
		}
        
		public void WordPartRight()
		{
			Ref.Send(Sci.SCI_WORDPARTRIGHT);
		}

		public void ParagraphUp()
		{
			Ref.Send(Sci.SCI_PARAUP);
		}
        
		public void ParagraphDown()
		{
			Ref.Send(Sci.SCI_PARADOWN);
		}
        
		public void PageUp()
		{
			Ref.Send(Sci.SCI_PAGEUP);
		}
        
		public void PageDown()
		{
			Ref.Send(Sci.SCI_PAGEDOWN);
		}

		public void DocumentStart()
		{
			Ref.Send(Sci.SCI_DOCUMENTSTART);
		}
        
		public void DocumentEnd()
		{
			Ref.Send(Sci.SCI_DOCUMENTEND);
		}

		public void LineStart()
		{
			Ref.Send(Sci.SCI_HOME);
		}
        
		public void NonBlankLineStart()
		{
			Ref.Send(Sci.SCI_VCHOME);
		}
        
		public void LineEnd()
		{
			Ref.Send(Sci.SCI_LINEEND);
		}
        
		public void VisibleLineEnd()
		{
			Ref.Send(Sci.SCI_LINEENDWRAP);
		}

        public void MakeUppercase()
        {
            Ref.Send(Sci.SCI_UPPERCASE);
        }

        public void MakeLowercase()
        {
            Ref.Send(Sci.SCI_LOWERCASE);
        }

		public int LineCount
		{
			get { return Ref.Send(Sci.SCI_GETLINECOUNT); }
		}

		public int FistVisibleLine
		{
			get { return Ref.Send(Sci.SCI_GETFIRSTVISIBLELINE); }
		}

		public int LinesOnScreen
		{
			get { return Ref.Send(Sci.SCI_LINESONSCREEN); }
		}
        
		public int CurrentPosition
		{
			get { return Ref.Send(Sci.SCI_GETCURRENTPOS); }
			set { Ref.Send(Sci.SCI_SETCURRENTPOS, value); }
		}
        
		public int CurrentLine
		{
			get { return GetLineFromPosition(CurrentPosition); }
		}
        
		public bool ReadOnly
		{
			get { return Ref.Send(Sci.SCI_GETREADONLY) == Sci.TRUE; }
			set { Ref.Send(Sci.SCI_SETREADONLY, value); }
		}
        
		public override string Text
		{
			get { return GetTextUnicode(); }
			set { SetText(value); }
		}
		#endregion

        #region Settings
        public void SetWordChars(string chars)
        {
            Ref.Send(Sci.SCI_SETWORDCHARS, Sci.NIL, chars);
        }

        public void ToggleWrapMode()
        {
            if (WordWrapMode == WordWrapMode.None)
                WordWrapMode = WordWrapMode.Char;
            else
                WordWrapMode = WordWrapMode.None;
        }

        public WordWrapMode WordWrapMode
        {
            get { return (WordWrapMode)Ref.Send(Sci.SCI_GETWRAPMODE); }
            set { Ref.Send(Sci.SCI_SETWRAPMODE, (Int32)value); }
        }

        public bool WordWrapIndicators
        {
            get { return Ref.Send(Sci.SCI_GETWRAPVISUALFLAGS) != Sci.SC_WRAPVISUALFLAG_NONE; }
            set { Ref.Send(Sci.SCI_SETWRAPVISUALFLAGS, value ? Sci.SC_WRAPVISUALFLAG_END : Sci.SC_WRAPVISUALFLAG_NONE); }
        }

        public bool MarginVisible
        {
            get { return Ref.Send(Sci.SCI_GETMARGINWIDTHN, Sci.MARGIN_LINENUMBERS) > 0; }
            set { Ref.Send(Sci.SCI_SETMARGINWIDTHN, Sci.MARGIN_LINENUMBERS, value ? 16 : 0); }
        }

        public Color MarginBackColor
        {
            get { return SciColor.FromScintillaColor(Ref.Send(Sci.SCI_STYLEGETBACK, Sci.STYLE_LINENUMBER)); }
            set { Ref.Send(Sci.SCI_STYLESETBACK, Sci.STYLE_LINENUMBER, value.ToScintillaColor()); }
        }

        public bool UseTabs
        {
            get { return Ref.Send(Sci.SCI_GETUSETABS) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETUSETABS, value); }
        }

        public int TabSize
        {
            get { return Ref.Send(Sci.SCI_GETTABWIDTH); }
            set { Ref.Send(Sci.SCI_SETTABWIDTH, value); }
        }

        public int IndentSize
        {
            get { return Ref.Send(Sci.SCI_GETINDENT); }
            set { Ref.Send(Sci.SCI_SETINDENT, value); }
        }

        public LineEndings LineEndings
        {
            get { return (LineEndings)Ref.Send(Sci.SCI_GETEOLMODE); }
            set
            {
                var val = (Int32)value;
                Ref.Send(Sci.SCI_SETEOLMODE, val);
                Ref.Send(Sci.SCI_CONVERTEOLS, val);
            }
        }

        public bool LongLineIndicator
        {
            get { return Ref.Send(Sci.SCI_GETEDGEMODE) != Sci.EDGE_NONE; }
            set { Ref.Send(Sci.SCI_SETEDGEMODE, value ? Sci.EDGE_LINE : Sci.EDGE_NONE); }
        }

        public int LongLineColumn
        {
            get { return Ref.Send(Sci.SCI_GETEDGECOLUMN); }
            set { Ref.Send(Sci.SCI_SETEDGECOLUMN, value); }
        }

        public IndentMode IndentMode { get; set; }
        #endregion

        #region View
        public bool ViewWhiteSpace
        {
            get { return Ref.Send(Sci.SCI_GETVIEWWS) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETVIEWWS, value); }
        }

        public bool ViewEol
        {
            get { return Ref.Send(Sci.SCI_GETVIEWEOL) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETVIEWEOL, value); }
        }
        #endregion

        #region Indentation
        public int GetLineIndentation(int line)
		{
			return Ref.Send(Sci.SCI_GETLINEINDENTATION, line);
		}
        
		public int GetLineEndColumn(int line)
		{
			return Ref.Send(Sci.SCI_GETLINEENDPOSITION, line) - Ref.Send(Sci.SCI_POSITIONFROMLINE, line);
		}
        
		public int GetLineIndentPosition(int line)
		{
			return Ref.Send(Sci.SCI_GETLINEINDENTPOSITION, line);
		}
        
		public void SetLineIndentation(int line, int indent)
		{
			Ref.Send(Sci.SCI_SETLINEINDENTATION, line, indent);
		}

        public bool IndentationGuides
        {
            get { return Ref.Send(Sci.SCI_GETINDENTATIONGUIDES) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETINDENTATIONGUIDES, value ? Sci.SC_IV_LOOKBOTH : Sci.SC_IV_NONE); }
        }
		#endregion
        
		#region Undo
		public void Undo()
		{
			Ref.Send(Sci.SCI_UNDO);
		}
        
		public void Redo()
		{
			Ref.Send(Sci.SCI_REDO);
		}
                
		public void BeginUndoAction()
		{
			Ref.Send(Sci.SCI_BEGINUNDOACTION);
		}
        
		public void EndUndoAction()
		{
			Ref.Send(Sci.SCI_ENDUNDOACTION);
		}
        
		public void AddUndoAction(int token, bool mayCoalesce)
		{
			Ref.Send(Sci.SCI_ADDUNDOACTION, token, mayCoalesce ?
				Sci.UNDO_MAY_COALESCE : Sci.NIL);
		}
        
		public void SetSavePoint()
		{
			Ref.Send(Sci.SCI_SETSAVEPOINT);
		}
		
		public bool CanUndo()
		{
			return Ref.Send(Sci.SCI_CANUNDO) > 0;
		}
        
		public bool CanRedo()
		{
			return Ref.Send(Sci.SCI_CANREDO) > 0;
		}

		public bool UndoStackEnabled
		{
			get { return Ref.Send(Sci.SCI_GETUNDOCOLLECTION) > 0; }
			set { Ref.Send(Sci.SCI_SETUNDOCOLLECTION, value); }
		}
		#endregion

        #region Clipboard
        public void Cut()
        {
            Ref.Send(Sci.SCI_CUT);
        }

        public void Copy()
        {
            //if (true)//conf.CopyAllowLine)
                Ref.Send(Sci.SCI_COPYALLOWLINE);
            //else
            //    Ref.Send(Sci.SCI_COPY);
        }

        public void Paste()
        {
            Ref.Send(Sci.SCI_PASTE);
        }

        public void SwapClipboard()
        {
            Paste();
            Clipboard.Clear();
        }

        public void Clear()
        {
            Ref.Send(Sci.SCI_CLEAR);
        }

        public void CopyRange(int start, int end)
        {
            Ref.Send(Sci.SCI_COPYRANGE, start, end);
        }

        public void CopyText(string text)
        {
            Ref.Send(Sci.SCI_COPYTEXT, text.Length, text);
        }
        
        public bool CanPaste()
        {
            return Ref.Send(Sci.SCI_CANPASTE) > 0;
        }
        #endregion

        #region Selection
        public Selection GetSelection()
        {
            return new Selection(Ref.Send(Sci.SCI_GETMAINSELECTION), Ref);
        }
        
        public IEnumerable<Selection> GetSelections()
        {
            for (var i = 0; i < SelectionsCount; i++)
                yield return new Selection(i, Ref);
        }
        
        public void Select(int position, int length, SelectionFlags flags)
        {
            if ((flags & SelectionFlags.MakeOnlySelection) == SelectionFlags.MakeOnlySelection)
                Ref.Send(Sci.SCI_SETSELECTION, position, position + length);
            else
            {
                Ref.Send(Sci.SCI_SETSELECTIONSTART, position);
                Ref.Send(Sci.SCI_SETSELECTIONEND, position + length);
            }

            if ((flags & SelectionFlags.ScrollToCaret) == SelectionFlags.ScrollToCaret)
                Ref.Send(Sci.SCI_SCROLLCARET);
        }
        
        public void SelectAll()
        {
            Ref.Send(Sci.SCI_SELECTALL);
        }
        
        public void ClearSelections()
        {
            Ref.Send(Sci.SCI_CLEARSELECTIONS);
        }
        
        public void AddSelection(int caret, int anchor)
        {
            Ref.Send(Sci.SCI_ADDSELECTION, caret, anchor);
        }

        public void DuplicateSelection()
        {
            var sel = new Selection(Ref.Send(Sci.SCI_ROTATESELECTION), Ref);
            var car = Ref.Send(Sci.SCI_GETSELECTIONNCARET, sel.Number);
            Ref.Send(Sci.SCI_INSERTTEXT, car, sel.Text);
        }
        
        public void MakeNextSelectionMain()
        {
            Ref.Send(Sci.SCI_ROTATESELECTION);
        }
        
        public void SwapCaretForMainSelection()
        {
            Ref.Send(Sci.SCI_SWAPMAINANCHORCARET);
        }
        
        public void ReplaceSelection(string text)
        {
            Ref.Send(Sci.SCI_REPLACESEL, Sci.NIL, text);
        }

        public bool HasSelections()
        {
            if (SelectionsCount > 1)
                return true;

            return Ref.Send(Sci.SCI_GETSELECTIONEND) - Ref.Send(Sci.SCI_GETSELECTIONSTART) > 0;
        }
       
        private bool _hideSelection;
        public bool HideSelection
        {
            get { return _hideSelection; }
            set
            {
                Ref.Send(Sci.SCI_HIDESELECTION, value ? Sci.TRUE : Sci.FALSE);
                _hideSelection = value;
            }
        }
        
        public SciSelectionMode SelectionMode
        {
            get { return (SciSelectionMode)Ref.Send(Sci.SCI_GETSELECTIONMODE); }
            set { Ref.Send(Sci.SCI_SETSELECTIONMODE, (Int32)value); }
        }
        
        public bool MultipleSelection
        {
            get { return Ref.Send(Sci.SCI_GETMULTIPLESELECTION) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETMULTIPLESELECTION, value); }
        }
        
        public bool MultipleSelectionTyping
        {
            get { return Ref.Send(Sci.SCI_GETADDITIONALSELECTIONTYPING) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETADDITIONALSELECTIONTYPING, value); }
        }

        public bool MultipleSelectionPaste
        {
            get { return Ref.Send(Sci.SCI_GETMULTIPASTE) == Sci.TRUE; }
            set { Ref.Send(Sci.SCI_SETMULTIPASTE, value); }
        }

        public VirtualSpaceMode VirtualSpace
        {
            get { return (VirtualSpaceMode)Ref.Send(Sci.SCI_GETVIRTUALSPACEOPTIONS); }
            set { Ref.Send(Sci.SCI_SETVIRTUALSPACEOPTIONS, (Int32)value); }
        }
        
        public int SelectionsCount
        {
            get { return Ref.Send(Sci.SCI_GETSELECTIONS); }
        }
        
        private Color _mainSelectionForeColor;
        public Color MainSelectionForeColor
        {
            get { return _mainSelectionForeColor; }
            set
            {
                Ref.Send(Sci.SCI_SETSELFORE, UseSelectionColor, value.ToScintillaColor());
                _mainSelectionForeColor = value;
            }
        }

        private Color _mainSelectionBackColor;
        public Color MainSelectionBackColor
        {
            get { return _mainSelectionBackColor; }
            set
            {
                Ref.Send(Sci.SCI_SETSELBACK, UseSelectionColor, value.ToScintillaColor());
                _mainSelectionBackColor = value;
            }
        }

        public int SelectionAlpha
        {
            get { return Ref.Send(Sci.SCI_GETSELALPHA); }
            set { Ref.Send(Sci.SCI_SETSELALPHA, value); }
        }

        public bool UseSelectionColor { get; set; }
        #endregion
        
        #region DirtyFlag
        public event EventHandler SavePointReached;
        private void OnSavePointReached()
        {
            var h = SavePointReached;

            if (h != null)
                h(this, EventArgs.Empty);
        }
        
        public event EventHandler SavePointLeft;
        private void OnSavePointLeft()
        {
            var h = SavePointLeft;

            if (h != null)
                h(this, EventArgs.Empty);
        }
        #endregion


		#region Properties


        internal FoldingManager Folding { get; private set; }

        public KeyboardManager Keyboard { get; private set; }
		
		public StandardStyles Styles { get; private set; }

        public StandardIndicators Indicators { get; private set; }
        
		#endregion


		#region Events
        public event EventHandler<FoldNeededEventArgs> FoldNeeded;
        private void OnFoldNeeded(int firstLine, int lastLine, ScNotification scn)
        {
            var end = LineCount;

            while (GetLineIndentation(firstLine) != 0 && firstLine > 0)
                firstLine--;

            while (GetLineIndentation(lastLine) != 0 && lastLine > end)
                lastLine--;

            if (firstLine < 0 || lastLine > end)
                return;
            
            var ev = new FoldNeededEventArgs(firstLine, lastLine);
            var h = FoldNeeded;

            if (h != null)
            {
                h(this, ev);
                Folding.Fold(ev.Regions);
            }
        }
        
        public event EventHandler<DwellEventArgs> MouseDwellEnd;
        private void OnMouseDwellEnd(DwellEventArgs e)
        {
            var h = MouseDwellEnd;

            if (h != null)
                h(this, e);
        }
        
        public event EventHandler<DwellEventArgs> MouseDwell;
        private void OnMouseDwell(DwellEventArgs e)
        {
            var h = MouseDwell;

            if (h != null)
                h(this, e);
        }

        public event EventHandler Updated;
        private void OnUpdated()
        {
            var h = Updated;

            if (h != null)
                h(this, EventArgs.Empty);
        }

        public event EventHandler SmartIndentRequired;
        private void OnSmartIndentRequired()
        {
            var h = SmartIndentRequired;

            if (h != null)
                h(this, EventArgs.Empty);
        }
        
        public event EventHandler<LinkClickedEventArgs> LinkClicked;
		private void OnLinkClicked(LinkClickedEventArgs e)
		{
			var h = LinkClicked;

			if (h != null)
				h(this, e);
        }

        public event EventHandler<LinkClickedEventArgs> LinkDoubleClicked;
        private void OnLinkDoubleClicked(LinkClickedEventArgs e)
        {
            var h = LinkDoubleClicked;

            if (h != null)
                h(this, e);
        }
        

        public event KeyPressEventHandler CharAdded;
        private void OnCharAdded(ScNotification scn)
        {
            var h = CharAdded;
            
            if (h != null)
                h(this, new KeyPressEventArgs((Char)scn.Ch));
        }
        
        public event EventHandler<StyleNeededEventArgs> StyleNeeded;
		private void OnStyleNeeded(ScNotification scn)
		{
			var endStyled = Ref.Send(Sci.SCI_GETENDSTYLED);
            var sline = LookupLine(Ref.Send(Sci.SCI_LINEFROMPOSITION, endStyled));
            var eline = Ref.Send(Sci.SCI_LINEFROMPOSITION, scn.Position);

			var sp = Ref.Send(Sci.SCI_POSITIONFROMLINE, sline);
			var ep = Ref.Send(Sci.SCI_GETLINEENDPOSITION, eline);


            //System.Diagnostics.Debug.WriteLine("Styling " + (cc++));
            Style(sp, ep, false);
            OnFoldNeeded(sline, eline, scn);            
		}

        public TextStyle GetStyleAt(int position)
        {
            return (TextStyle)Ref.Send(Sci.SCI_GETSTYLEAT, position);
        }

        private void Style(int sp, int ep, bool norepeat)
        {
            var text = UseUnicodeLexing ? GetTextRange(sp, ep) : GetTextRangeUnicode(sp, ep);//GetText();
            var e = new StyleNeededEventArgs(sp, ep, 0, text);

            var h = StyleNeeded;
            
            if (h != null)
                h(this, e);

            Ref.Send(Sci.SCI_STARTSTYLING, sp, Sci.STYLE_MASK);
            Ref.Send(Sci.SCI_SETSTYLING, ep - sp, Sci.STYLE_DEFAULT);
            var tok = default(StyledToken);
            var dict = new Dictionary<Int32,Int32>();

            foreach (var i in e.Items)
            {
                var pos = i.Position + sp;

                //if (pos + i.Length < sp || pos > ep)
                //    continue;

                var style = (Int32)i.StyleKey;
                Ref.Send(Sci.SCI_STARTSTYLING, pos, Sci.STYLE_MASK);
                Ref.Send(Sci.SCI_SETSTYLING, i.Length, style);

                var regStart = Ref.Send(Sci.SCI_LINEFROMPOSITION, pos);
                var regEnd = Ref.Send(Sci.SCI_LINEFROMPOSITION, pos + i.Length);
                var st = i.StyleKey == TextStyle.MultilineStyle1 
                    || i.StyleKey == TextStyle.MultilineStyle2
                    || i.StyleKey == TextStyle.MultilineStyle3
                    || i.StyleKey == TextStyle.MultilineStyle4 ? 1 : 0;

                for (var l = regStart; l < regEnd + 1; l++)
                {
                    if (st == 1 || !dict.ContainsKey(l))
                    {
                        Ref.Send(Sci.SCI_SETLINESTATE, l, st);

                        if (st == 1 && !dict.ContainsKey(l))
                            dict.Add(l, l);
                    }
                }

                tok = i;
            }

            //if (!norepeat && (tok.StyleKey == TextStyle.VerbatimString || tok.StyleKey == TextStyle.MultilineComment))
            //{
            //    var text = GetLineFromPosition(sp);
            //    Style(sp, GetPositionFromLine(text + LinesOnScreen), true);
            //}
        }

        public bool UseUnicodeLexing { get; set; }

        public void SetLineState(int line, int state)
        {
            Ref.Send(Sci.SCI_SETLINESTATE, line, state);
        }

        public int GetLineState(int line)
        {
            return Ref.Send(Sci.SCI_GETLINESTATE, line);
        }

        public void RestyleDocument()
        {
            Style(0, GetTextLength(), false);
        }
        
        private int LookupLine(int line)
        {
            do
                line--;
            while (Ref.Send(Sci.SCI_GETLINESTATE, line) != 0);                

            return line < 0 ? 0 : line >= LineCount ? LineCount - 1 : line;
        }
		#endregion
	}
}