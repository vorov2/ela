using System;
using System.Text;
using Ela.Parsing;

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
            Format.PutInBraces(TargetObject, sb);
			sb.Append('.');
			sb.Append(FieldName);
		}

        internal override string GetName()
        {
            return TargetObject.GetName() + '.' + FieldName;
        }
		
        public string FieldName { get; set; }

		public ElaExpression TargetObject { get; set; }
	}
}