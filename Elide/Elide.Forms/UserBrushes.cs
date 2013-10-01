using System;
using System.Drawing;

namespace Elide.Forms
{
    public static class UserBrushes
	{
        public static readonly Brush Text = SystemBrushes.ControlText;//Brushes.Black;

        public static readonly Brush HighlightText = SystemBrushes.HighlightText;// Brushes.White;

        public static readonly Brush Disabled = SystemBrushes.GrayText;// Brushes.White;
        
        public static readonly Brush Hover = SystemBrushes.Highlight;//Brushes.LightGray;

        public static readonly Brush Selection = SystemBrushes.Highlight;//Brushes.DarkGray;

        public static readonly Brush Border = SystemBrushes.ActiveBorder;//Brushes.DarkGray;

        public static readonly Brush LightBorder = SystemBrushes.InactiveBorder;//Brushes.DarkGray;

		public static readonly Brush Window = SystemBrushes.Window;//Brushes.White;

        public static readonly Brush Menu = SystemBrushes.Menu;//Brushes.LightGray;

        public static readonly Brush Background = SystemBrushes.Desktop;

        public static readonly Brush Button = Brushes.WhiteSmoke;
	}
}
