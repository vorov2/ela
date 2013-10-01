using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;

namespace Elide.Forms
{
    public abstract class HostControl : Panel
    {
        protected HostControl()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }
        
        protected void SetPadding(int left, int top, int right, int bottom)
        {
            base.Padding = new Padding(left, top, right, bottom);
        }
        
        [Browsable(false)]
        public new Padding Padding
        {
            get { return base.Padding; }
            set { }
        }
    }
}
