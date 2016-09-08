using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Elide.Forms
{
    public class DocumentContainer : HostControl
    {
        private const int HEIGHT = 20;
        private const int LABELS_WIDTH = 200;
        private const int MAX_ITEMS = 30;
        private readonly ContextMenuStrip contextMenu;
        private int hoverItem = -1;
        private int lastWidth = 0;
        
        public DocumentContainer()
        {
            Items = new List<DocumentItem>();
            contextMenu = new ContextMenuStrip();
            ContextMenuStrip = contextMenu;
            contextMenu.Closed += (o,e) => { hoverItem = -1; Refresh(); };
            SetPadding(1, HEIGHT + 1, 1, 1);
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
            Application.Idle += Idle;
        }

        private void Idle(object sender, EventArgs e)
        {
            if (Items.Any(i => i.IsCaptionChanged()))
            {
                Invoke((Action)Refresh);
                OnDocumentCaptionRedraw();
            }

            if (Items.Count > 0)
                OnInfoBarUpdate();
            else
            {
                if (InfoBarVisible)
                {
                    InfoBarVisible = false;
                    Refresh();
                }
            }
        }

        public DocumentItem AddDocument(Func<String> captionGet, object tag)
        {
            var doc = new DocumentItem(captionGet, tag);
            Items.Add(doc);
            return doc;
        }

        public DocumentItem FindItemByTag(object tag)
        {
            return Items.FirstOrDefault(d => d.Tag == tag);
        }
        
        public void RemoveDocument(DocumentItem doc)
        {
            Items.Remove(doc);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            PaintMain(e.Graphics);
        }

        private void PaintMain(Graphics g, bool fast = false)
        {
            var textColor = UserColors.Text;
            var corner = "Corner";

            var rect = new Rectangle(0, 0, ClientSize.Width - Dpi.ScaleX(1), Dpi.ScaleY(HEIGHT));

            if (!fast)
            {
                var fullRect = new Rectangle(0, 0, ClientSize.Width - Dpi.ScaleX(1), ClientSize.Height - Dpi.ScaleY(1));
                g.FillRectangle(UserBrushes.Window, rect);
                g.DrawRectangle(UserPens.Border, rect);
                g.DrawRectangle(UserPens.Border, fullRect);
            }

            var text = SelectedDocument != null ? SelectedDocument.Caption : String.Empty;
            var captionRect = new Rectangle(Dpi.ScaleX(15), 0, ClientSize.Width - Dpi.ScaleY(LABELS_WIDTH), Dpi.ScaleY(HEIGHT));
            var flags = TextFormatFlags.Left | TextFormatFlags.PathEllipsis | TextFormatFlags.VerticalCenter;
            lastWidth = (Int32)g.MeasureString(text, Fonts.Header).Width;

            if (lastWidth > captionRect.Width)
                lastWidth = captionRect.Width;

            if (hoverItem == 0 || contextMenu.Visible)
            {
                textColor = UserColors.HighlightText;
                corner = "CornerWhite";
                g.FillRectangle(UserBrushes.Selection, new Rectangle(Dpi.ScaleX(3), Dpi.ScaleY(4), Dpi.ScaleX(16) + lastWidth, Dpi.ScaleY(14)));
            }
            else if (hoverItem == -1 && fast)
                g.FillRectangle(UserBrushes.Window, new Rectangle(Dpi.ScaleX(3), Dpi.ScaleY(4), Dpi.ScaleX(16) + lastWidth, Dpi.ScaleY(14)));

            TextRenderer.DrawText(g, text, Fonts.Header, captionRect, textColor, flags);

            if (Items.Count > 1)
            {
                using (var bmp = Bitmaps.Load(corner))
                    g.DrawImage(bmp, new Rectangle(Dpi.ScaleX(5), Dpi.ScaleY(9), Dpi.ScaleX(bmp.Width), Dpi.ScaleY(bmp.Height)));
            }
            else if (Items.Count == 1)
            {
                using (var bmp = Bitmaps.Load("CornerDisabled"))
                    g.DrawImage(bmp, new Rectangle(Dpi.ScaleX(7), Dpi.ScaleY(7), Dpi.ScaleX(bmp.Width), Dpi.ScaleY(bmp.Height)));
            }

            if (InfoBarVisible && !fast)
            {
                TextRenderer.DrawText(g, Overtype ? "OVR" : "INS", Fonts.Header, new Rectangle(ClientSize.Width - Dpi.ScaleX(32), 0, Dpi.ScaleX(30), Dpi.ScaleY(HEIGHT)), UserColors.Text, flags);
                TextRenderer.DrawText(g, Column.ToString(), Fonts.Menu, new Rectangle(ClientSize.Width - Dpi.ScaleX(68), 0, Dpi.ScaleX(25), Dpi.ScaleY(HEIGHT)), UserColors.Text, flags);
                TextRenderer.DrawText(g, "Col", Fonts.Header, new Rectangle(ClientSize.Width - Dpi.ScaleX(90), 0, Dpi.ScaleX(25), Dpi.ScaleY(20)), UserColors.Text, flags);
                TextRenderer.DrawText(g, Line.ToString(), Fonts.Menu, new Rectangle(ClientSize.Width - Dpi.ScaleX(123), 0, Dpi.ScaleX(35), Dpi.ScaleY(HEIGHT)), UserColors.Text, flags);
                TextRenderer.DrawText(g, "Line", Fonts.Header, new Rectangle(ClientSize.Width - Dpi.ScaleX(150), 0, Dpi.ScaleX(30), Dpi.ScaleY(HEIGHT)), UserColors.Text, flags);

                using (var bmp = Bitmaps.Load("Bounds"))
                    g.DrawImage(bmp, new Rectangle(ClientSize.Width - Dpi.ScaleX(167), Dpi.ScaleY(5), Dpi.ScaleX(bmp.Width), Dpi.ScaleY(bmp.Height)));
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (Items.Count > 1 && e.X <= Dpi.ScaleX(16) + lastWidth && e.Y < Dpi.ScaleY(HEIGHT))
            {
                if (hoverItem != 0)
                {
                    hoverItem = 0;
                    using (var g = CreateGraphics())
                        PaintMain(g, fast: true);
                }
            }
            else if (hoverItem != -1)
            {
                hoverItem = -1;
                using (var g = CreateGraphics())
                    PaintMain(g, fast: true);
            }

            base.OnMouseMove(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            if (hoverItem != -1)
            {
                hoverItem = -1;
                using (var g = CreateGraphics())
                    PaintMain(g, fast: true);
            }
 
            base.OnMouseLeave(e);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.X < Dpi.ScaleX(16) + lastWidth)
            {
                GenerateMenu();
                contextMenu.Font = Fonts.Menu;
                contextMenu.Renderer = new DocumentMenuRenderer(ClientSize.Width - Dpi.ScaleX(LABELS_WIDTH));
                contextMenu.Show(this, new Point(Dpi.ScaleX(3), Dpi.ScaleY(HEIGHT - 2)));
            }
        }

        private void GenerateMenu()
        {
            contextMenu.Items.Clear();
            var c = 0;
            var max = ClientSize.Height - Dpi.ScaleY(50);

            for (var i = 0; i < Items.Count; i++)
            {
                if (Items[i] != SelectedDocument)
                {
                    var it = Items[i];
                    var mi = new ToolStripMenuItem(it.Caption) { Tag = Items[i].Tag };
                    mi.Click += (o,e) => {
                        OnUserDocumentSelect(((ToolStripMenuItem)o).Tag);
                        Refresh();
                    };
                    contextMenu.Items.Add(mi);
                    c += 16;
                }

                if (c >= max || i > MAX_ITEMS)
                {
                    var mi = new ToolStripMenuItem("...");
                    mi.Click += (o, e) => OnMoreDocumentsRequested();
                    contextMenu.Items.Add(mi);
                    break;
                }
            }
        }
        
        public DocumentItem SelectedDocument
        {
            get
            {
                if (SelectedDocumentFunc != null)
                {
                    var tag = SelectedDocumentFunc();
                    return FindItemByTag(tag);
                }
                else
                    return null;
            }
        }

        public Func<Object> SelectedDocumentFunc { get; set; }

        public int Line { get; set; }

        public int Column { get; set; }

        public bool Overtype { get; set; }

        public bool InfoBarVisible { get; set; }

        internal List<DocumentItem> Items { get; private set; }

        public event EventHandler<ObjectEventArgs> UserDocumentSelect;
        private void OnUserDocumentSelect(object obj)
        {
            var h = UserDocumentSelect;

            if (h != null)
                h(this, new ObjectEventArgs(obj));
        }

        public event EventHandler MoreDocumentsRequested;
        private void OnMoreDocumentsRequested()
        {
            var h = MoreDocumentsRequested;

            if (h != null)
                h(this, EventArgs.Empty);
        }

        public event EventHandler DocumentCaptionRedraw;
        private void OnDocumentCaptionRedraw()
        {
            var h = DocumentCaptionRedraw;

            if (h != null)
                h(this, EventArgs.Empty);
        }

        public event EventHandler InfoBarUpdate;
        private void OnInfoBarUpdate()
        {
            var h = InfoBarUpdate;

            if (h != null)
                h(this, EventArgs.Empty);
        }
    }
}
