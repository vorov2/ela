using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaGenerator : ElaExpression
	{
		internal ElaGenerator(Token tok) : base(tok, ElaNodeType.Generator)
		{
			
		}
        
		public ElaGenerator() : this(null)
		{
			
		}
        
        internal override bool Safe()
        {
            return (Guard == null || Guard.Safe()) && Target != null && Target.Safe() && Body != null && Body.Safe(); 
        }

        internal override void ToString(StringBuilder sb, int ident)		
		{
			var sbNew = new StringBuilder();
			var sel = GetSelect(this, sbNew);
            sel.ToString(sb, 0);
            sb.Append(" \\\\ ");
            sb.Append(sbNew);
		}
        
		private ElaExpression GetSelect(ElaGenerator gen, StringBuilder sb)
		{
			gen.Pattern.ToString(sb, 0);
			sb.Append(" <- ");
			gen.Target.ToString(sb, 0);

			if (Guard != null)
			{
				sb.Append(" | ");
				gen.Guard.ToString(sb, 0);
			}

			if (gen.Body.Type == ElaNodeType.Generator)
			{
				sb.Append(',');
				return GetSelect((ElaGenerator)gen.Body, sb);
			}
			else
				return gen.Body;
		}
		
		public ElaExpression Pattern { get; set; }

		public ElaExpression Guard { get; set; }

		public ElaExpression Target { get; set; }

		public ElaExpression Body { get; set; }
	}
}