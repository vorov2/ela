﻿using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
    public sealed class ElaJuxtaposition : ElaExpression
    {
        private static readonly char[] opChars = new char[] { '!', '%', '&', '*', '+', '-', '.', ':', '$', '/', '\\', '<', '=', '>', '?', '@', '^', '|', '~', '"' };

        internal ElaJuxtaposition(Token tok) : base(tok, ElaNodeType.Juxtaposition)
        {
            Parameters = new List<ElaExpression>();
        }
        
        public ElaJuxtaposition() : this(null)
        {
            
        }
        
        internal override string GetName()
        {
            return Target.GetName();
        }
        
        internal override bool Safe()
        {
            return false;
        }

        internal override bool IsConstructor()
        {
            return Target != null && Target.Type == ElaNodeType.NameReference &&
                ((ElaNameReference)Target).Uppercase;
        }

        internal override void ToString(StringBuilder sb, int ident)
        {
            sb.Append(' ', ident);

            if ((Target == null || (Target.Type == ElaNodeType.NameReference && Target.GetName().IndexOfAny(opChars) != -1)) &&
                Parameters.Count == 2)
            {
                Format.PutInParens(Parameters[0], sb);
                sb.Append(' ');
                sb.Append(Target != null ? Target.GetName() : "<null>");
                sb.Append(' ');
                Format.PutInParens(Parameters[1], sb);
            }
            else
            {
                Format.PutInParens(Target, sb);

                foreach (var p in Parameters)
                {
                    sb.Append(' ');
                    Format.PutInParens(p, sb);
                }
            }
        }

        internal override bool CanFollow(ElaExpression exp)
        {
            if (exp.IsIrrefutable())
                return false;

            if (exp.Type == ElaNodeType.Juxtaposition)
            {
                var jx = (ElaJuxtaposition)exp;

                if (jx.Parameters.Count != Parameters.Count || jx.Target.GetName() != Target.GetName())
                    return true;

                for (var i = 0; i < Parameters.Count; i++)
                    if (Parameters[i].CanFollow(jx.Parameters[i]))
                        return true;

                return false;
            }

            return true;
        }

        internal override IEnumerable<String> ExtractNames()
        {
            if (Target != null)
                foreach (var n in Target.ExtractNames())
                    yield return n;

            foreach (var e in Parameters)
                foreach (var n in e.ExtractNames())
                    yield return n;
        }
        
        public ElaExpression Target { get; set; }

        public List<ElaExpression> Parameters { get; set; }

        public bool FlipParameters { get; set; }

        internal bool Spec;
    }
}