using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.CodeModel
{
    public sealed class ElaNewtype : ElaExpression
    {
        internal ElaNewtype(Token tok): base(tok, ElaNodeType.Newtype)
        {
            Constructors = new List<ElaExpression>();
            ConstructorFlags = new List<ElaVariableFlags>();
        }
        
        public ElaNewtype() : this(null)
        {

        }
        
        internal override bool Safe()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            if (Extends)
                sb.Append("data ");
            else if (Opened)
                sb.Append("opentype ");
            else
                sb.Append("type ");

            if (Prefix != null)
            {
                sb.Append(Prefix);
                sb.Append('.');
            }

            sb.Append(Name);

            if (HasBody)
            {
                sb.Append(" = ");

                for (var i = 0; i < Constructors.Count; i++)
                {
                    if (i > 0)
                        sb.Append(" | ");

                    var c = Constructors[i];
                    c.ToString(sb, 0);

                    if ((ConstructorFlags[i] & ElaVariableFlags.Private) == ElaVariableFlags.Private)
                        sb.Append(" # private");
                }
            }

            if (And != null)
            {
                sb.AppendLine();
                And.ToString(sb, indent);
            }
        }

        public List<ElaExpression> Constructors { get; private set; }

        public List<ElaVariableFlags> ConstructorFlags { get; private set; }

        public bool HasBody
        {
            get { return Constructors.Count > 0; }
        }

        public string Name { get; set; }

        public string Prefix { get; set; }

        public bool Extends { get; set; }
        
        public bool Opened { get; set; }
        
        public bool Header { get; set; }

        public ElaVariableFlags Flags { get; set; }

        public ElaNewtype And { get; set; }
    }
}