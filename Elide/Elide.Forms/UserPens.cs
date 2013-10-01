using System;
using System.Drawing;

namespace Elide.Forms
{
    public static class UserPens
	{
        public static readonly Pen Text = SystemPens.ControlText;//Pen.Black;

        public static readonly Pen HighlightText = SystemPens.HighlightText;//Pen.White;

        public static readonly Pen Disabled = SystemPens.GrayText;//Pen.Gray;

        public static readonly Pen Hover = SystemPens.Highlight;

        public static readonly Pen Selection = SystemPens.Highlight;

        public static readonly Pen Border = SystemPens.ActiveBorder;//Pen.DarkGray;

        public static readonly Pen LightBorder = SystemPens.InactiveBorder;//Pen.DarkGray;

        public static readonly Pen Window = SystemPens.Window;//Pen.White;

        public static readonly Pen Menu = SystemPens.Menu;//Pen.LightGray;

        public static readonly Pen Background = SystemPens.AppWorkspace;

        public static readonly Pen Button = Pens.WhiteSmoke;
	}
}
