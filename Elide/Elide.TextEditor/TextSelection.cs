using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.TextEditor
{
    internal sealed class TextSelection
    {
        public int Start { get; set; }

        public int End { get; set; }

        public int Caret { get; set; }

        public bool Main { get; set; }
    }
}
