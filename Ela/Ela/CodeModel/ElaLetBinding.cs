using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

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
            if (Expression != null)
            {
                Expression.ToString(sb, indent);

                sb.AppendLine();
                sb.Append(' ', indent);
                sb.Append("where ");
                Equations.ToString(sb, indent + 6);
            }
            else
            {
                sb.Append(' ', indent);
                sb.Append("let ");
                Equations.ToString(sb, 0);
            }
        }

        internal override IEnumerable<String> ExtractNames()
        {
            foreach (var n in Equations.ExtractNames())
                yield return n;

            foreach (var n in Expression.ExtractNames())
                yield return n;
        }

        public ElaEquationSet Equations { get; set; }

        public ElaExpression Expression { get; set; }
    }
}