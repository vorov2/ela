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
    public sealed class ComplexModule : ForeignModule
    {
        internal static readonly CultureInfo Culture = CultureInfo.InvariantCulture;

        public ComplexModule()
        {

        }

        public override void Initialize()
        {
            Add<Double,Double,Double>("magnitude", Magnitude);
            Add<Double,Double,Double>("phase", Phase);
            Add<Double,Double,Double,Double,ElaTuple>("divide", Divide);
        }

        public double Magnitude(double a, double aa)
        {
            var ret = new Complex(a, aa).Magnitude;
            return ret;
        }

        public double Phase(double a, double aa)
        {
            var ret = new Complex(a, aa).Phase;
            return ret;
        }

        public ElaTuple Divide(double a, double aa, double b, double bb)
        {
            var x = new Complex(a, aa);
            var y = new Complex(b, bb);
            var ret = Complex.Divide(x, y);
            return new ElaTuple(ret.Real, ret.Imaginary);
        }
    }
}
