using System;
using System.Drawing;
using Elide.Environment.Configuration;
using Sci = Elide.Scintilla.ObjectModel;

namespace Elide.TextEditor.Configuration
{
    [Serializable]
    public class TextConfig : Config
    {
        public TextConfig()
        {
            UseTabs = null;
            IndentMode = null;
            LineEndings = null;
            TabSize = null;
            IndentSize = null;
            CaretLineAlpha = null;
            CaretBlinkPeriod = null;
            MultipleSelection = null;
            MultipleSelectionTyping = null;
            MultipleSelectionPaste = null;
            VirtualSpace = null;
            CaretLineVisible = null;
            CaretColor = null;
            CaretLineColor = null;
            CaretStyle = null;
            CaretWidth = null;
            CaretSticky = null;
            IndentationGuides = null;
            VisibleWhiteSpace = null;
            VisibleLineEndings = null;
            WordWrap = null;
            WordWrapIndicator = null;
            LongLineIndicator = null;
            SelectionBackColor = null;
            SelectionForeColor = null;
            UseSelectionColor = null;
            LongLine = null;
            SelectionTransparency = null;
        }

        public static TextConfig CreateDefault()
        {
            return new TextConfig
                {
                    UseTabs = false,
                    IndentMode = Sci.IndentMode.None,
                    TabSize = 2,
                    IndentSize = 2,
                    LineEndings = Sci.LineEndings.Windows,
                    MultipleSelection = true,
                    MultipleSelectionTyping = true,
                    MultipleSelectionPaste = true,
                    VirtualSpace = false,
                    CaretLineVisible = false,
                    CaretColor = KnownColor.Black,
                    CaretLineColor = KnownColor.Silver,
                    CaretLineAlpha = 50,
                    CaretStyle = Sci.CaretStyle.Line,
                    CaretWidth = Sci.CaretWidth.Thin,
                    CaretBlinkPeriod = 500,
                    CaretSticky = false,
                    IndentationGuides = false,
                    VisibleWhiteSpace = true,
                    VisibleLineEndings = false,
                    WordWrap = Sci.WordWrapMode.None,
                    WordWrapIndicator = false,
                    LongLineIndicator = false,
                    LongLine = 80,
                    SelectionBackColor = KnownColor.Gray,
                    SelectionForeColor = KnownColor.Black,
                    UseSelectionColor = false,
                    SelectionTransparency = 100
                };
        }

        public bool? UseTabs { get; set; }

        public Sci.IndentMode? IndentMode { get; set; }

        public int? TabSize { get; set; }

        public int? IndentSize { get; set; }

        public Sci.LineEndings? LineEndings { get; set; }

        public bool? MultipleSelection { get; set; }

        public bool? MultipleSelectionTyping { get; set; }

        public bool? MultipleSelectionPaste { get; set; }

        public bool? VirtualSpace { get; set; }

        public bool? CaretLineVisible { get; set; }

        public KnownColor? CaretLineColor { get; set; }

        public KnownColor? CaretColor { get; set; }

        public Sci.CaretStyle? CaretStyle { get; set; }

        public Sci.CaretWidth? CaretWidth { get; set; }

        public int? CaretLineAlpha { get; set; }

        public int? CaretBlinkPeriod { get; set; }

        public bool? CaretSticky { get; set; }

        public bool? IndentationGuides { get; set; }

        public bool? VisibleWhiteSpace { get; set; }

        public bool? VisibleLineEndings { get; set; }

        public Sci.WordWrapMode? WordWrap { get; set; }

        public bool? WordWrapIndicator { get; set; }

        public bool? LongLineIndicator { get; set; }

        public int? LongLine { get; set; }

        public KnownColor? SelectionBackColor { get; set; }

        public KnownColor? SelectionForeColor { get; set; }

        public bool? UseSelectionColor { get; set; }

        public int? SelectionTransparency { get; set; }
    }
}
