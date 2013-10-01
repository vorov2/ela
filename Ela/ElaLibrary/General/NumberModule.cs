using System;
using Ela.Linking;
using Ela.Runtime;

namespace Ela.Library.General
{
    public sealed class NumberModule : ForeignModule
    {
        public NumberModule()
        {

        }
        
        public override void Initialize()
        {
            Add<Single,Boolean>("infSingle", IsInfinitySingle);
            Add<Double,Boolean>("infDouble", IsInfinityDouble);
            Add<Single,Boolean>("posInfSingle", IsPositiveInfinitySingle);
            Add<Double,Boolean>("posInfDouble", IsPositiveInfinityDouble);
            Add<Single,Boolean>("negInfSingle", IsNegativeInfinitySingle);
            Add<Double,Boolean>("negInfDouble", IsNegativeInfinityDouble);
            Add<Single,Boolean>("nanSingle", IsNanSingle);
            Add<Double,Boolean>("nanDouble", IsNanDouble);
            Add<Double,Double>("floor", Floor);
            Add<Double,Double>("ceiling", Ceiling);
            Add<Double,Double>("round", Round);
            Add<Double,Int64>("truncate", Truncate);

            Add<Double,Double>("exp", Exp);
            Add<Double,Double>("cos", Cos);
            Add<Double,Double>("sin", Sin);
            Add<Double,Double>("log", Log);
            Add<Double,Double,Double>("logBase", LogBase);
            Add<Double,Double>("tan", Tan);
            Add<Double,Double>("acos", Acos);
            Add<Double,Double>("asin", Asin);
            Add<Double,Double>("atan", Atan);
            Add<Double,Double>("cosh", Cosh);
            Add<Double,Double>("sinh", Sinh);
            Add<Double,Double>("tanh", Tanh);
            Add<Double,Double>("acosh", Acosh);
            Add<Double,Double>("asinh", Asinh);
            Add<Double,Double>("atanh", Atanh);
        }

        public double Exp(double val)
        {
            return Math.Exp(val);
        }

        public double Cos(double val)
        {
            return Math.Cos(val);
        }

        public double Sin(double val)
        {
            return Math.Sin(val);
        }

        public double Tan(double val)
        {
            return Math.Tan(val);
        }

        public double Log(double x)
        {
            return Math.Log(x);
        }

        public double LogBase(double x, double y)
        {
            return Math.Log(x, y);
        }

        public double Acos(double val)
        {
            return Math.Acos(val);
        }

        public double Asin(double val)
        {
            return Math.Asin(val);
        }

        public double Atan(double val)
        {
            return Math.Atan(val);
        }

        public double Cosh(double val)
        {
            return Math.Cosh(val);
        }

        public double Sinh(double val)
        {
            return Math.Sinh(val);
        }

        public double Tanh(double val)
        {
            return Math.Tanh(val);
        }

        public double Acosh(double x)
        {
            if (x < 1.0) 
                throw new ElaException("Invalid range.");

            return Math.Log(x + Math.Sqrt(x * x - 1));
        }

        public double Asinh(double xx)
        {
            double x;
            int sign;
            
            if (xx == 0.0) 
                return xx;
            
            if (xx < 0.0)
            {
                sign = -1;
                x = -xx;
            }
            else
            {
                sign = 1;
                x = xx;
            }
            
            return sign * Math.Log(x + Math.Sqrt(x * x + 1));
        }

        public double Atanh(double x)
        {
            if (x > 1.0 || x < -1.0)
                throw new ElaException("Invalid range.");

            return 0.5 * Math.Log((1.0 + x) / (1.0 - x));
        }
        
        public double Round(double x)
        {
            return Math.Round(x);
        }

        public long Truncate(double x)
        {
            return (Int64)Math.Truncate(x);
        }

        public double Floor(double val)
        {
            return Math.Floor(val);
        }

        public double Ceiling(double val)
        {
            return Math.Ceiling(val);
        }

        public bool IsInfinitySingle(float val)
        {
            return Single.IsInfinity(val);
        }

        public bool IsInfinityDouble(double val)
        {
            return Double.IsInfinity(val);
        }

        public bool IsNegativeInfinitySingle(float val)
        {
            return Single.IsNegativeInfinity(val);
        }

        public bool IsNegativeInfinityDouble(double val)
        {
            return Double.IsNegativeInfinity(val);
        }

        public bool IsPositiveInfinitySingle(float val)
        {
            return Single.IsPositiveInfinity(val);
        }

        public bool IsPositiveInfinityDouble(double val)
        {
            return Double.IsPositiveInfinity(val);
        }
        
        public bool IsNanSingle(float val)
        {
            return Single.IsNaN(val);
        }

        public bool IsNanDouble(double val)
        {
            return Double.IsNaN(val);
        }
    }
}
