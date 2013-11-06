using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaLetBinding : ElaExpression
    {
        internal ElaLetBinding(Token tok) : base(tok, ElaNodeType.LetBinding)
        {

        }

        public ElaLetBinding() : base(ElaNodeType.LetBinding)
        {

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            Expression.ToString(sb, indent);
            sb.AppendLine();
            sb.Append(' ', indent);
            sb.Append("where ");
            Equations.ToString(sb, indent + 6);            
        }

        public ElaEquationSet Equations { get; set; }

        public ElaExpression Expression { get; set; }
    }
}