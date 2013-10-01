using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaEquationSet : ElaExpression
	{
		public ElaEquationSet() : base(null, ElaNodeType.EquationSet)
		{
            Equations = new List<ElaEquation>();
		}

        internal override bool Safe()
        {
            return false;
        }

        internal override void ToString(StringBuilder sb, int indent)
		{
            var c = 0;

            foreach (var e in Equations)
            {
                if (c++ > 0)
                    sb.AppendLine();

                e.ToString(sb, indent, c == 1);
            }
		}
		
		public List<ElaEquation> Equations { get; private set; }
	}
}