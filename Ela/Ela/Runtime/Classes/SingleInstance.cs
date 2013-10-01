using System;
using System.Globalization;
using Ela.Runtime.ObjectModel;

namespace Ela.Runtime.Classes
{
    internal sealed class SingleInstance : Class
    {
        #region Show
        internal override string Show(ElaValue value, ExecutionContext ctx)
        {
            var d = value.DirectGetSingle();

            if (Single.IsInfinity(d) || Single.IsNaN(d))
                return value.ToString();
            else
                return value.ToString() + "f";
        }
        #endregion

        #region Eq Ord
        internal override bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() == right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() == right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() == right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "equal", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() == right.DirectGetSingle();
        }

        internal override bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() != right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() != right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() != right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "notequal", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() != right.DirectGetSingle();
        }

        internal override bool Greater(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() > right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() > right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() > right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "greater", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() > right.DirectGetSingle();
        }

        internal override bool Lesser(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() < right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() < right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() < right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "lesser", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() < right.DirectGetSingle();
        }

        internal override bool GreaterEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() >= right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() >= right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() >= right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "greaterequal", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() >= right.DirectGetSingle();
        }

        internal override bool LesserEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return left.DirectGetSingle() <= right.Ref.AsDouble();
                else if (right.TypeId == ElaMachine.INT)
                    return left.DirectGetSingle() <= right.I4;
                else if (right.TypeId == ElaMachine.LNG)
                    return left.DirectGetSingle() <= right.Ref.AsLong();
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "lesserequal", ctx);
                    return false;
                }
            }

            return left.DirectGetSingle() <= right.DirectGetSingle();
        }
        #endregion

        #region Num
        internal override ElaValue Add(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.DirectGetSingle() + right.Ref.AsDouble());
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.DirectGetSingle() + right.I4);
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue(left.DirectGetSingle() + right.Ref.AsLong());
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "add", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.DirectGetSingle() + right.DirectGetSingle());
        }

        internal override ElaValue Subtract(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.DirectGetSingle() - right.Ref.AsDouble());
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.DirectGetSingle() - right.I4);
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue(left.DirectGetSingle() - right.Ref.AsLong());
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "subtract", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.DirectGetSingle() - right.DirectGetSingle());
        }

        internal override ElaValue Multiply(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.DirectGetSingle() * right.Ref.AsDouble());
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.DirectGetSingle() * right.I4);
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue(left.DirectGetSingle() * right.Ref.AsLong());
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "multiply", ctx);
                    return Default();
                }
            }

            return new ElaValue(left.DirectGetSingle() * right.DirectGetSingle());
        }

        internal override ElaValue Divide(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.DirectGetSingle() / right.Ref.AsDouble());
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.DirectGetSingle() / right.I4);
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue(left.DirectGetSingle() / right.Ref.AsLong());
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "divide", ctx);
                    return Default();
                }
            }

            if (right.DirectGetSingle() == 0)
            {
                ctx.DivideByZero(left);
                return Default();
            }

            return new ElaValue(left.DirectGetSingle() / right.DirectGetSingle());
        }
                
        internal override ElaValue Remainder(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(left.DirectGetSingle() % right.Ref.AsDouble());
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue(left.DirectGetSingle() % right.I4);
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue(left.DirectGetSingle() % right.Ref.AsLong());
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "remainder", ctx);
                    return Default();
                }
            }

            if (right.DirectGetSingle() == 0)
            {
                ctx.DivideByZero(left);
                return Default();
            }

            return new ElaValue(left.DirectGetSingle() % right.DirectGetSingle());
        }

        internal static ElaValue Modulus(float x, float y, ExecutionContext ctx)
        {
            var r = x % y;
            return x < 0 && y > 0 || x > 0 && y < 0 ? new ElaValue(r + y) : new ElaValue(r);
        }

        internal override ElaValue Modulus(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return DoubleInstance.Modulus(left.DirectGetSingle(), right.Ref.AsDouble(), ctx);
                else if (right.TypeId == ElaMachine.INT)
                    return Modulus(left.DirectGetSingle(), right.I4, ctx);
                else if (right.TypeId == ElaMachine.LNG)
                    return Modulus(left.DirectGetSingle(), right.Ref.AsLong(), ctx);
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "modulus", ctx);
                    return Default();
                }
            }

            return Modulus(left.DirectGetSingle(), right.DirectGetSingle(), ctx);
        }
        
        internal override ElaValue Power(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (right.TypeId != ElaMachine.REA)
            {
                if (right.TypeId == ElaMachine.DBL)
                    return new ElaValue(Math.Pow(left.DirectGetSingle(), right.Ref.AsDouble()));
                else if (right.TypeId == ElaMachine.INT)
                    return new ElaValue((Single)Math.Pow(left.DirectGetSingle(), right.I4));
                else if (right.TypeId == ElaMachine.LNG)
                    return new ElaValue((Single)Math.Pow(left.DirectGetSingle(), right.Ref.AsLong()));
                else
                {
                    NoOverloadBinary(TCF.SINGLE, right, "power", ctx);
                    return Default();
                }
            }

            return new ElaValue(Math.Pow(left.DirectGetSingle(), right.DirectGetSingle()));
        }
        
        internal override ElaValue Negate(ElaValue value, ExecutionContext ctx)
        {
            return new ElaValue(-value.DirectGetSingle());
        }
        #endregion
    }
}
