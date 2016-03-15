using Ela.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.CodeModel
{
    public sealed class ElaDoNotation : ElaExpression
    {
        internal ElaDoNotation(Token tok) : base(tok, ElaNodeType.DoNotation)
        {
            Statements = new List<ElaExpression>();
        }
        
        public ElaDoNotation() : this(null)
        {
            
        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            sb.AppendLine("do ");

            foreach (var s in Statements)
            {
                s.ToString(sb, indent + 3);
                sb.AppendLine();
            }
        }

        public List<ElaExpression> Statements { get; private set; }
    }
}
