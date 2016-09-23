using Ela.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.CodeModel
{
    public sealed class ElaDebugPoint : ElaExpression
    {
        public ElaDebugPoint() : this(null)
        {

        }

        internal ElaDebugPoint(Token tok) : base(tok, ElaNodeType.DebugPoint)
        {

        }

        internal override bool Safe()
        {
            return true;
        }

        public override string ToString()
        {
            var act = Action == ElaDebugAction.TracePoint ? "trace" : String.Empty;
            return "##[" + act + (Data != null ? " " + Data : String.Empty) + "]#";
        }

        internal override void ToString(StringBuilder sb, int indent)
        {
            sb.Append(ToString());
            sb.Append(' ');

            if (Expression != null)
                Expression.ToString(sb, indent);
        }

        internal override bool CanFollow(ElaExpression _)
        {
            return true;
        }

        public ElaDebugAction Action { get; set; }

        public string Data { get; set; }

        public ElaExpression Expression { get; set; }
    }
}
