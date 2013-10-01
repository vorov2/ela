using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public class ElaMatch : ElaExpression
	{
		internal ElaMatch(Token tok, ElaNodeType type) : base(tok, type)
		{
            
		}

		internal ElaMatch(Token tok) : this(tok, ElaNodeType.Match)
		{
			
		}

		public ElaMatch() : this(null, ElaNodeType.Match)
		{

        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
		{
            ToString(sb, indent, "match");
		}

        internal void ToString(StringBuilder sb, int indent, string keyword)
        {
            sb.Append(keyword);
            sb.Append(' ');
            Expression.ToString(sb, 0);
            sb.Append(" with ");

            foreach (var e in Entries.Equations)
            {
                sb.AppendLine();
                e.ToString(sb, indent + keyword.Length + 1);
            }
        }

		public ElaExpression Expression { get; set; }
		
		public ElaEquationSet Entries { get; set; }
	}
}