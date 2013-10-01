using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Runtime.CompilerServices;

namespace Elide.Console
{
    internal sealed class ConsoleTextWriter : TextWriter
    {
        private ConsoleTextBox editor;

        internal ConsoleTextWriter(ConsoleTextBox editor)
        {
            this.editor = editor;
        }
        
        public override void Write(char value)
        {
            editor.Invoke(() => editor.Print(value.ToString()));
        }
        
        public override void WriteLine(string value)
        {
            editor.Invoke(() => editor.PrintLine(value ?? String.Empty));
        }

        public override void Write(string value)
        {
            editor.Invoke(() => editor.Print(value));
        }
                
        public override Encoding Encoding
        {
            get { return Encoding.UTF8; }
        }
    }
}
