using System;
using System.Runtime.InteropServices;
using Elide.Scintilla.Internal;
using System.Text;

namespace Elide.Scintilla.ObjectModel
{
	public sealed class Line
	{
		private EditorRef sref;

		internal Line(int number, EditorRef sref)
		{
			this.sref = sref;
			Number = number;
		}

		public int Number { get; private set; }

		public int? SelectionStart
		{
			get
			{
				var ret = sref.Send(Sci.SCI_GETLINESELSTARTPOSITION, Number);
				return ret == Sci.INVALID_POSITION ? null : (int?)ret;
			}
		}

		public int? SelectionEnd
		{
			get
			{
				var ret = sref.Send(Sci.SCI_GETLINESELENDPOSITION, Number);
				return ret == Sci.INVALID_POSITION ? null : (int?)ret;
			}
		}

		public string Text
		{
			get
			{
				var len = Length + 1;
				var ptr = Marshal.AllocHGlobal(len);
				Marshal.Copy(new byte[len], 0, ptr, len);

				try
				{
					var plen = sref.Send(Sci.SCI_GETLINE, Number, (Int32)ptr);
                    var buffer = new byte[len];

                    for (var i = 0; i < plen; i++)
                        buffer[i] = Marshal.ReadByte(ptr, i);

                    return Encoding.UTF8.GetString(buffer);
				}
				finally
				{
					Marshal.FreeHGlobal(ptr);
				}
			}
		}

		public int Length
		{
			get { return sref.Send(Sci.SCI_LINELENGTH, Number); }
		}

		public int StartPosition
		{
			get { return sref.Send(Sci.SCI_POSITIONFROMLINE, Number); }
		}

		public int EndPosition
		{
			get { return sref.Send(Sci.SCI_GETLINEENDPOSITION, Number); }
		}

		public int TextHeight
		{
			get { return sref.Send(Sci.SCI_TEXTHEIGHT, Number); }
		}
	}
}
