using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.CodeModel
{
    public sealed class ElaFieldDeclaration : ElaExpression
    {
        internal ElaFieldDeclaration(Token tok) : base(tok, ElaNodeType.FieldDeclaration)
        {

        }
        
        public ElaFieldDeclaration() : base(ElaNodeType.FieldDeclaration)
        {

        }
        
        internal override bool Safe()
        {
            return FieldValue.Safe();
        }
        
        internal override void ToString(StringBuilder sb, int ident)		
        {
            sb.Append(FieldName);
            sb.Append('=');
            FieldValue.ToString(sb, 0);
        }

        internal override IEnumerable<String> ExtractNames()
        {
            return FieldValue.ExtractNames();
        }
        
        public string FieldName { get; set; }

        public ElaExpression FieldValue { get; set; }
    }
}