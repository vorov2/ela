using System;
using System.Text;
using Ela.Parsing;

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
		
		public ElaExpression First { get; set; }

		public ElaExpression Second { get; set; }

		public ElaExpression Last { get; set; }
	}
}