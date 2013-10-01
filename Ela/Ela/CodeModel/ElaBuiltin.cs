using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaBuiltin : ElaExpression
	{
		internal ElaBuiltin(Token tok) : base(tok, ElaNodeType.Builtin)
		{

		}
        
		public ElaBuiltin() : base(ElaNodeType.Builtin)
		{

		}

        internal override bool Safe()
        {
            return true;
        }
		
		internal override void ToString(StringBuilder sb, int indent)
		{
			sb.Append("__internal ");
			sb.Append(Kind.ToString().ToLower());
		}
		
		public ElaBuiltinKind Kind { get; set; }
	}
}