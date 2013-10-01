using System;
using System.Collections.Generic;
using System.Text;
using Ela.Parsing;

namespace Ela.CodeModel
{
	public sealed class ElaListLiteral : ElaExpression
	{
        internal static readonly ElaListLiteral Empty = new ElaListLiteral();

		internal ElaListLiteral(Token tok) : base(tok, ElaNodeType.ListLiteral)
		{
			
		}
        
		public ElaListLiteral() : this(null)
		{
			
		}

        internal override bool IsLiteral()
        {
            return true;
        }
        
        internal override bool Safe()
        {
            if (_values == null)
                return true;

            foreach (var e in _values)
                if (!e.Safe())
                    return false;

            return true;
        }

        internal override void ToString(StringBuilder sb, int ident)
		{
			sb.Append('[');
			var c = 0;

			foreach (var v in Values)
			{
				if (c++ > 0)
					sb.Append(',');

				v.ToString(sb, 0);
			}

			sb.Append(']');
		}

        public bool HasValues()
        {
            return _values != null && _values.Count > 0;
        }

        internal override bool CanFollow(ElaExpression pat)
        {
            if (pat.IsIrrefutable())
                return false;

            if (pat.Type == ElaNodeType.ListLiteral)
            {
                var xs = (ElaListLiteral)pat;

                if (xs.Values.Count != Values.Count)
                    return true;

                for (var i = 0; i < Values.Count; i++)
                    if (Values[i].CanFollow(xs.Values[i]))
                        return true;

                return false;
            }

            return true;
        }
		
		private List<ElaExpression> _values;
		public List<ElaExpression> Values 
		{
			get
			{
				if (_values == null)
					_values = new List<ElaExpression>();

				return _values;
			}
			internal set { _values = value; }
		}
	}
}