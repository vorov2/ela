using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Threading;
using Elide.Core;
using Elide.CodeEditor.Views;

namespace Elide.Console
{
    internal sealed class ConsoleTextReader : TextReader
    {
        private ConsoleTextBox editor;
        private AutoResetEvent waitHandle;
        private string line;

        internal ConsoleTextReader(ConsoleTextBox editor)
        {
            this.editor = editor;
            this.editor.Submit += Submit;
        }
        
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override string ReadLine()
        {
            waitHandle = new AutoResetEvent(false);
            waitHandle.WaitOne();
            return line ?? String.Empty;
        }
        
        private void Submit(object sender, SubmitEventArgs e)
        {
            var h = waitHandle;

            if (h != null)
            {
                line = e.Text;
                h.Set();
            }
        }
        
        private void Exec(Action<Object> fun)
        {
            editor.Invoke(() => fun(null));
        }
    }
}
