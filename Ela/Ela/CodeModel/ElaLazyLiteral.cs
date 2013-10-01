using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaLazyLiteral : ElaExpression
	{
		internal ElaLazyLiteral(Token tok) : base(tok, ElaNodeType.LazyLiteral)
		{

		}
        
		public ElaLazyLiteral() : base(ElaNodeType.LazyLiteral)
		{

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append("(& ");
            Expression.ToString(sb, 0);
			sb.Append(')');
		}

        internal override bool IsIrrefutable()
        {
            return true;
        }

        public ElaExpression Expression { get; set; }
	}
}