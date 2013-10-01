using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Elide.Forms
{
	public sealed class SwitchBar : Control
    {
        private ContextMenuStrip contextMenu;
        private int hoverItem = -1;
		private int selItem = -1;

        public SwitchBar()
		{
			SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer
				| ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
			_items = new List<SwitchBarItem>();
            contextMenu = new ContextMenuStrip();
		}		       

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.HighQuality;
            
            g.FillRectangle(UserBrushes.Window, ClientRectangle);

            g.DrawImage(Bitmaps.Load("Corner"), 5, 9);
            g.DrawRectangle(UserPens.Border, new Rectangle(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width - 1, ClientRectangle.Height));
            
            if (SelectedItem != null)
            {
                var width = (Int32)g.MeasureString(SelectedItem.Caption, Fonts.Caption).Width + 20;

                TextRenderer.DrawText(g, SelectedItem.Caption, Fonts.Caption, new Rectangle(14, 0, width, ClientSize.Height), UserColors.Text,
                    TextFormatFlags.VerticalCenter | TextFormatFlags.Left | TextFormatFlags.EndEllipsis);
            }
        }
        
		protected override void OnMouseClick(MouseEventArgs e)
		{
            contextMenu.Items.Clear();

            for (var i = 0; i < Items.Count; i++)
            {
                var it = Items[i];

                if (it != SelectedItem)
                {
                    var mi = new ToolStripMenuItem { Text = it.Caption, Tag = it };
                    mi.Click += (o, a) =>
                    {
                        var itm = (SwitchBarItem)((ToolStripMenuItem)o).Tag;
                        SelectedIndex = Items.IndexOf(itm);
                    };

                    contextMenu.Items.Add(mi);
                }
            }

            contextMenu.Renderer = new DocumentMenuRenderer(ClientSize.Width - 20);
            contextMenu.Show(this, 0, 20);
		}
        		
		public event EventHandler<SwitchBarEventArgs> SelectedIndexChanged;
		private void OnSelectedIndexChanged(SwitchBarEventArgs e)
		{
			var h = SelectedIndexChanged;

			if (h != null)
				h(this, e);
		}
                
		private List<SwitchBarItem> _items;
		public List<SwitchBarItem> Items
		{
			get { return _items; }
		}

        public SwitchBarItem SelectedItem
        {
            get { return selItem == -1 ? null : Items[selItem]; }
        }
        
		public int SelectedIndex
		{
			get { return selItem; }
			set
			{
				if (value >= Items.Count)
					throw new IndexOutOfRangeException();

				if (selItem != value)
				{
                    selItem = value;

					if (hoverItem == value)
						hoverItem = -1;

					Refresh();
					OnSelectedIndexChanged(new SwitchBarEventArgs(SelectedItem));
				}
			}
		}
	}
}