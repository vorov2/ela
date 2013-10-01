using System;
using System.Windows.Forms;

namespace Elide.Forms
{
    public class BufferedGridView : DataGridView
    {
        public BufferedGridView()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
        }
    }
}
