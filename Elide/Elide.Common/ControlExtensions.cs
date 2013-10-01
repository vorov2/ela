using System;
using System.Windows.Forms;

namespace Elide
{
    public static class ControlExtensions
    {
        public static void Invoke(this Control control, Action fun)
        {
            control.Invoke(fun);
        }
    }
}
