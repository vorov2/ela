using System;

namespace Elide.Scintilla.Internal
{
	internal class DocumentRef
	{
		internal DocumentRef(IntPtr ptr)
		{
			Pointer = ptr;
		}
        		
        public static implicit operator DocumentRef(IntPtr ptr)
		{
			return new DocumentRef(ptr);
		}
        
		public static implicit operator DocumentRef(int ptr)
		{
			return new DocumentRef((IntPtr)ptr);
		}
        
		public static implicit operator IntPtr(DocumentRef @ref)
		{
			return @ref.Pointer;
		}
        
		public static implicit operator Int32(DocumentRef @ref)
		{
			return @ref.Pointer.ToInt32();
		}
		
        internal IntPtr Pointer { get; private set; }
	}
}
