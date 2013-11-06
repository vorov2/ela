using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

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
		
        public ElaExpression Condition { get; set; }

		public ElaExpression True { get; set; }

		public ElaExpression False { get; set; }
	}
}