using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaClassInstance : ElaExpression
    {
        internal ElaClassInstance(Token tok) : base(tok, ElaNodeType.ClassInstance)
        {
            
        }
        
        public ElaClassInstance() : this(null)
        {

        }

        internal override bool Safe()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            sb.Append("instance ");

            if (TypeClassPrefix != null)
                sb.Append(TypeClassPrefix + ".");

            sb.Append(TypeClassName);
            sb.Append(' ');

            if (TypePrefix != null)
                sb.Append(TypePrefix + ".");
            
            sb.Append(TypeName);
            sb.AppendLine();

            if (Where != null)
            {
                var kw = "  where ";
                sb.Append(kw);
                Where.ToString(sb, indent + kw.Length);
            }

            if (And != null)
            {
                sb.AppendLine();
                And.ToString(sb, indent);
            }
        }
        
        public string TypeClassPrefix { get; set; }

        public string TypeClassName { get; set; }

        public string TypeName { get; set; }

        public string TypePrefix { get; set; }

        public ElaEquationSet Where { get; set; }

        public ElaClassInstance And { get; set; }
    }
}