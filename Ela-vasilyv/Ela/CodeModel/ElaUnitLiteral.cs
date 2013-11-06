using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaUnitLiteral : ElaExpression
	{
		internal ElaUnitLiteral(Token tok) : base(tok, ElaNodeType.UnitLiteral)
		{
			
		}

		public ElaUnitLiteral() : this(null)
		{

        }

        internal override bool IsLiteral()
        {
            return true;
        }
        
        internal override bool Safe()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append("()");
		}
	}
}
