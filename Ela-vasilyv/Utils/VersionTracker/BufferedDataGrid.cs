using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace VersionTracker
{
    public class BufferedDataGrid : DataGridView
    {
        public BufferedDataGrid()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }
    }
}
