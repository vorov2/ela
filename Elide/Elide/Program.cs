using System;
using System.Windows.Forms;

namespace Elide
{
    internal static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ElaLoader.AttachLoader();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
            Application.Run(new ProgramContext(args));
        }
    }
}
