using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;
using System.Linq;

namespace Ela.CodeModel
{
    public sealed class ElaCondition : ElaExpression
    {
        internal ElaCondition(Token tok) : base(tok, ElaNodeType.Condition)
        {
            
        }
        
        public ElaCondition() : base(ElaNodeType.Condition)
        {
            
        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            sb.Append("if ");
            Condition.ToString(sb, 0);
            sb.Append(" then ");
            True.ToString(sb, 0); 
            sb.Append(" else ");

            if (False != null)
                False.ToString(sb, 0);
        }

        internal override IEnumerable<String> ExtractNames()
        {
            foreach (var n in Condition.ExtractNames())
                yield return n;

            foreach (var n in True.ExtractNames())
                yield return n;

            if (False != null)
                foreach (var n in False.ExtractNames())
                    yield return n;
        }

        public ElaExpression Condition { get; set; }

        public ElaExpression True { get; set; }

        public ElaExpression False { get; set; }
    }
}