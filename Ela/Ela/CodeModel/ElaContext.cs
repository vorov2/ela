using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaContext : ElaExpression
    {
        internal ElaContext(Token tok) : base(tok, ElaNodeType.Context)
        {

        }

        public ElaContext() : base(ElaNodeType.Context)
        {

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            Expression.ToString(sb, 0);
            sb.Append(" ::: ");

            if (Context.Parens)
            {
                sb.Append('(');
                Context.ToString(sb, 0);
                sb.Append(')');
            }
            else
                Context.ToString(sb, 0);
        }

        public bool Tentative { get; set; }

        public ElaExpression Expression { get; set; }

        public ElaExpression Context { get; set; }
    }
}