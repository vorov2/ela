using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

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
            if (Expression != null)
            {
                Expression.ToString(sb, 0);
                sb.Append(' ');
            }

            sb.Append("::: ");

            if (Context.Parens)
            {
                sb.Append('(');
                Context.ToString(sb, 0);
                sb.Append(')');
            }
            else
                Context.ToString(sb, 0);
        }

        internal override IEnumerable<String> ExtractNames()
        {
            if (Expression != null)
                foreach (var n in Expression.ExtractNames())
                    yield return n;

            if (Context != null)
                foreach (var n in Context.ExtractNames())
                    yield return n;
        }

        public bool Tentative { get; set; }

        public ElaExpression Expression { get; set; }

        public ElaExpression Context { get; set; }
    }
}