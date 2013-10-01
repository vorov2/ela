using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Printing;

namespace Elide.Scintilla.Printing
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ScintillaPageSettings : System.Drawing.Printing.PageSettings
    {
        public static readonly PageInformation DefaultFooter = new PageInformation(PageInformationBorder.Top, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing);
        public static readonly PageInformation DefaultHeader = new PageInformation(PageInformationBorder.Bottom, InformationType.DocumentName, InformationType.Nothing, InformationType.PageNumber);
        private bool baseColor;
         
        public ScintillaPageSettings()
        {
            baseColor = base.Color;
            
            _header = new HeaderInformation(PageInformationBorder.Bottom, InformationType.DocumentName, InformationType.Nothing, InformationType.PageNumber);
            _footer = new FooterInformation(PageInformationBorder.Top, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing);
            _fontMagnification = 0;
            _colorMode = PrintColorMode.Normal;

            // Set default margins to 1/2 inch (50/100ths)
            base.Margins.Top = 50;
            base.Margins.Left = 50;
            base.Margins.Right = 50;
            base.Margins.Bottom = 50;
        }
        
        private void ResetColor()
        {
            Color = baseColor;
        }
        
        private void ResetColorMode()
        {
            _colorMode = PrintColorMode.Normal;
        }
        
        private void ResetFontMagnification()
        {
            _fontMagnification = 0;
        }

        private void ResetLandscape()
        {
            Landscape = false;
        }
        
        private void ResetMargins()
        {
            Margins = new Margins(50,50,50,50);
        }
        
        internal bool ShouldSerialize()
        {
            return ShouldSerializeColor() ||
                ShouldSerializeColorMode() ||
                ShouldSerializeFontMagnification() ||
                ShouldSerializeFooter() ||
                ShouldSerializeHeader() ||
                ShouldSerializeLandscape() ||
                ShouldSerializeMargins();
        }

        private bool ShouldSerializeColor()
        {
            return Color != baseColor;
        }
        
        private bool ShouldSerializeColorMode()
        {
            return _colorMode != PrintColorMode.Normal;
        }

        private bool ShouldSerializeFontMagnification()
        {
            return _fontMagnification != 0;
        }

        private bool ShouldSerializeFooter()
        {
            return _footer.ShouldSerialize();
        }
        
        private bool ShouldSerializeHeader()
        {
            return _header.ShouldSerialize();
        }

        private bool ShouldSerializeLandscape()
        {
            return Landscape;
        }

        private bool ShouldSerializeMargins()
        {
            return Margins.Bottom != 50 ||Margins.Left != 50 || Margins.Right != 50 || Margins.Bottom != 50;
        }

        private PrintColorMode _colorMode;
        public PrintColorMode ColorMode
        {
            get { return _colorMode; }
            set { _colorMode = value; }
        }

        private short _fontMagnification;
        public short FontMagnification
        {
            get { return _fontMagnification; }
            set { _fontMagnification = value; }
        }

        private FooterInformation _footer;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public FooterInformation Footer
        {
            get { return _footer; }
            set { _footer = value; }
        }

        private HeaderInformation _header;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public HeaderInformation Header
        {
            get { return _header; }
            set { _header = value; }
        }
    }
}