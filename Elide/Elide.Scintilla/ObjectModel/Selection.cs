using System;
using System.Runtime.InteropServices;
using Elide.Scintilla.Internal;
using System.Text;

namespace Elide.Scintilla.ObjectModel
{
    public sealed class Selection
    {
        private EditorRef sref;

        internal Selection(int number, EditorRef sref)
        {
            this.sref = sref;
            Number = number;
        }
       
        public int Number { get; internal set; }

        public bool MainSelection
        {
            get { return sref.Send(Sci.SCI_GETMAINSELECTION) == Number; }
            set
            {
                if (value)
                    sref.Send(Sci.SCI_SETMAINSELECTION, Number);
                else
                    sref.Send(Sci.SCI_ROTATESELECTION);
            }
        }

        public int CaretPosition
        {
            get { return sref.Send(Sci.SCI_GETSELECTIONNCARET, Number); }
        }

        public int Start
        {
            get { return sref.Send(Sci.SCI_GETSELECTIONNSTART, Number); }
        }

        public int End
        {
            get { return sref.Send(Sci.SCI_GETSELECTIONNEND, Number); }
        }

        public string Text
        {
            get
            {
                var len = sref.Send(Sci.SCI_GETSELECTIONNEND, Number) - sref.Send(Sci.SCI_GETSELECTIONNSTART, Number) + 1;
                var ptr = Marshal.AllocHGlobal(len);

                try
                {
                    sref.Send(Sci.SCI_GETSELTEXT, Sci.NIL, (Int32)ptr);
                    var buffer = new byte[len];

                    for (var i = 0; i < len; i++)
                        buffer[i] = Marshal.ReadByte(ptr, i);

                    return Encoding.UTF8.GetString(buffer).TrimEnd('\0');
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
            }
        }
    }
}
