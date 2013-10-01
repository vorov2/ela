using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaTry : ElaMatch
	{
		internal ElaTry(Token tok) : base(tok, ElaNodeType.Try)
		{

		}
        
		public ElaTry() : base(null, ElaNodeType.Try)
		{

		}

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
		{
			base.ToString(sb, indent, "try");
		}
	}
}