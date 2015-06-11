using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

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

        internal override IEnumerable<String> ExtractNames()
        {
            if (Pattern != null)
                foreach (var n in Pattern.ExtractNames())
                    yield return n;

            if (Guard != null)
                foreach (var n in Guard.ExtractNames())
                    yield return n;

            if (Target != null)
                foreach (var n in Target.ExtractNames())
                    yield return n;

            if (Body != null)
                foreach (var n in Body.ExtractNames())
                    yield return n;
        }
        
        public ElaExpression Pattern { get; set; }

        public ElaExpression Guard { get; set; }

        public ElaExpression Target { get; set; }

        public ElaExpression Body { get; set; }
    }
}