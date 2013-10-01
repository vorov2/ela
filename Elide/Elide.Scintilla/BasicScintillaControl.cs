using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Elide.Scintilla.Internal;
using System.IO;
using System.Runtime.InteropServices;
using Elide.Scintilla.ObjectModel;

namespace Elide.Scintilla
{
    public class BasicScintillaControl : Control
    {
        private const string CLS_NAME = "Scintilla";
        private const string LIB_NAME32 = "SciLexer.dll";
        private const string LIB_NAME64 = "SciLexer64.dll";

        private static IntPtr libPtr;

        public BasicScintillaControl()
        {
            SetStyle(ControlStyles.UserPaint, false);
            Ref = new EditorRef(Handle);
            InternalInitialize();
            //Ref.Send(Sci.SCI_SETYCARETPOLICY, Sci.CARET_SLOP | Sci.CARET_STRICT | Sci.CARET_EVEN, 50);
        }

        protected virtual void InternalInitialize()
        {

        }
        
        private static void LoadLibrary()
        {
            if (libPtr == IntPtr.Zero)
            {
                var path = Path.Combine(Application.StartupPath, IntPtr.Size == 4 ? LIB_NAME32 : LIB_NAME64);
                libPtr = NativeMethods.LoadLibrary(path);
            }
        }

        public void EmptyUndoBuffer()
        {
            Ref.Send(Sci.SCI_EMPTYUNDOBUFFER);
        }

        public bool IsDirty()
        {
            return Ref.Send(Sci.SCI_GETMODIFY) == Sci.TRUE;
        }

        public void ClearDirtyFlag()
        {
            Ref.Send(Sci.SCI_SETSAVEPOINT);
        }
        
        public void SetText(string value)
        {
            Ref.Send(Sci.SCI_SETCODEPAGE, Sci.SC_CP_UTF8);
            var os = Ref.Send(Sci.SCI_GETREADONLY);
            Ref.Send(Sci.SCI_SETREADONLY);
            var buffer = Encoding.UTF8.GetBytes(value);
            Ref.Send(Sci.SCI_SETTEXT, Sci.NIL, buffer);//value);
            Ref.Send(Sci.SCI_SETREADONLY, os);
        }

        public string GetText()
        {
            var len = Ref.Send(Sci.SCI_GETTEXTLENGTH) + 1;
            var ptr = Marshal.AllocCoTaskMem(len + 1);

            try
            {
                if (Ref.Send(Sci.SCI_GETTEXT, len, (Int32)ptr) != 0)
                    return Marshal.PtrToStringAnsi(ptr);
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptr);
            }

            return String.Empty;
        }

        public string GetTextUnicode()
        {
            var len = Ref.Send(Sci.SCI_GETTEXTLENGTH) + 1;
            var ptr = Marshal.AllocCoTaskMem(len + 1);

            try
            {
                if (Ref.Send(Sci.SCI_GETTEXT, len, (Int32)ptr) != Sci.FALSE)
                {
                    var buffer = new byte[len];

                    for (var i = 0; i < len; i++)
                        buffer[i] = Marshal.ReadByte(ptr, i);

                    return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                }
            }
            finally
            {
                if (ptr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(ptr);
            }

            return String.Empty;
        }

        public int GetTextLength()
        {
            return Ref.Send(Sci.SCI_GETTEXTLENGTH);
        }

        public string GetTextRangeUnicode(int startPos, int endPos)
        {
            if (startPos > endPos)
                return String.Empty;

            if (endPos > Ref.Send(Sci.SCI_GETTEXTLENGTH) - 1)
                endPos = Ref.Send(Sci.SCI_GETTEXTLENGTH);

            var rng = new InternalTextRange();
            rng.Range.Min = startPos;
            rng.Range.Max = endPos;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rng));

            try
            {
                var ptr2 = Marshal.AllocHGlobal(endPos - startPos + 1);
                rng.Text = ptr2;

                try
                {
                    Marshal.StructureToPtr(rng, ptr, false);
                    var len = Ref.Send(Sci.SCI_GETTEXTRANGE, Sci.NIL, (Int32)ptr);
                    var ret = (InternalTextRange)Marshal.PtrToStructure(ptr, typeof(InternalTextRange));
                    
                    var buffer = new byte[len];

                    for (var i = 0; i < len; i++)
                        buffer[i] = Marshal.ReadByte(ret.Text, i);

                    return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr2);
                }
            }
            finally
            {
                Marshal.DestroyStructure(ptr, typeof(InternalTextRange));
            }
        }

        public string GetTextRange(int startPos, int endPos)
        {
            if (startPos > endPos)
                return String.Empty;

            var rng = new InternalTextRange();
            rng.Range.Min = startPos;
            rng.Range.Max = endPos;
            var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(rng));

            try
            {
                var ptr2 = Marshal.AllocHGlobal(endPos - startPos + 1);
                rng.Text = ptr2;

                try
                {
                    Marshal.StructureToPtr(rng, ptr, false);
                    Ref.Send(Sci.SCI_GETTEXTRANGE, Sci.NIL, (Int32)ptr);
                    var ret = (InternalTextRange)Marshal.PtrToStructure(ptr, typeof(InternalTextRange));
                    return Marshal.PtrToStringAnsi(ret.Text);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr2);
                }
            }
            finally
            {
                Marshal.DestroyStructure(ptr, typeof(InternalTextRange));
            }
        }

        public int GetColumnFromPosition(int position)
        {
            return Ref.Send(Sci.SCI_GETCOLUMN, position);
        }

        public int GetLineFromPosition(int position)
        {
            return Ref.Send(Sci.SCI_LINEFROMPOSITION, position);
        }

        public Line GetLine(int number)
        {
            return new Line(number, Ref);
        }
        
        public SciDocument CreateDocument()
        {
            var ptr = (IntPtr)Ref.Send(Sci.SCI_CREATEDOCUMENT);
            return new SciDocument(Ref, ptr);
        }

        public SciDocument GetCurrentDocument()
        {
            var ptr = (IntPtr)Ref.Send(Sci.SCI_GETDOCPOINTER);
            return new SciDocument(Ref, ptr);
        }

        public void AttachDocument(SciDocument doc)
        {
            var ptr = Ref.Send(Sci.SCI_GETDOCPOINTER);

            if (ptr == doc.Pointer.ToInt32())
                return;

            ClearDocumentState();

            if (ptr != Sci.NIL)
                Ref.Send(Sci.SCI_ADDREFDOCUMENT, Sci.NIL, ptr);

            Ref.Send(Sci.SCI_SETDOCPOINTER, Sci.NIL, doc.Pointer.ToInt32());
            Ref.Send(Sci.SCI_SETCODEPAGE, Sci.SC_CP_UTF8);
            OnDocumentAttached();
        }

        protected int DocumentStateFlags { get; set; }

        protected virtual void ClearDocumentState()
        {
            DocumentStateFlags = 0;            
        }

        protected override CreateParams CreateParams
        {
            get
            {
                if (!DesignMode)
                    LoadLibrary();
                
                var p = base.CreateParams;
                p.ClassName = libPtr == IntPtr.Zero ? "EDIT" : CLS_NAME;
                return p;
            }
        }

        public event EventHandler DocumentAttached;
        private void OnDocumentAttached()
        {
            var h = DocumentAttached;

            if (h != null)
                h(this, EventArgs.Empty);
        }
        
        internal EditorRef Ref { get; private set; }
    }
}
