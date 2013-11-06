using System;

namespace Ela.CodeModel
{
	public struct ElaLiteralValue : IEquatable<ElaLiteralValue>
	{
		private Conv data;
		private object objval;

		public ElaLiteralValue(int val)
		{
			data = new Conv();
			data.I4_1 = val;
			objval = null;
			LiteralType = ElaTypeCode.Integer;
		}

		public ElaLiteralValue(float val)
		{
			data = new Conv();
			data.R4 = val;
			objval = null;
			LiteralType = ElaTypeCode.Single;
		}
        
		public ElaLiteralValue(string val)
		{
			data = new Conv();
			objval = val;
			LiteralType = ElaTypeCode.String;
        }
        
		public ElaLiteralValue(bool val)
		{
			data = new Conv();
			data.I4_1 = val ? 1 : 0;
			objval = null;
			LiteralType = ElaTypeCode.Boolean;
		}

		public ElaLiteralValue(char val)
		{
			data = new Conv();
			data.I4_1 = (Int32)val;
			objval = null;
			LiteralType = ElaTypeCode.Char;	
		}

		public ElaLiteralValue(long val)
		{
			data = new Conv();
			data.I8 = val;
			objval = null;
			LiteralType = ElaTypeCode.Long;	
		}

		public ElaLiteralValue(double val)
		{
			data = new Conv();
			data.R8 = val;
			objval = null;
			LiteralType = ElaTypeCode.Double;	
		}

		public ElaLiteralValue(ElaTypeCode type)
		{
			data = new Conv();
			objval = null;
			LiteralType = type;			
		}

		public int AsInteger()
		{
			return data.I4_1;
		}

		public float AsReal()
		{
			return data.R4;
		}
				
		public long AsLong()
		{
			return data.I8;
		}

		public double AsDouble()
		{
			return data.R8;
		}
        
		public bool AsBoolean()
		{
			return data.I4_1 == 1;
		}

		public char AsChar()
		{
			return (Char)data.I4_1;
		}

		public string AsString()
		{
			return (String)objval;
		}

		internal Conv GetData()
		{
			return data;
		}

		public ElaLiteralValue MakeNegative()
		{
			if (LiteralType == ElaTypeCode.Integer)
				return new ElaLiteralValue(-AsInteger());
			else if (LiteralType == ElaTypeCode.Long)
				return new ElaLiteralValue(-AsLong());
			else if (LiteralType == ElaTypeCode.Single)
				return new ElaLiteralValue(-AsReal());
			else if (LiteralType == ElaTypeCode.Double)
				return new ElaLiteralValue(-AsDouble());
			else
				throw new NotSupportedException();
		}

		public override string ToString()
		{
			switch (LiteralType)
			{
				case ElaTypeCode.Integer: return AsInteger().ToString(Culture.NumberFormat);
				case ElaTypeCode.Long: return AsLong().ToString(Culture.NumberFormat) + "L";
				case ElaTypeCode.Boolean: return AsBoolean().ToString().ToLower();
				case ElaTypeCode.Char: return "'" + AsChar().ToString() + "'";
				case ElaTypeCode.Single: return AsReal().ToString(Culture.NumberFormat);
				case ElaTypeCode.Double: return AsDouble().ToString(Culture.NumberFormat) + "D";
				case ElaTypeCode.String: return "\"" + AsString() + "\"";
				default: return String.Empty;
			}
		}
        
		public bool Equals(ElaLiteralValue value)
		{
			if (LiteralType != value.LiteralType)
				return false;

			switch (LiteralType)
			{
				case ElaTypeCode.Integer: return AsInteger() == value.AsInteger();
				case ElaTypeCode.Long: return AsLong() == value.AsLong();
				case ElaTypeCode.Boolean: return AsBoolean() == value.AsBoolean();
				case ElaTypeCode.Char: return AsChar() == value.AsChar();
				case ElaTypeCode.Single: return AsReal() == value.AsReal();
				case ElaTypeCode.Double: return AsDouble() == value.AsDouble();
				case ElaTypeCode.String: return AsString() == value.AsString();
				default: return true;
			}
		}

        public bool IsNegative()
        {
            if (LiteralType == ElaTypeCode.Integer)
                return AsInteger() < 0;

            if (LiteralType == ElaTypeCode.Long)
                return AsLong() < 0;

            if (LiteralType == ElaTypeCode.Single)
                return AsReal() < 0;

            if (LiteralType == ElaTypeCode.Double)
                return AsDouble() < 0;

            return false;
        }

		public readonly ElaTypeCode LiteralType;
	}
}