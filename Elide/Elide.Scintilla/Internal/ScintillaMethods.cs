using System;
using System.Runtime.InteropServices;
using System.Text;

namespace Elide.Scintilla.Internal
{
	internal static class ScintillaMethods
	{
        internal static int Send(this EditorRef @this, int cmd, int lhs, byte[] rhs)
        {
            return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, (IntPtr)lhs, rhs);
        }

        internal static int Send(this EditorRef @this, int cmd, IntPtr lhs, byte[] rhs)
        {
            return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, lhs, rhs);
        }

		internal static int Send(this EditorRef @this, int cmd, int lhs, string rhs)
		{
			return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, new IntPtr(lhs), rhs);
		}

		internal static int Send(this EditorRef @this, int cmd, int lhs, int rhs)
		{
			return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, new IntPtr(lhs), new IntPtr(rhs));
        }

        internal static int Send(this EditorRef @this, int cmd, int lhs, IntPtr rhs)
        {
            return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, new IntPtr(lhs), rhs);
        }

		internal static int Send(this EditorRef @this, int cmd, int lhs, uint rhs)
		{
			return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, lhs, rhs);
		}

		internal static int Send(this EditorRef @this, int cmd, int param)
		{
			return Send(@this, cmd, param, Sci.NIL);
		}

		internal static int Send(this EditorRef @this, int cmd)
		{
			return Send(@this, cmd, Sci.NIL);
		}

		internal static int Send(this EditorRef @this, int cmd, int lhs, bool rhs)
		{
			return Send(@this, cmd, lhs, rhs ? Sci.TRUE : Sci.FALSE);
		}

		internal static int Send(this EditorRef @this, int cmd, bool lhs, int rhs)
		{
			return Send(@this, cmd, lhs ? Sci.TRUE : Sci.FALSE, rhs);
		}

		internal static int Send(this EditorRef @this, int cmd, bool param)
		{
			return Send(@this, cmd, param ? Sci.TRUE : Sci.FALSE);
		}

		internal static int Send(this EditorRef @this, int cmd, string lhs, string rhs)
		{
			var ptr = Marshal.StringToCoTaskMemAnsi(lhs);
			var ret = Sci.NIL;

			try
			{
				ret = NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, ptr, rhs);
			}
			finally
			{
				if (ptr != IntPtr.Zero)
					Marshal.FreeCoTaskMem(ptr);
			}

			return ret;
		}

		internal unsafe static string SendStr(this EditorRef @ref, int cmd, int param)
		{
			var length = Send(@ref, cmd, 0, 0);			
			var buffer = new byte[length + 1];

			fixed (byte* bp = buffer)
			{
				NativeMethods.SendMessage(@ref.Handle, (uint)cmd, (IntPtr)cmd, (IntPtr)bp);

				if (bp[length - 1] == 0)
					length--;
			}

			return Encoding.UTF8.GetString(buffer, 0, length + 1);
		}

		internal static int Send(this EditorRef @this, int cmd, IntPtr lhs, IntPtr rhs)
		{
			return NativeMethods.SendMessage(@this.Handle, (UInt32)cmd, lhs, rhs);
		}
	}
}
