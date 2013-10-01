using System;
using Elide.Scintilla.Internal;

namespace Elide.Scintilla
{
	public sealed class StandardStyles
	{
		internal StandardStyles(EditorRef @ref)
		{
			Default = new Style(@ref, TextStyle.Default);
            CallTip = new Style(@ref, TextStyle.CallTip);
            BraceMatch = new Style(@ref, TextStyle.BraceMatch);
            BraceBad = new Style(@ref, TextStyle.BraceBad);
            Hyperlink = new Style(@ref, TextStyle.Hyperlink);
            Invisible = new Style(@ref, TextStyle.Invisible);
            
            Style1 = new Style(@ref, TextStyle.Style1);
            Style2 = new Style(@ref, TextStyle.Style2);
            Style3 = new Style(@ref, TextStyle.Style3);
            Style4 = new Style(@ref, TextStyle.Style4);
            Style5 = new Style(@ref, TextStyle.Style5);
            Style6 = new Style(@ref, TextStyle.Style6);
            Style7 = new Style(@ref, TextStyle.Style7);
            Style8 = new Style(@ref, TextStyle.Style8);
            Style9 = new Style(@ref, TextStyle.Style9);
            Style10 = new Style(@ref, TextStyle.Style10);
            Style11 = new Style(@ref, TextStyle.Style11);
            Style12 = new Style(@ref, TextStyle.Style12);

            MultilineStyle1 = new Style(@ref, TextStyle.MultilineStyle1);
            MultilineStyle2 = new Style(@ref, TextStyle.MultilineStyle2);
            MultilineStyle3 = new Style(@ref, TextStyle.MultilineStyle3);
            MultilineStyle4 = new Style(@ref, TextStyle.MultilineStyle4);

            Annotation1 = new Style(@ref, TextStyle.Annotation1);
            Annotation2 = new Style(@ref, TextStyle.Annotation2);
            Annotation3 = new Style(@ref, TextStyle.Annotation3);
            
            Hyperlink.Hotspot = true;
            Invisible.Visible = false;
		}
		
		public Style Default { get; private set; }

        public Style CallTip { get; private set; }

        public Style BraceMatch { get; private set; }

        public Style BraceBad { get; private set; }

        public Style Invisible { get; private set; }

        public Style Hyperlink { get; private set; }
        
        
        public Style Style1 { get; private set; }

        public Style Style2 { get; private set; }

        public Style Style3 { get; private set; }

        public Style Style4 { get; private set; }

        public Style Style5 { get; private set; }

        public Style Style6 { get; private set; }

        public Style Style7 { get; private set; }

        public Style Style8 { get; private set; }

        public Style Style9 { get; private set; }

        public Style Style10 { get; private set; }

        public Style Style11 { get; private set; }

        public Style Style12 { get; private set; }

        
        public Style MultilineStyle1 { get; private set; }

        public Style MultilineStyle2 { get; private set; }

        public Style MultilineStyle3 { get; private set; }

        public Style MultilineStyle4 { get; private set; }


        public Style Annotation1 { get; private set; }

        public Style Annotation2 { get; private set; }

        public Style Annotation3 { get; private set; }
	}
}
