using Ela.Linking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using BI = Ela.Library.Wrapper<System.Numerics.BigInteger>;
using Ela.Runtime.ObjectModel;
using System.Globalization;

namespace Ela.Library.General
{
    public sealed class BigIntegerModule : ForeignModule
    {
        internal static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public BigIntegerModule()
        {

        }

        public override void Initialize()
        {
            Add<BI,BI,BI>("add", Sum);
            Add<BI,BI,BI>("subtract", Subtract);
            Add<BI,BI,BI>("multiply", Multiply);
            Add<BI,BI,BI>("divide", Divide);
            Add<BI,BI>("negate", Negate);
            Add<BI,BI,BI>("pow", Power);
            Add<String,BI>("parse", Parse);
            Add<BI,BI,Boolean>("equals", Equals);
            Add<BI,String,String>("show", Show);
            Add<BI,BI,Boolean>("greater", Greater);
            Add<BI,BI,Boolean>("lesser", Lesser);
            Add<BI,BI,BI>("mod", Modulo);
            Add<BI,BI,BI>("rem", Remainder);
            Add<BI,Int32>("toInt", ToInt32);
            Add<BI,Int64>("toLong", ToInt64);
            Add<Int32,BI>("fromInt", FromInt32);
            Add<Int64,BI>("fromLong", FromInt64);
        }

        public BI Sum(BI left, BI right)
        {
            return new BI(left.Value + right.Value);
        }

        public BI Subtract(BI left, BI right)
        {
            return new BI(left.Value - right.Value);
        }

        public BI Multiply(BI left, BI right)
        {
            return new BI(left.Value * right.Value);
        }

        public BI Divide(BI left, BI right)
        {
            return new BI(left.Value / right.Value);
        }

        public BI Negate(BI val)
        {
            return new BI(-val.Value);
        }

        public BI Power(BI left, BI right)
        {
            return new BI(BigInteger.Pow(left.Value, (Int32)right.Value));
        }

        public BI Parse(string str)
        {
            var bi = default(BigInteger);
            
            if (!BigInteger.TryParse(str, NumberStyles.Integer, Culture.NumberFormat, out bi))
                return new BI(new BigInteger(0));

            return new BI(bi);
        }

        public bool Equals(BI left, BI right)
        {
            return left.Value == right.Value;
        }

        public string Show(BI val, string format)
        {
            return val.Value.ToString(format, Culture.NumberFormat);
        }

        public bool Greater(BI left, BI right)
        {
            return left.Value > right.Value;
        }

        public bool Lesser(BI left, BI right)
        {
            return left.Value < right.Value;
        }

        public BI Modulo(BI left, BI right)
        {
            return new BI(left.Value % right.Value);
        }

        public BI Remainder(BI left, BI right)
        {
            return new BI(BigInteger.Remainder(left.Value, right.Value));
        }

        public int ToInt32(BI val)
        {
            return (Int32)val.Value;
        }

        public long ToInt64(BI val)
        {
            return (Int64)val.Value;
        }

        public BI FromInt32(int val)
        {
            return new BI((BigInteger)val);
        }

        public BI FromInt64(long val)
        {
            return new BI((BigInteger)val);
        }
    }
}
