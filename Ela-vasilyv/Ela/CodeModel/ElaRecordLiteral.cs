using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public class ElaRecordLiteral : ElaExpression
	{
		internal ElaRecordLiteral(Token tok) : this(tok, ElaNodeType.RecordLiteral)
		{
			
		}
        
		internal ElaRecordLiteral(Token tok, ElaNodeType type) : base(tok, type)
		{
			Fields = new List<ElaFieldDeclaration>();
		}
        
		public ElaRecordLiteral() : this(ElaNodeType.RecordLiteral)
		{
			
		}
        
		protected ElaRecordLiteral(ElaNodeType type) : base(type)
		{
			Fields = new List<ElaFieldDeclaration>();
        }

        internal override bool IsLiteral()
        {
            return true;
        }
		
		internal override bool Safe()
        {
            foreach (var f in Fields)
                if (!f.Safe())
                    return false;

            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append('{');
			var c = 0;

			foreach (var f in Fields)
			{
				if (c++ > 0)
					sb.Append(',');

				f.ToString(sb, 0);
			}

			sb.Append('}');
		}

        internal override bool CanFollow(ElaExpression pat)
        {
            if (pat.IsIrrefutable())
                return false;

            if (pat.Type == ElaNodeType.RecordLiteral)
            {
                var rec = (ElaRecordLiteral)pat;

                if (rec.Fields.Count != Fields.Count)
                    return true;

                for (var i = 0; i < rec.Fields.Count; i++)
                    if (Fields[i].CanFollow(rec.Fields[i]))
                        return true;

                return false;
            }

            return true;
        }
		
		public List<ElaFieldDeclaration> Fields { get; private set; }
	}
}