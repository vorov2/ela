using System;
using System.ComponentModel;
using System.Drawing;

namespace Elide.Scintilla.Printing
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class HeaderInformation : PageInformation
    {
        public HeaderInformation() : base(PageInformationBorder.None, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing)
        {

        }

        public HeaderInformation(int iMargin, Font oFont, PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight)
            : base(iMargin, oFont, eBorder, eLeft, eCenter, eRight)
        {

        }

        public HeaderInformation(PageInformationBorder eBorder, InformationType eLeft, InformationType eCenter, InformationType eRight)
            : base(3, DefaultFont, eBorder, eLeft, eCenter, eRight)
        {

        }

        private void ResetBorder()
        {
            Border = PageInformationBorder.Bottom;
        }
        
        private void ResetCenter()
        {
            Center = InformationType.Nothing;
        }
        
        private void ResetFont()
        {
            Font = DefaultFont;
        }
        
        private void ResetLeft()
        {
            Left = InformationType.DocumentName;
        }
        
        private void ResetMargin()
        {
            Margin = 3;
        }
        
        private void ResetRight()
        {
            Right = InformationType.PageNumber;
        }
        
        internal bool ShouldSerialize()
        {
            return ShouldSerializeBorder() ||
                ShouldSerializeCenter() ||
                ShouldSerializeFont() ||
                ShouldSerializeLeft() ||
                ShouldSerializeMargin() ||
                ShouldSerializeRight();
        }
        
        private bool ShouldSerializeBorder()
        {
            return Border != PageInformationBorder.Bottom;
        }
        
        private bool ShouldSerializeCenter()
        {
            return Center != InformationType.Nothing;
        }
        
        private bool ShouldSerializeFont()
        {
            return !DefaultFont.Equals(Font);
        }
        
        private bool ShouldSerializeLeft()
        {
            return Left != InformationType.DocumentName;
        }
        
        private bool ShouldSerializeMargin()
        {
            return Margin != 3;
        }
        
        private bool ShouldSerializeRight()
        {
            return Right != InformationType.PageNumber;
        }

        public override PageInformationBorder Border
        {
            get { return base.Border; }
            set { base.Border = value; }
        }

        public override InformationType Center
        {
            get { return base.Center; }
            set { base.Center = value; }
        }

        public override Font Font
        {
            get { return base.Font; }
            set { base.Font = value; }
        }

        public override InformationType Left
        {
            get { return base.Left; }
            set { base.Left = value; }
        }

        public override int Margin
        {
            get { return base.Margin; }
            set { base.Margin = value; }
        }

        public override InformationType Right
        {
            get { return base.Right; }
            set { base.Right = value; }
        }
    }
}
