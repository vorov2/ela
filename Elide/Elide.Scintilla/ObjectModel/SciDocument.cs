using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla.ObjectModel
{
    public sealed class SciDocument : IDisposable
    {
        private EditorRef @ref;

        internal SciDocument(EditorRef @ref, IntPtr ptr)
        {
            Pointer = ptr;
            this.@ref = @ref;
        }
        
        public void Dispose()
        {
            if (Pointer != IntPtr.Zero)
            {
                while (@ref.Send(Sci.SCI_GETDOCPOINTER) == Pointer.ToInt32())
                    @ref.Send(Sci.SCI_SETDOCPOINTER, Sci.NIL, Sci.NIL);

                @ref.Send(Sci.SCI_RELEASEDOCUMENT, Sci.NIL, Pointer.ToInt32());
                Pointer = IntPtr.Zero;
            }
        }
        
        public static bool Equals(SciDocument left, SciDocument right)
        {
            return Object.ReferenceEquals(left, right) ||
                !Object.ReferenceEquals(left, null) && !Object.ReferenceEquals(right, null) &&
                left.Pointer == right.Pointer;
        }
        
        public override bool Equals(object obj)
        {
            return obj is SciDocument && Equals(this, (SciDocument)obj);
        }
        
        public override int GetHashCode()
        {
            return Pointer.GetHashCode();
        }
        
        public static bool operator ==(SciDocument left, SciDocument right)
        {
            return Equals(left, right);
        }
        
        public static bool operator !=(SciDocument left, SciDocument right)
        {
            return !Equals(left, right);
        }
        
        public static implicit operator IntPtr(SciDocument @ref)
        {
            return @ref.Pointer;
        }
        
        public static implicit operator Int32(SciDocument @ref)
        {
            return @ref.Pointer.ToInt32();
        }
        
        internal IntPtr Pointer { get; private set; }
    }
}
