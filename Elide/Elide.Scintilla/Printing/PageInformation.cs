using System;
using System.ComponentModel;
using System.Drawing;

namespace Elide.Scintilla.Printing
{
    public class PageInformation
    {
        public static readonly Font DefaultFont = new Font(FontFamily.GenericSansSerif, 8F);
        private const int BORDER_SPACE = 2;
        
        public PageInformation() : this(PageInformationBorder.None, InformationType.Nothing, InformationType.Nothing, InformationType.Nothing)
        {

        }

        public PageInformation(int margin, Font font, PageInformationBorder border, InformationType left, InformationType center, InformationType right)
        {
            _margin = margin;
            _font = font;
            _border = border;
            _left = left;
            _center = center;
            _right = right;
        }

        public PageInformation(PageInformationBorder border, InformationType left, InformationType center, InformationType right)
            : this(3, DefaultFont, border, left, center, right)
        {

        }
                
        public void Draw(Graphics gr, Rectangle bounds, String docName, int pageNumber)
        {
            StringFormat oFormat = new StringFormat(StringFormat.GenericDefault);
            Pen oPen = Pens.Black;
            Brush oBrush = Brushes.Black;

            // Draw border
            switch (_border)
            {
                case PageInformationBorder.Top:
                    gr.DrawLine(oPen, bounds.Left, bounds.Top, bounds.Right, bounds.Top);
                    break;
                case PageInformationBorder.Bottom:
                    gr.DrawLine(oPen, bounds.Left, bounds.Bottom, bounds.Right, bounds.Bottom);
                    break;
                case PageInformationBorder.Box:
                    gr.DrawRectangle(oPen, bounds);
                    bounds = new Rectangle(bounds.Left + BORDER_SPACE, bounds.Top, bounds.Width - (2 * BORDER_SPACE), bounds.Height);
                    break;
                case PageInformationBorder.None:
                default:
                    break;
            }

            // Center vertically
            oFormat.LineAlignment = StringAlignment.Center;

            // Draw left side
            oFormat.Alignment = StringAlignment.Near;
            switch (_left)
            {
                case InformationType.DocumentName:
                    gr.DrawString(docName, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    gr.DrawString("Page " + pageNumber, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }

            // Draw center
            oFormat.Alignment = StringAlignment.Center;
            switch (_center)
            {
                case InformationType.DocumentName:
                    gr.DrawString(docName, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    gr.DrawString("Page " + pageNumber, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }

            // Draw right side
            oFormat.Alignment = StringAlignment.Far;
            switch (_right)
            {
                case InformationType.DocumentName:
                    gr.DrawString(docName, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.PageNumber:
                    gr.DrawString("Page " + pageNumber, _font, oBrush, bounds, oFormat);
                    break;
                case InformationType.Nothing:
                default:
                    break;
            }
        }

        private PageInformationBorder _border;
        public virtual PageInformationBorder Border
        {
            get { return _border; }
            set { _border = value; }
        }

        private InformationType _center;
        public virtual InformationType Center
        {
            get { return _center; }
            set { _center = value; }
        }
        
        [Browsable(false)]
        public bool Display
        {
            get
            {
                return (_left != InformationType.Nothing) ||
                    (_center != InformationType.Nothing) ||
                    (_right != InformationType.Nothing);
            }
        }

        private Font _font;
        public virtual Font Font
        {
            get { return _font; }
            set { _font = value; }
        }
        
        [Browsable(false)]
        public int Height
        {
            get
            {
                int iHeight = Font.Height;

                switch (_border)
                {
                    case PageInformationBorder.Top:
                    case PageInformationBorder.Bottom:
                        iHeight += BORDER_SPACE;
                        break;

                    case PageInformationBorder.Box:
                        iHeight += 2 * BORDER_SPACE;
                        break;

                    case PageInformationBorder.None:
                    default:
                        break;
                }

                return iHeight;
            }
        }

        private InformationType _left;
        public virtual InformationType Left
        {
            get { return _left; }
            set { _left = value; }
        }
        
        private int _margin;
        public virtual int Margin
        {
            get { return _margin; }
            set { _margin = value; }
        }

        private InformationType _right;
        public virtual InformationType Right
        {
            get { return _right; }
            set { _right = value; }
        }
    }
}
