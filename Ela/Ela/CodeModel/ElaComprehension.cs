using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaComprehension : ElaExpression
	{
		internal ElaComprehension(Token tok) : base(tok, ElaNodeType.Comprehension)
		{

		}
        
		public ElaComprehension() : base(null, ElaNodeType.Comprehension)
		{

		}

        internal override bool Safe()
        {
            return Generator.Safe();
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append('[');

			if (Lazy)
				sb.Append("& ");

			Generator.ToString(sb, 0);
			sb.Append(']');
		}

		public ElaGenerator Generator { get; set; }

		public bool Lazy { get; set; }
	}
}
