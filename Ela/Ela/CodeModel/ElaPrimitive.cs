using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaPrimitive : ElaExpression
	{
		internal ElaPrimitive(Token tok) : base(tok, ElaNodeType.Primitive)
		{
			
		}
        
		public ElaPrimitive() : base(ElaNodeType.Primitive)
		{
			
		}

        internal override bool Safe()
        {
            return true;
        }

        internal override bool IsLiteral()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append(Value);
		}

        internal override bool CanFollow(ElaExpression exp)
        {
            if (exp.IsIrrefutable())
                return false;

            if (exp.Type == ElaNodeType.Primitive)
            {
                var lit = (ElaPrimitive)exp;
                return !Value.Equals(lit.Value);
            }

            return true;
        }
		
		public ElaLiteralValue Value { get; set; }
	}
}