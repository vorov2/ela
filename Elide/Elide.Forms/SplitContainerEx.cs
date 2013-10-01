using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace Elide.Forms
{
    public class SplitContainerEx : SplitContainer
    {
        private bool dragging;

        public SplitContainerEx()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (dragging)
                e.Graphics.FillRectangle(UserBrushes.Selection, e.ClipRectangle);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!IsSplitterFixed)
            {
                dragging = true;
                Refresh();
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            dragging = false;
            Refresh();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (dragging)
            {
                if (e.Button == MouseButtons.Left)
                {
                    if (Orientation.Equals(Orientation.Vertical))
                    {
                        if (e.X > 0 && e.X < Width)
                            SplitterDistance = e.X;
                    }
                    else if (e.Y > 0 && e.Y < Height)
                        SplitterDistance = e.Y;
                }
                else
                    dragging = false;
            }

            base.OnMouseMove(e);
        }
    }
}
