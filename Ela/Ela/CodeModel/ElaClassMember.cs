using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaClassMember : ElaExpression
    {
        internal ElaClassMember(Token tok) : base(tok, ElaNodeType.ClassMember)
        {
            
        }
        
        public ElaClassMember() : this(null)
        {

        }
        
        internal override void ToString(StringBuilder sb, int ident)
        {
            if (Format.IsSymbolic(Name))
            {
                sb.Append('(');
                sb.Append(Name);
                sb.Append(')');
            }
            else
                sb.Append(Name);

            if (Mask != 0)
            {
                sb.Append(" ");

                for (var i = 0; i < Components; i++)
                {
                    if (i > 0)
                        sb.Append("->");

                    if ((Mask & (1 << i)) == (1 << i))
                        sb.Append("a");
                    else
                        sb.Append("_");
                }
            }
        }

        internal override bool Safe()
        {
            return true;
        }
        
        public string Name { get; set; }

        public int Components { get; set; }

        public int Mask { get; set; }
    }
}