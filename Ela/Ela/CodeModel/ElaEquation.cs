using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public class ElaEquation : ElaExpression, IEnumerable<ElaEquation>
	{
        internal ElaEquation(Token tok, ElaNodeType type) : base(tok, type)
		{
			
		}


		internal ElaEquation(Token tok) : base(tok, ElaNodeType.Equation)
		{
			
		}
        
		public ElaEquation() : this(null)
		{
			
		}

        internal override bool Safe()
        {
            return Right == null && Left.Safe();
        }

        internal void ToString(StringBuilder sb, int indent, bool omitFirstIndent)
        {
            if (indent > 0 && !omitFirstIndent)
                sb.Append(' ', indent);

            var ln = sb.Length;

            if (!Left.Parens || (Left.Type == ElaNodeType.NameReference && Format.IsSymbolic(Left.GetName())))
                Left.ToString(sb, 0);
            else
            {
                sb.Append('(');
                Left.ToString(sb, 0);
                sb.Append(')');
            }

            if (Right != null)
            {
                sb.Append(" = ");
                Right.ToString(sb, indent + (sb.Length - ln));
            }

            if (Next != null)
            {
                sb.AppendLine();
                Next.ToString(sb, indent);
            }
        }

        internal override void ToString(StringBuilder sb, int indent)
		{
            ToString(sb, indent, false);
		}

        internal bool IsFunction()
        {
            return !Left.Parens && Left.Type == ElaNodeType.Juxtaposition;
        }

        internal string GetFunctionName()
        {
            var t = ((ElaJuxtaposition)Left).Target;

            if (t.Type == ElaNodeType.NameReference)
                return t.GetName();
            else
                return null;
        }

        internal string GetLeftName()
        {
            return IsFunction() ? GetFunctionName() : Left.GetName();
        }

        internal int GetArgumentNumber()
        {
            return IsFunction() ? ((ElaJuxtaposition)Left).Parameters.Count : 0;
        }

        internal int Arguments { get; set; }

        public ElaVariableFlags VariableFlags { get; set; }

        public ElaExpression Left { get; set; }

        public ElaExpression Right { get; set; }

        public ElaEquation Next { get; set; }

        public IEnumerator<ElaEquation> GetEnumerator()
        {
            var b = this;

            while (b != null)
            {
                yield return b;
                b = b.Next;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}