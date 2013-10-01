using System;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
	public sealed class ElaGuid : ElaObject
	{
		#region Construction
		private const string TYPE_NAME = "guid";

		public ElaGuid(Guid value)
		{
			Value = value;
		}
		#endregion


		#region Methods
		protected override string GetTypeName()
		{
			return TYPE_NAME;
		}


		public override int GetHashCode()
		{
			return Value.GetHashCode();
		}


		protected override int Compare(ElaValue @this, ElaValue other)
		{
			var g1 = @this.As<ElaGuid>();
			var g2 = other.As<ElaGuid>();
			return g1 != null && g2 != null ? g1.Value.CompareTo(g2.Value) : -1;
		}


		protected override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
		{
			var lg = left.As<ElaGuid>();
			var rg = right.As<ElaGuid>();

			if (lg != null && rg != null)
				return lg.Value == rg.Value;
			else if (lg != null)
				return right.Equal(left, right, ctx);
			
			return false;
		}


        protected override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
		{
			var lg = left.As<ElaGuid>();
			var rg = right.As<ElaGuid>();

			if (lg != null && rg != null)
				return lg.Value != rg.Value;
			else if (lg != null)
				return right.NotEqual(left, right, ctx);
			
			return true;
		}


		protected override string Show(ElaValue @this, ShowInfo info, ExecutionContext ctx)
		{
			return !String.IsNullOrEmpty(info.Format) ? Value.ToString(info.Format) :
				Value.ToString();
		}


		protected override ElaValue Convert(ElaValue @this, ElaTypeInfo type, ExecutionContext ctx)
		{
			if (type.ReflectedTypeCode == ElaTypeCode.String)
				return new ElaValue(Value.ToString());
			
			ctx.ConversionFailed(@this, type.ReflectedTypeName);
			return Default();
		}
		#endregion


		#region Properties
		public Guid Value { get; internal set; }
		#endregion
	}  
}
