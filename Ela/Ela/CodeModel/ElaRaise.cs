using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaRaise : ElaExpression
	{
		internal ElaRaise(Token tok) : base(tok, ElaNodeType.Raise)
		{
			
		}
        
		public ElaRaise() : this(null)
		{

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append("fail (");
            Expression.ToString(sb, 0);
            sb.Append(')');
		}

        public ElaExpression Expression { get; set; }
	}
}