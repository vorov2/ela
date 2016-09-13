using Ela.Linking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Numerics;
using M = Ela.Library.Wrapper<System.Decimal>;
using Ela.Runtime.ObjectModel;
using System.Globalization;

namespace Ela.Library.General
{
    public sealed class MoneyModule : ForeignModule
    {
        internal static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public MoneyModule()
        {

        }

        public override void Initialize()
        {
            Add<M, M, M>("add", Sum);
            Add<M, M, M>("subtract", Subtract);
            Add<M, M, M>("multiply", Multiply);
            Add<M, M, M>("divide", Divide);
            Add<M, M>("negate", Negate);
            Add<M, M, M>("pow", Power);
            Add<String, M>("parse", Parse);
            Add<M, M, Boolean>("equals", Equals);
            Add<M, String, String>("show", Show);
            Add<M, M, Boolean>("greater", Greater);
            Add<M, M, Boolean>("lesser", Lesser);
            Add<M, M, M>("mod", Modulo);
            Add<M, M, M>("rem", Remainder);
            Add<M, Double>("toDouble", ToDouble);
            Add<M, Single>("toSingle", ToSingle);
            Add<Double, M>("fromDouble", FromDouble);
            Add<Single, M>("fromSingle", FromSingle);
            Add<M, M>("truncate", Truncate);
            Add<M, M>("ceiling", Ceiling);
            Add<M, M>("floor", Floor);
            Add<M, M>("round", Round);
        }

        public M Sum(M left, M right)
        {
            return new M(left.Value + right.Value);
        }

        public M Subtract(M left, M right)
        {
            return new M(left.Value - right.Value);
        }

        public M Multiply(M left, M right)
        {
            return new M(left.Value * right.Value);
        }

        public M Divide(M left, M right)
        {
            return new M(left.Value / right.Value);
        }

        public M Negate(M val)
        {
            return new M(-val.Value);
        }

        public M Power(M left, M right)
        {
            return new M((Decimal)Math.Pow((Double)left.Value, (Double)right.Value));
        }

        public M Parse(string str)
        {
            var bi = default(Decimal);

            if (!Decimal.TryParse(str, NumberStyles.Number, Culture.NumberFormat, out bi))
                return new M(-1m);

            return new M(bi);
        }

        public bool Equals(M left, M right)
        {
            return left.Value == right.Value;
        }

        public string Show(M val, string format)
        {
            return val.Value.ToString(format, Culture.NumberFormat);
        }

        public bool Greater(M left, M right)
        {
            return left.Value > right.Value;
        }

        public bool Lesser(M left, M right)
        {
            return left.Value < right.Value;
        }

        public M Modulo(M left, M right)
        {
            return new M(left.Value % right.Value);
        }

        public M Remainder(M left, M right)
        {
            return new M(Decimal.Remainder(left.Value, right.Value));
        }

        public M Truncate(M left)
        {
            return new M(Decimal.Truncate(left.Value));
        }

        public M Ceiling(M left)
        {
            return new M(Decimal.Ceiling(left.Value));
        }

        public M Floor(M left)
        {
            return new M(Decimal.Floor(left.Value));
        }

        public M Round(M left)
        {
            return new M(Decimal.Round(left.Value, MidpointRounding.AwayFromZero));
        }

        public double ToDouble(M val)
        {
            return (Double)val.Value;
        }

        public float ToSingle(M val)
        {
            return (Single)val.Value;
        }

        public M FromDouble(double val)
        {
            return new M((Decimal)val);
        }

        public M FromSingle(float val)
        {
            return new M((Decimal)val);
        }
    }
}
