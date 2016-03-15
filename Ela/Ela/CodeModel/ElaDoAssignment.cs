using Ela.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.CodeModel
{
    public sealed class ElaDoAssignment : ElaExpression
    {
        internal ElaDoAssignment(Token tok) : base(tok, ElaNodeType.DoAssignment)
        {
            
        }

        public ElaDoAssignment() : this(null)
        {

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            sb.Append(' ', ident);
            Format.PutInParens(Left, sb);
            sb.Append(" <- ");
            Format.PutInParens(Right, sb);
        }

        public ElaExpression Left { get; set; }

        public ElaExpression Right { get; set; }
    }
}
