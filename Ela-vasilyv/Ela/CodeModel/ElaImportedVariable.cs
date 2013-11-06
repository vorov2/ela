using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaImportedVariable : ElaExpression
	{
		internal ElaImportedVariable(Token tok) : base(tok, ElaNodeType.ImportedVariable)
		{
			
		}
        
		public ElaImportedVariable() : this(null)
		{

        }

        internal override bool Safe()
        {
            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			if (Private)
				sb.Append("private ");

			if (Name == LocalName)
				sb.Append(Name);
			else
				sb.AppendFormat("{0}={1}", LocalName, Name);
		}
		
		public string Name { get; set; }

		public string LocalName { get; set; }

		public bool Private { get; set; }
	}
}