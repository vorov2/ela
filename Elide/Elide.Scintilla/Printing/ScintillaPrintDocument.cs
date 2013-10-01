using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;

namespace Elide.Scintilla.Printing
{
    [TypeConverterAttribute(typeof(System.ComponentModel.ExpandableObjectConverter))]
    public class ScintillaPrintDocument : System.Drawing.Printing.PrintDocument
    {
        private int _iCurrentPage;
        private int _iPosition;
        private int _iPrintEnd;
        private ScintillaControl sci;
        
        public ScintillaPrintDocument(ScintillaControl sci, string documentTitle)
        {
            this.sci = sci;
            DocumentName = documentTitle;
            DefaultPageSettings = new ScintillaPageSettings();
        }

        private void DrawCurrentPage(Graphics gr, Rectangle bounds)
        {
            Point[] oPoints = {
                new Point(bounds.Left, bounds.Top),
                new Point(bounds.Right, bounds.Bottom)
                };
            gr.TransformPoints(CoordinateSpace.Device, CoordinateSpace.Page, oPoints);

            PrintRectangle oPrintRectangle = new PrintRectangle(oPoints[0].X, oPoints[0].Y, oPoints[1].X, oPoints[1].Y);

            RangeToFormat oRangeToFormat = new RangeToFormat();
            oRangeToFormat.hdc = oRangeToFormat.hdcTarget = gr.GetHdc();
            oRangeToFormat.rc = oRangeToFormat.rcPage = oPrintRectangle;
            oRangeToFormat.chrg.Min = _iPosition;
            oRangeToFormat.chrg.Max = _iPrintEnd;
            _iPosition = sci.FormatRange(true, ref oRangeToFormat);
            
        }

        private Rectangle DrawFooter(Graphics oGraphics, Rectangle oBounds, PageInformation oFooter)
        {
            if (oFooter.Display)
            {
                int iHeight = oFooter.Height;
                Rectangle oFooterBounds = new Rectangle(oBounds.Left, oBounds.Bottom - iHeight, oBounds.Width, iHeight);

                oFooter.Draw(oGraphics, oFooterBounds, this.DocumentName, _iCurrentPage);

                return new Rectangle(
                    oBounds.Left, oBounds.Top,
                    oBounds.Width, oBounds.Height - oFooterBounds.Height - oFooter.Margin
                    );
            }
            else
            {
                return oBounds;
            }
        }

        private Rectangle DrawHeader(Graphics oGraphics, Rectangle oBounds, PageInformation oHeader)
        {
            if (oHeader.Display)
            {
                Rectangle oHeaderBounds = new Rectangle(oBounds.Left, oBounds.Top, oBounds.Width, oHeader.Height);

                oHeader.Draw(oGraphics, oHeaderBounds, this.DocumentName, _iCurrentPage);

                return new Rectangle(
                    oBounds.Left, oBounds.Top + oHeaderBounds.Height + oHeader.Margin,
                    oBounds.Width, oBounds.Height - oHeaderBounds.Height - oHeader.Margin
                    );
            }
            else
            {
                return oBounds;
            }
        }

        protected override void OnBeginPrint(PrintEventArgs e)
        {
            base.OnBeginPrint(e);

            _iPosition = 0;
            _iPrintEnd = sci.GetTextLength();
            _iCurrentPage = 1;
        }

        protected override void OnPrintPage(PrintPageEventArgs e)
        {
            base.OnPrintPage(e);

            ScintillaPageSettings oPageSettings = null;
            HeaderInformation oHeader = ((ScintillaPageSettings)DefaultPageSettings).Header;
            FooterInformation oFooter = ((ScintillaPageSettings)DefaultPageSettings).Footer;
            Rectangle oPrintBounds = e.MarginBounds;
            bool bIsPreview = this.PrintController.IsPreview;

            // When not in preview mode, adjust graphics to account for hard margin of the printer
            if (!bIsPreview)
            {
                e.Graphics.TranslateTransform(-e.PageSettings.HardMarginX, -e.PageSettings.HardMarginY);
            }

            // Get the header and footer provided if using Scintilla.Printing.PageSettings
            if (e.PageSettings is ScintillaPageSettings)
            {
                oPageSettings = (ScintillaPageSettings)e.PageSettings;

                oHeader = oPageSettings.Header;
                oFooter = oPageSettings.Footer;

                sci.SetPrintMagnification(oPageSettings.FontMagnification);
                sci.SetPrintColourMode((int)oPageSettings.ColorMode);
            }

            // Draw the header and footer and get remainder of page bounds
            oPrintBounds = DrawHeader(e.Graphics, oPrintBounds, oHeader);
            oPrintBounds = DrawFooter(e.Graphics, oPrintBounds, oFooter);

            // When not in preview mode, adjust page bounds to account for hard margin of the printer
            if (!bIsPreview)
            {
                oPrintBounds.Offset((int)-e.PageSettings.HardMarginX, (int)-e.PageSettings.HardMarginY);
            }
            DrawCurrentPage(e.Graphics, oPrintBounds);

            // Increment the page count and determine if there are more pages to be printed
            _iCurrentPage++;
            e.HasMorePages = (_iPosition < _iPrintEnd);
        }
        
        private void ResetDocumentName()
        {
            DocumentName = "document";
        }
        
        private void ResetOriginAtMargins()
        {
            OriginAtMargins = false;
        }

        public bool ShouldSerialize()
        {
            return base.DocumentName != "document" || OriginAtMargins;
        }

        private bool ShouldSerializeDocumentName()
        {
            return DocumentName != "document";
        }

        private bool ShouldSerializeOriginAtMargins()
        {
            return OriginAtMargins;
        }
    }
}
