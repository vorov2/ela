using System;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public abstract class ElaExpression
	{
        protected ElaExpression(ElaNodeType type) : this(null, type)
		{

		}
        
		internal ElaExpression(Token tok, ElaNodeType type)
		{
			Type = type;

			if (tok != null)
			{
				Line = tok.line;
				Column = tok.col;
			}
		}
		
        public void SetLinePragma(int line, int column)
		{
			Line = line;
			Column = column;
		}

        internal virtual string GetName()
		{
			return null;
		}
        
        internal virtual bool Safe()
        {
            return false;
        }
        
		public override string ToString()
		{
			var sb = new StringBuilder();
			ToString(sb, 0);
			return sb.ToString();
		}
        
		internal abstract void ToString(StringBuilder sb, int ident);

        internal virtual bool IsLiteral()
        {
            return false;
        }

        internal virtual bool IsConstructor()
        {
            return false;
        }

        internal virtual bool CanFollow(ElaExpression exp)
        {
            return !exp.IsIrrefutable();
        }

        internal virtual bool IsIrrefutable()
        {
            return false;
        }
		
        public int Line { get; internal set; }

        public int Column { get; internal set; }
		
		public ElaNodeType Type { get; protected set; }

        public bool Parens { get; set; }

        internal string Code
        {
            get { return ToString(); }
        }
	}
}