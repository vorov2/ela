using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla
{
	public sealed class Style
	{
        private EditorRef @ref;

		internal Style(EditorRef @ref, TextStyle key)
		{
			this.@ref = @ref;
			Key = key;
		}
		
		public TextStyle Key { get; private set; }
        
		public string Font
		{
			get { return @ref.SendStr(Sci.SCI_STYLEGETFONT, (Int32)Key); }
			set { @ref.Send(Sci.SCI_STYLESETFONT, (Int32)Key, value); }
		}
        
		public int FontSize
		{
			get { return @ref.Send(Sci.SCI_STYLEGETSIZE, (Int32)Key); }
			set { @ref.Send(Sci.SCI_STYLESETSIZE, (Int32)Key, value); }
		}
        
		public Color ForeColor
		{
			get { return SciColor.FromScintillaColor(@ref.Send(Sci.SCI_STYLEGETFORE)); }
			set { @ref.Send(Sci.SCI_STYLESETFORE, (Int32)Key, value.ToScintillaColor()); }
		}
        
		public Color BackColor
		{
			get { return SciColor.FromScintillaColor(@ref.Send(Sci.SCI_STYLEGETBACK)); }
			set { @ref.Send(Sci.SCI_STYLESETBACK, (Int32)Key, value.ToScintillaColor()); }
		}
        
		public bool Bold
		{
			get { return @ref.Send(Sci.SCI_STYLEGETBOLD, (Int32)Key) > 0; }
			set { @ref.Send(Sci.SCI_STYLESETBOLD, (Int32)Key, value); }
		}
        
		public bool Italic
		{
			get { return @ref.Send(Sci.SCI_STYLEGETITALIC, (Int32)Key) > 0; }
			set {  @ref.Send(Sci.SCI_STYLESETITALIC, (Int32)Key, value); }
		}
        
		public bool Underline
		{
			get { return @ref.Send(Sci.SCI_STYLEGETUNDERLINE, (Int32)Key) > 0; }
			set { @ref.Send(Sci.SCI_STYLESETUNDERLINE, (Int32)Key, value); }
		}

        public bool Hotspot
        {
            get { return @ref.Send(Sci.SCI_STYLEGETHOTSPOT, (Int32)Key) > 0; }
            set { @ref.Send(Sci.SCI_STYLESETHOTSPOT, (Int32)Key, value); }
        }

        public bool Visible
        {
            get { return @ref.Send(Sci.SCI_STYLEGETVISIBLE, (Int32)Key) > 0; }
            set { @ref.Send(Sci.SCI_STYLESETVISIBLE, (Int32)Key, value); }
        }
	}
}
