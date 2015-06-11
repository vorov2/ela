using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.CodeModel
{
    public sealed class ElaRange : ElaExpression
    {
        internal ElaRange(Token tok) : base(tok, ElaNodeType.Range)
        {

        }
        
        public ElaRange() : base(ElaNodeType.Range)
        {

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            sb.Append('[');
            First.ToString(sb, 0);

            if (Second != null)
            {
                sb.Append(',');
                Second.ToString(sb, 0);
            }

            sb.Append(" .. ");

            if (Last != null)
                Last.ToString(sb, 0);

            sb.Append(']');
        }

        internal override IEnumerable<String> ExtractNames()
        {
            if (First != null)
                foreach (var n in First.ExtractNames())
                    yield return n;

            if (Second != null)
                foreach (var n in Second.ExtractNames())
                    yield return n;

            if (Last != null)
                foreach (var n in Last.ExtractNames())
                    yield return n;
        }
        
        public ElaExpression First { get; set; }

        public ElaExpression Second { get; set; }

        public ElaExpression Last { get; set; }
    }
}