using System;
using System.Text;
using Ela.Parsing;
using System.Collections.Generic;

namespace Ela.CodeModel
{
    public sealed class ElaAs : ElaExpression
    {
        internal ElaAs(Token t) : base(t, ElaNodeType.As)
        {

        }

        public ElaAs() : base(ElaNodeType.As)
        {

        }

        internal override bool Safe()
        {
            return Expression.Safe();
        }

        internal override IEnumerable<String> ExtractNames()
        {
            return Expression.ExtractNames();
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            Format.PutInParens(Expression, sb);
            sb.Append("@");
            sb.Append(Name);
        }

        internal override bool IsIrrefutable()
        {
            return Expression.IsIrrefutable();
        }

        internal override bool CanFollow(ElaExpression exp)
        {
            return Expression.CanFollow(exp);
        }

        public string Name { get; set; }

        public ElaExpression Expression { get; set; }
    }
}