using System;
using System.Globalization;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class LongInstance : Class
    {
        #region Show
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            return value.ToString() + "L";
        }
        #endregion

        #region Bit
        internal override ElaValue BitwiseAnd(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.Ref.AsLong() & right.I4);
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "bitwiseand", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() & right.Ref.AsLong());
        }

        internal override ElaValue BitwiseOr(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                {
                    var lo = (Int64)right.I4;
                    return new ElaValue(left.Ref.AsLong() | lo);
                }
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "bitwiseor", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() | right.Ref.AsLong());
        }

        internal override ElaValue BitwiseXor(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.Ref.AsLong() ^ right.I4);
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "bitwisexor", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() ^ right.Ref.AsLong());
        }

        internal override ElaValue ShiftLeft(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.INT)
            {
                NoOverloadBinary(TCF.LONG, right, "shiftleft", ctx);
                return Default();
            }

            return new ElaValue(left.Ref.AsLong() << right.I4);
        }

        internal override ElaValue ShiftRight(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.INT)
            {
                NoOverloadBinary(TCF.LONG, right, "shiftright", ctx);
                return Default();
            }

            return new ElaValue(left.Ref.AsLong() >> right.I4);
        }
        #endregion

        #region Eq Ord
        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() == right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() == right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() == right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "equal", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() == right.Ref.AsLong();
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() != right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() != right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() != right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "notequal", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() != right.Ref.AsLong();
        }

        internal override bool Greater(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() > right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() > right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() > right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "greater", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() > right.Ref.AsLong();
        }

        internal override bool Lesser(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() < right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() < right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() < right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "lesser", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() < right.Ref.AsLong();
        }

        internal override bool GreaterEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() >= right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() >= right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() >= right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "greaterequal", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() >= right.Ref.AsLong();
        }

        internal override bool LesserEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return left.Ref.AsLong() <= right.I4;
                else if (right.TypeId == ElaMachine.REA)
                    return left.Ref.AsLong() <= right.DirectGetSingle();
                else if (right.TypeId == ElaMachine.DBL)
                    return left.Ref.AsLong() <= right.Ref.AsDouble();
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "lesserequal", ctx);
                    return false;
                }
            }

            return left.Ref.AsLong() <= right.Ref.AsLong();
        }
        #endregion

        #region Num
        internal override ElaValue Add(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.Ref.AsLong() + right.I4);
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue(left.Ref.AsLong() + right.DirectGetSingle());
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.Ref.AsLong() + right.Ref.AsDouble());
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "add", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() + right.Ref.AsLong());
        }

        internal override ElaValue Subtract(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.Ref.AsLong() - right.I4);
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue(left.Ref.AsLong() - right.DirectGetSingle());
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.Ref.AsLong() - right.Ref.AsDouble());
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "subtract", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() - right.Ref.AsLong());
        }

        internal override ElaValue Multiply(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.Ref.AsLong() + right.I4);
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue(left.Ref.AsLong() + right.DirectGetSingle());
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.Ref.AsLong() + right.Ref.AsDouble());
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "multiply", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.Ref.AsLong() * right.Ref.AsLong());
        }

        internal override ElaValue Remainder(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                {
                    if (right.I4 == 0)
                    {
                        ctx.DivideByZero(left);
                        return Default();
                    }

                    return new ElaValue(left.Ref.AsLong() % right.I4);
                }
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue(left.Ref.AsLong() % right.DirectGetSingle());
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.Ref.AsLong() % right.Ref.AsDouble());
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "remainder", ctx);
                    return Default();
                }
            }

            var rl = right.Ref.AsLong();

            if (rl == 0)
            {
                ctx.DivideByZero(left);
                return Default();
            }

            return new ElaValue(left.Ref.AsLong() % rl);
        }

        internal override ElaValue Divide(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                {
                    return new ElaValue((Double)left.Ref.AsLong() / right.I4);
                }
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue(left.Ref.AsLong() / right.DirectGetSingle());
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.Ref.AsLong() / right.Ref.AsDouble());
                else
                {
                    NoOverloadBinary(TCF.INT, right, "divide", ctx);
                    return Default();
                }
            }

            var r = right.Ref.AsLong();
            return new ElaValue((Double)left.Ref.AsLong() / r);
        }

        internal static ElaValue Modulus(long x, long y, ExecutionContext ctx)
        {
            if (y == 0)
            {
                ctx.DivideByZero(new ElaValue(x));
                return Default();
            }

            var r = x % y;
            return x < 0 && y > 0 || x > 0 && y < 0 ? new ElaValue(r + y) : new ElaValue(r);
        }

        internal override ElaValue Modulus(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return Modulus(left.Ref.AsLong(), right.I4, ctx);
                else if (right.TypeId == ElaMachine.REA)
                    return SingleInstance.Modulus(left.Ref.AsLong(), right.DirectGetSingle(), ctx);
                else if (right.TypeId == ElaMachine.DBL)
                    return DoubleInstance.Modulus(left.Ref.AsLong(), right.Ref.AsDouble(), ctx);
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "modulus", ctx);
                    return Default();
                }
            }

            return Modulus(left.Ref.AsLong(), right.Ref.AsLong(), ctx);
        }
        
        internal override ElaValue Power(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.LNG)
            {
                if (right.TypeId == ElaMachine.INT)
                    return new ElaValue((Int64)Math.Pow(left.Ref.AsLong(), right.I4));
                else if (right.TypeId == ElaMachine.REA)
                    return new ElaValue((Single)Math.Pow(left.Ref.AsLong(), right.DirectGetSingle()));
                else if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue((Double)Math.Pow(left.Ref.AsLong(), right.Ref.AsDouble()));
                else
                {
                    NoOverloadBinary(TCF.LONG, right, "power", ctx);
                    return Default();
                }
            }

            return new ElaValue((Int64)Math.Pow(left.Ref.AsLong(), right.Ref.AsLong()));
        }

        internal override ElaValue Negate(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(-value.Ref.AsLong());
        }
        #endregion
    }
}
