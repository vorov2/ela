using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaHeader : ElaExpression
    {
        internal ElaHeader(Token t) : base(t, ElaNodeType.Header)
        {

        }

        public ElaHeader() : base(ElaNodeType.Header)
        {

        }

        internal override bool Safe()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            sb.Append(Name);
            sb.Append(" # ");
            
            if ((Attributes & ElaVariableFlags.Private) == ElaVariableFlags.Private)
                sb.Append("private ");

            if ((Attributes & ElaVariableFlags.Qualified) == ElaVariableFlags.Qualified)
                sb.Append("qualified ");
        }

        public string Name { get; set; }

        public ElaVariableFlags Attributes { get; set; }
    }
}