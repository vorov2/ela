using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaNameReference : ElaExpression
	{
		internal ElaNameReference(Token tok) : base(tok, ElaNodeType.NameReference)
		{
			
		}
        
		public ElaNameReference() : this(null)
		{
			
		}
		
        internal override string GetName()
		{
			return Name;
        }

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
            if (Bang)
                sb.Append('!');

            if (Format.IsSymbolic(Name))
                sb.AppendFormat("({0})", Name);
            else
                sb.Append(Name);
		}

        internal override bool IsIrrefutable()
        {
            return !Uppercase;
        }

        internal override bool CanFollow(ElaExpression exp)
        {
            if (exp.IsIrrefutable())
                return false;

            if (exp.Type == ElaNodeType.NameReference)
                return Name != exp.GetName();

            return true;
        }

		public string Name { get; set; }

        public bool Uppercase { get; set; }

        public bool Bang { get; set; }
	}
}