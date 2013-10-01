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
            get { return sref.Send(Sci.SCI_GETSELECTIONNCARET); }
            set { sref.Send(Sci.SCI_SETSELECTIONNCARET, value); }
        }

        public int Start
        {
            get { return sref.Send(Sci.SCI_GETSELECTIONSTART); }
            set { sref.Send(Sci.SCI_SETSELECTIONSTART, value); }
        }

        public int End
        {
            get { return sref.Send(Sci.SCI_GETSELECTIONEND); }
            set { sref.Send(Sci.SCI_SETSELECTIONEND, value); }
        }

        public string Text
        {
            get
            {
                var len = sref.Send(Sci.SCI_GETSELECTIONEND) - sref.Send(Sci.SCI_GETSELECTIONSTART) + 1;
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
            set 
            {
                var buffer = Encoding.UTF8.GetBytes(value);
                sref.Send(Sci.SCI_REPLACESEL, Sci.NIL, buffer); 
            }
        }
    }
}
