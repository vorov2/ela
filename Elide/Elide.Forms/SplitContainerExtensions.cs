using System;
using System.Windows.Forms;

namespace Elide.Forms
{
    public static class SplitContainerExtensions
    {
        public static void SetPanel1Size(this SplitContainer cont, int size)
        {
            SetPanelSize(cont, cont.Panel1, size);
        }

        public static void SetPanel2Size(this SplitContainer cont, int size)
        {
            SetPanelSize(cont, cont.Panel2, size);
        }

        private static void SetPanelSize(SplitContainer cont, SplitterPanel panel, int size)
        {
            if (cont.Orientation == Orientation.Vertical)
            {
                var width = cont.ClientSize.Width - cont.SplitterWidth;

                if (panel == cont.Panel1)
                    cont.SplitterDistance = size;
                else
                    cont.SplitterDistance = width - size;
            }
            else
            {
                var height = cont.ClientSize.Height - cont.SplitterWidth;

                if (panel == cont.Panel1)
                    cont.SplitterDistance = size;
                else
                    cont.SplitterDistance = height - size;
            }
        }
    }
}
