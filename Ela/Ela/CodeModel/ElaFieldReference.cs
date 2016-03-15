using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.CodeModel
{
    public sealed class ElaFieldReference : ElaExpression
    {
        internal ElaFieldReference(Token tok) : base(tok, ElaNodeType.FieldReference)
        {
            
        }

        public ElaFieldReference() : this(null)
        {
            
        }

        internal override void ToString(StringBuilder sb, int ident)		
        {
            Format.PutInParens(TargetObject, sb);
            sb.Append('.');
            sb.Append(FieldName);
        }

        internal override string GetName()
        {
            return TargetObject.GetName() + '.' + FieldName;
        }

        internal override IEnumerable<String> ExtractNames()
        {
            return TargetObject.ExtractNames();
        }
        
        public string FieldName { get; set; }

        public ElaExpression TargetObject { get; set; }
    }
}