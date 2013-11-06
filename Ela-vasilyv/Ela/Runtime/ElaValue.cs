using System;
using System.Collections;
using System.Collections.Generic;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime
{
	public struct ElaValue : IFormattable
	{
		#region Construction
        internal ElaValue(int val, ElaObject obj)
        {
            I4 = val;
            Ref = obj;
        }
        
        public ElaValue(ElaObject val)
		{
			I4 = 0;
			Ref = val;
		}
        
		public ElaValue(int val)
		{
			I4 = val;
			Ref = ElaInteger.Instance;
		}

		public ElaValue(char val)
		{
			I4 = (Int32)val;
			Ref = ElaChar.Instance;
		}

		public ElaValue(float val)
		{
			var conv = new Conv();
			conv.R4 = val;
			I4 = conv.I4_1;
			Ref = ElaSingle.Instance;
		}

		public ElaValue(bool val)
		{
			I4 = val ? 1 : 0;
			Ref = ElaBoolean.Instance;
		}
        
		public ElaValue(string value)
		{
			I4 = 0;
			Ref = new ElaString(value);
		}
        
		public ElaValue(long val)
		{
			I4 = 0;
			Ref = new ElaLong(val);
		}
        
		public ElaValue(double val)
		{
			I4 = 0;
			Ref = new ElaDouble(val);
		}
		#endregion

		#region Casting
        internal int GetInt()
        {
            if (TypeCode == ElaTypeCode.Long)
                return (Int32)Ref.AsLong();
            else if (TypeCode == ElaTypeCode.Double)
                return (Int32)Ref.AsDouble();
            else if (TypeCode == ElaTypeCode.Single)
                return (Int32)DirectGetSingle();

            return I4;
        }

        internal bool GetBool()
        {
            if (TypeCode == ElaTypeCode.Long)
                return Ref.AsLong() != 0;

            return I4 != 0;
        }

        internal char GetChar()
        {
            if (TypeCode == ElaTypeCode.Long)
                return (Char)Ref.AsLong();
            else if (TypeCode == ElaTypeCode.String)
            {
                var s = DirectGetString();
                return s.Length == 0 ? '\0' : s[0];
            }
            
            return (Char)I4;
        }

        internal float GetSingle()
        {
            if (TypeCode == ElaTypeCode.Integer)
                return (Single)I4;
            else if (TypeCode == ElaTypeCode.Long)
                return (Single)Ref.AsLong();
            else if (TypeCode == ElaTypeCode.Double)
                return (Single)Ref.AsDouble();

            return DirectGetSingle();
        }

        internal float DirectGetSingle()
        {
            var conv = new Conv();
            conv.I4_1 = I4;
            return conv.R4;
        }

        internal string DirectGetString()
        {
            return ((ElaString)Ref).Value;
        }

        internal double GetDouble()
        {
            return TypeCode == ElaTypeCode.Double ? Ref.AsDouble() :
                TypeCode == ElaTypeCode.Single ? DirectGetSingle() :
                TypeCode == ElaTypeCode.Long ? Ref.AsLong() :
                (Double)I4;
        }

        internal long GetLong()
        {
            if (TypeCode == ElaTypeCode.Long)
                return Ref.AsLong();
            else if (TypeCode == ElaTypeCode.Double)
                return (Int64)Ref.AsDouble();
            else if (TypeCode == ElaTypeCode.Single)
                return (Int64)DirectGetSingle();

            return (Int64)I4;
        }

		public override string ToString()
		{
            return ToString(String.Empty, Culture.NumberFormat);   
        }
        
        public string ToString(string format, IFormatProvider formatProvider)
        {
            if (Ref == null)
                return "_|_";

            if (TypeId == ElaMachine.INT)
                return I4.ToString(format, formatProvider);
            else if (TypeId == ElaMachine.REA)
                return DirectGetSingle().ToString(format, Culture.NumberFormat);
            else if (TypeId == ElaMachine.CHR)
                return ((Char)I4).ToString();
            else if (TypeId == ElaMachine.BYT)
                return I4 == 1 ? "true" : "false";
            else
                return Ref.ToString(format, formatProvider);
        }
        
        public override int GetHashCode()
        {
            switch (Ref.TypeId)
            {
                case ElaMachine.INT: return I4.GetHashCode();
                case ElaMachine.REA: return DirectGetSingle().GetHashCode();
                case ElaMachine.CHR: return ((Char)I4).GetHashCode();
                case ElaMachine.BYT: return (I4 == 1).GetHashCode();
                default: return Ref.GetHashCode();
            }
        }
                
		public string GetTypeName()
		{
			return Ref != null ? Ref.GetTypeName() : "<unknown>";
		}
        
        public bool Is<T>()
        {
			return Ref is T;
        }
        
        public T As<T>() where T : class
        {
			return Ref as T;
        }

		public static ElaValue FromObject(object value)
		{
            if (value == null)
                return new ElaValue(ElaUnit.Instance);
            else if (value is ElaObject)
                return new ElaValue((ElaObject)value);
            else if (value is Int32)
                return new ElaValue((Int32)value);
            else if (value is Int64)
                return new ElaValue((Int64)value);
            else if (value is Single)
                return new ElaValue((Single)value);
            else if (value is Double)
                return new ElaValue((Double)value);
            else if (value is Boolean)
                return new ElaValue((Boolean)value);
            else if (value is Char)
                return new ElaValue((Char)value);
            else if (value is String)
                return new ElaValue((String)value);
            else if (value is ElaValue)
                return (ElaValue)value;
            else if (value is IEnumerable)
                return new ElaValue(ElaList.FromEnumerable((IEnumerable)value));
            else if (value is Delegate)
                return new ElaValue(new DynamicDelegateFunction("<f>", (Delegate)value));
            else
                throw new InvalidCastException();
		}

        public T Convert<T>()
        {
            return Convert<T>(this);
        }

        public static T Convert<T>(ElaValue val)
		{
			var ti = typeof(T);
			return (T)Convert(val, ti);
		}

		public static object Convert(ElaValue val, Type ti)
		{
            if (ti == typeof(Int32) && val.TypeId == ElaMachine.INT)
                return val.I4;
            else if (ti == typeof(Single) && val.TypeId <= ElaMachine.REA)
                return val.GetSingle();
            else if (ti == typeof(Int64) && val.TypeId <= ElaMachine.LNG)
                return val.GetLong();
            else if (ti == typeof(Double) && val.TypeId <= ElaMachine.DBL)
                return val.GetDouble();
            else if (ti == typeof(Boolean) && val.TypeId == ElaMachine.BYT)
                return val.I4 == 1;
            else if (ti == typeof(String) && val.TypeId == ElaMachine.STR)
                return val.DirectGetString();
            else if (ti == typeof(Char) && val.TypeId == ElaMachine.CHR)
                return (Char)val.I4;
            else if (ti == typeof(ElaList) && val.TypeId == ElaMachine.LST)
                return (ElaList)val.Ref;
            else if (ti == typeof(ElaRecord) && val.TypeId == ElaMachine.REC)
                return (ElaRecord)val.Ref;
            else if (ti == typeof(ElaTuple) && val.TypeId == ElaMachine.TUP)
                return (ElaTuple)val.Ref;
            else if (ti == typeof(ElaFunction) && val.TypeId == ElaMachine.FUN)
                return (ElaFunction)val.Ref;
            else if (ti == typeof(ElaModule) && val.TypeId == ElaMachine.MOD)
                return (ElaModule)val.Ref;
            else if (ti == typeof(ElaUnit) && val.TypeId == ElaMachine.UNI)
                return ElaUnit.Instance;
            else if (ti == typeof(ElaObject))
                return val.Ref;
            else if (ti == typeof(ElaValue))
                return val;
            else if (ti == typeof(Object))
                return val.AsObject();
            else if (val.TypeId == ElaMachine.LAZ)
            {
                var la = (ElaLazy)val.Ref;

                if (la.Evaled)
                    return Convert(la.Value, ti);
                else
                    throw InvalidCast(val, TypeToElaTypeCode(ti));
            }
            else if (ti.IsArray)
                return ConvertToArray(val, ti.GetElementType());
            else if (ti.IsGenericType && ti.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                return ConvertToArray(val, ti.GetGenericArguments()[0]);
            else
            {
                try
                {
                    return System.Convert.ChangeType(val.Ref, ti);
                }
                catch (Exception) { }
            }

            throw InvalidCast(val, TypeToElaTypeCode(ti));
		}

        private static ElaTypeCode TypeToElaTypeCode(System.Type ti)
        {
            if (ti == typeof(Int32))
                return ElaTypeCode.Integer;
            else if (ti == typeof(Single))
                return ElaTypeCode.Single;
            else if (ti == typeof(Int64))
                return ElaTypeCode.Long;
            else if (ti == typeof(Double))
                return ElaTypeCode.Double;
            else if (ti == typeof(Boolean))
                return ElaTypeCode.Boolean;
            else if (ti == typeof(String))
                return ElaTypeCode.String;
            else if (ti == typeof(Char))
                return ElaTypeCode.Char;
            else if (ti == typeof(ElaList))
                return ElaTypeCode.List;
            else if (ti == typeof(ElaRecord))
                return ElaTypeCode.Record;
            else if (ti == typeof(ElaTuple))
                return ElaTypeCode.Tuple;
            else if (ti == typeof(ElaFunction))
                return ElaTypeCode.Function;
            else if (ti == typeof(ElaModule))
                return ElaTypeCode.Module;
            else if (ti == typeof(ElaUnit))
                return ElaTypeCode.Unit;
            else
                return ElaTypeCode.None;
        }

		private static object ConvertToArray(ElaValue val, Type el)
		{
            var seq = (IEnumerable<ElaValue>)val.Ref;
            var len = 0;

            if (val.Ref is ElaList)
                len = ((ElaList)val.Ref).Length;
            else if (val.Ref is ElaTuple)
                len = ((ElaTuple)val.Ref).Length;

			var arr = Array.CreateInstance(el, len);
			var i = 0;

			foreach (var e in seq)
			{
				var o = Convert(e, el);
				arr.SetValue(o, i++);
			}

			return arr;
		}

		public object AsObject()
		{
			switch (TypeCode)
			{
				case ElaTypeCode.Boolean: return I4 == 1;
				case ElaTypeCode.Char: return (Char)I4;
				case ElaTypeCode.Double: return Ref.AsDouble();
				case ElaTypeCode.Integer: return I4;
				case ElaTypeCode.Long: return Ref.AsLong();
				case ElaTypeCode.Single: return DirectGetSingle();
				case ElaTypeCode.String: return DirectGetString();
				case ElaTypeCode.Unit: return null;
				case ElaTypeCode.Lazy:
                    return Ref.Force(this, ElaObject.DummyContext).Ref;
				default:
					if (Ref == null)
						throw new InvalidOperationException();
					else
						return Ref;
			}
		}
        
		public int AsInt32()
		{
			if (TypeCode == ElaTypeCode.Integer)
				return I4;

            throw InvalidCast(typeof(Int32));
		}

        public long AsInt64()
        {
            if (TypeCode == ElaTypeCode.Long)
                return Ref.AsLong();

            throw InvalidCast(typeof(Int64));
        }

        public char AsChar()
        {
            if (TypeCode == ElaTypeCode.Char)
                return (Char)I4;

            throw InvalidCast(typeof(Char));
        }
        
        public float AsSingle()
        {
            if (TypeCode == ElaTypeCode.Single)
                return DirectGetSingle();

            throw InvalidCast(typeof(Single));
        }

        public double AsDouble()
        {
            if (TypeCode == ElaTypeCode.Double)
                return Ref.AsDouble();

            throw InvalidCast(typeof(Double));
        }        

        public string AsString()
        {
            if (TypeCode == ElaTypeCode.String)
                return DirectGetString();
            else if (TypeCode == ElaTypeCode.Char)
                return ((Char)I4).ToString();

            throw InvalidCast(typeof(String));
        }

        public bool AsBoolean()
        {
            if (TypeCode == ElaTypeCode.Boolean)
                return I4 == 1;

            throw InvalidCast(typeof(Boolean));
        }

        private Exception InvalidCast(ElaTypeCode type)
        {
            return InvalidCast(this, type);
        }
        
		private static Exception InvalidCast(ElaValue val, ElaTypeCode type)
		{
            return new ElaRuntimeException(ElaRuntimeError.InvalidType, TCF.GetShortForm(type), val.GetTypeName());
        }

        private Exception InvalidCast(System.Type target)
        {
            return InvalidCast(this, target);
        }

        private static Exception InvalidCast(ElaValue val, System.Type target)
        {
            return new InvalidCastException(Strings.GetMessage("InvalidCast", TCF.GetShortForm(val.TypeCode),
                target.Name));
        }
		#endregion
        
		#region Properties
		internal int I4;

		internal ElaObject Ref;
		
        public ElaTypeCode TypeCode
		{
			get { return Ref != null ? (ElaTypeCode)TypeId : ElaTypeCode.None; }
		}
        
		internal int TypeId
		{
			get { return Ref.TypeId; }
		}
		#endregion
    }
}