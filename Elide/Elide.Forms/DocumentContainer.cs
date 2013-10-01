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
        
        public DocumentContainer()
        {
            Items = new List<DocumentItem>();
            contextMenu = new ContextMenuStrip();
            ContextMenuStrip = contextMenu;
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
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;

            var rect = new Rectangle(0, 0, ClientSize.Width - 1, HEIGHT);
            var fullRect = new Rectangle(0, 0, ClientSize.Width - 1, ClientSize.Height - 1);
            g.FillRectangle(UserBrushes.Window, rect);
            g.DrawRectangle(UserPens.Border, rect);
            g.DrawRectangle(UserPens.Border, fullRect);

            var text = SelectedDocument != null ? SelectedDocument.Caption : String.Empty;
            var captionRect = new Rectangle(15, 0, ClientSize.Width - LABELS_WIDTH, HEIGHT);
            var flags = TextFormatFlags.Left | TextFormatFlags.PathEllipsis | TextFormatFlags.VerticalCenter;
            TextRenderer.DrawText(g, text, Fonts.Header, captionRect, UserColors.Text, flags);

            if (Items.Count > 1)
            {
                using (var bmp = Bitmaps.Load("Corner"))
                    g.DrawImage(bmp, 5, 9);
            }
            else if (Items.Count == 1)
            {
                using (var bmp = Bitmaps.Load("CornerDisabled"))
                    g.DrawImage(bmp, 7, 7);
            }

            if (InfoBarVisible)
            {
                TextRenderer.DrawText(g, Overtype ? "OVR" : "INS", Fonts.Header, new Rectangle(ClientSize.Width - 32, 0, 30, HEIGHT), UserColors.Text, flags);
                TextRenderer.DrawText(g, Column.ToString(), Fonts.Menu, new Rectangle(ClientSize.Width - 68, 0, 25, HEIGHT), UserColors.Text, flags);
                TextRenderer.DrawText(g, "Col", Fonts.Header, new Rectangle(ClientSize.Width - 90, 0, 25, 20), UserColors.Text, flags);
                TextRenderer.DrawText(g, Line.ToString(), Fonts.Menu, new Rectangle(ClientSize.Width - 123, 0, 35, HEIGHT), UserColors.Text, flags);
                TextRenderer.DrawText(g, "Line", Fonts.Header, new Rectangle(ClientSize.Width - 150, 0, 30, HEIGHT), UserColors.Text, flags);

                using (var bmp = Bitmaps.Load("Bounds"))
                    g.DrawImage(bmp, ClientSize.Width - 167, 5);
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.X < ClientSize.Width - LABELS_WIDTH)
            {
                GenerateMenu();
                contextMenu.Font = Fonts.Menu;
                contextMenu.Renderer = new DocumentMenuRenderer(ClientSize.Width - LABELS_WIDTH);
                contextMenu.Show(this, new Point(0, HEIGHT));
            }
        }

        private void GenerateMenu()
        {
            contextMenu.Items.Clear();
            var c = 0;
            var max = ClientSize.Height - 50;

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
