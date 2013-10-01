using System;

namespace Eladoc
{
    public sealed class DocLine
    {
        public string Text { get; set; }

        public LineType Type { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }

    public enum LineType
    {
        None,

        Text,

        ElaCode,

        Code,

        EvalCode,

        Table,

        Header1,

        Header2,

        Header3,

        List,

        CodeItem
    }
}
