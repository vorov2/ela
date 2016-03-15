using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaTypeCheck : ElaExpression
    {
        internal ElaTypeCheck(Token tok) : base(tok, ElaNodeType.TypeCheck)
        {

        }

        internal ElaTypeCheck() : base(ElaNodeType.TypeCheck)
        {

        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            Format.PutInParens(Expression, sb);

            sb.Append(" is ");

            if (_traits != null)
            {
                var c = 0;

                foreach (var ti in _traits)
                {
                    if (c++ > 0)
                        sb.Append(' ');

                    sb.Append(ti.ToString());
                }
            }
        }

        internal override IEnumerable<String> ExtractNames()
        {
            return Expression.ExtractNames();
        }

        public ElaExpression Expression { get; set; }

        private List<TraitInfo> _traits;
        public List<TraitInfo> Traits
        {
            get { return _traits ?? (_traits = new List<TraitInfo>()); }
        }
    }
}
