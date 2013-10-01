using System;
using Ela.Runtime.ObjectModel;
using Ela.CodeModel;

namespace Ela.Runtime.Classes
{
    public class Class
    {
        private ElaFunction or;
        private ElaFunction and;
        private ElaFunction xor;
        private ElaFunction not;
        private ElaFunction shr;
        private ElaFunction shl;
        private ElaFunction cat;
        private ElaFunction eq;
        private ElaFunction neq;
        private ElaFunction len;
        private ElaFunction getval;
        private ElaFunction getfld;
        private ElaFunction hasfld;
        private ElaFunction add;
        private ElaFunction sub;
        private ElaFunction mul;
        private ElaFunction div;
        private ElaFunction rem;
        private ElaFunction mod;
        private ElaFunction pow;
        private ElaFunction neg;
        private ElaFunction gt;
        private ElaFunction lt;
        private ElaFunction gteq;
        private ElaFunction lteq;
        private ElaFunction head;
        private ElaFunction tail;
        private ElaFunction isnil;
        private ElaFunction show;
        
        internal Class()
        {

        }

        protected void NoOverloadBinary(string exp, ElaValue got, string fun, ExecutionContext ctx)
        {
            ctx.NoOverload(exp + "->" + exp + "->*", exp + "->" + got.GetTypeName() + "->*", fun);
        }

        internal virtual ElaValue BitwiseOr(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (or != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "bitwiseor", ctx);
                    return Default();
                }

                ctx.SetDeffered(or, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "bitwiseor");
            return Default();
        }

        internal virtual ElaValue BitwiseAnd(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (and != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "bitwiseand", ctx);
                    return Default();
                }

                ctx.SetDeffered(and, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "bitwiseand");
            return Default();
        }

        internal virtual ElaValue BitwiseXor(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (xor != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "bitwisexor", ctx);
                    return Default();
                }

                ctx.SetDeffered(xor, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "bitwisexor");
            return Default();
        }

        internal virtual ElaValue BitwiseNot(ElaValue @this, ExecutionContext ctx)
        {
            if (not != null)
            {
                ctx.SetDeffered(not, 1);
                return Default();
            }

            ctx.NoOverload(@this.GetTypeName(), "bitwisenot");
            return Default();
        }

        internal virtual ElaValue ShiftRight(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (shr != null)
            {
                ctx.SetDeffered(shr, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "shiftright");
            return Default();
        }

        internal virtual ElaValue ShiftLeft(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (shl != null)
            {
                ctx.SetDeffered(shl, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "shiftleft");
            return Default();
        }

        internal virtual ElaValue Concatenate(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (cat != null)
            {
                ctx.SetDeffered(cat, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "concatenate");
            return Default();
        }

        internal virtual bool Equal(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (eq != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "equal", ctx);
                    return false;
                }

                ctx.SetDeffered(eq, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "equal");
            return false;
        }

        internal virtual bool NotEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (neq != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "notequal", ctx);
                    return false;
                }

                ctx.SetDeffered(neq, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "notequal");
            return false;
        }

        internal virtual ElaValue GetLength(ElaValue value, ExecutionContext ctx)
        {
            if (len != null)
            {
                ctx.SetDeffered(len, 1);
                return Default();
            }

            ctx.NoOverload(value.GetTypeName(), "length");
            return Default();
        }

        internal virtual ElaValue GetValue(ElaValue value, ElaValue index, ExecutionContext ctx)
        {
            if (getval != null)
            {
                ctx.SetDeffered(getval, 2);
                return Default();
            }

            ctx.NoOverload(value.GetTypeName(), "getvalue");
            return Default();
        }

        internal virtual ElaValue GetField(ElaValue obj, ElaValue field, ExecutionContext ctx)
        {
            if (getfld != null)
            {
                ctx.SetDeffered(getfld, 2);
                return Default();
            }

            ctx.NoOverload(obj.GetTypeName(), "getfield");
            return Default();
        }

        internal virtual bool HasField(ElaValue obj, ElaValue field, ExecutionContext ctx)
        {
            if (hasfld != null)
            {
                ctx.SetDeffered(hasfld, 2);
                return false;
            }

            ctx.NoOverload(obj.GetTypeName(), "hasfield");
            return false;
        }

        internal virtual ElaValue Add(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (add != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "add", ctx);
                    return Default();
                }

                ctx.SetDeffered(add, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "add");
            return Default();
        }

        internal virtual ElaValue Subtract(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (sub != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "subtract", ctx);
                    return Default();
                }

                ctx.SetDeffered(sub, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "subtract");
            return Default();
        }

        internal virtual ElaValue Multiply(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (mul != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "multiply", ctx);
                    return Default();
                }

                ctx.SetDeffered(mul, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "multiply");
            return Default();
        }

        internal virtual ElaValue Divide(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (div != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "divide", ctx);
                    return Default();
                }

                ctx.SetDeffered(div, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "divide");
            return Default();
        }

        internal virtual ElaValue Remainder(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (rem != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "remainder", ctx);
                    return Default();
                }

                ctx.SetDeffered(rem, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "remainder");
            return Default();
        }

        internal virtual ElaValue Modulus(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (mod != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "modulus", ctx);
                    return Default();
                }

                ctx.SetDeffered(mod, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "modulus");
            return Default();
        }

        internal virtual ElaValue Power(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (pow != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "power", ctx);
                    return Default();
                }

                ctx.SetDeffered(pow, 2);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "power");
            return Default();
        }

        internal virtual ElaValue Negate(ElaValue @this, ExecutionContext ctx)
        {
            if (neg != null)
            {
                ctx.SetDeffered(neg, 1);
                return Default();
            }

            ctx.NoOverload(@this.GetTypeName(), "negate");
            return Default();
        }

        internal virtual bool Greater(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (gt != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "greater", ctx);
                    return false;
                }
                
                ctx.SetDeffered(gt, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "greater");
            return false;
        }

        internal virtual bool Lesser(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (lt != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "lesser", ctx);
                    return false;
                }

                ctx.SetDeffered(lt, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "lesser");
            return false;
        }

        internal virtual bool GreaterEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (gteq != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "greaterequal", ctx);
                    return false;
                }

                ctx.SetDeffered(gteq, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "greaterequal");
            return false;
        }

        internal virtual bool LesserEqual(ElaValue left, ElaValue right, ExecutionContext ctx)
        {
            if (lteq != null)
            {
                if (left.TypeId != right.TypeId)
                {
                    NoOverloadBinary(left.GetTypeName(), right, "lesserequal", ctx);
                    return false;
                }

                ctx.SetDeffered(lteq, 2);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "lesserequal");
            return false;
        }

        internal virtual ElaValue Head(ElaValue left, ExecutionContext ctx)
        {
            if (head != null)
            {
                ctx.SetDeffered(head, 1);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "head");
            return Default();
        }

        internal virtual ElaValue Tail(ElaValue left, ExecutionContext ctx)
        {
            if (tail != null)
            {
                ctx.SetDeffered(tail, 1);
                return Default();
            }

            ctx.NoOverload(left.GetTypeName(), "tail");
            return Default();
        }

        internal virtual bool IsNil(ElaValue left, ExecutionContext ctx)
        {
            if (isnil != null)
            {
                ctx.SetDeffered(isnil, 1);
                return false;
            }

            ctx.NoOverload(left.GetTypeName(), "isnil");
            return false;
        }

        internal virtual string Show(ElaValue value, ExecutionContext ctx)
        {
            if (show != null)
            {
                ctx.SetDeffered(show, 1);
                return String.Empty;
            }

            ctx.NoOverload(value.GetTypeName(), "showf");
            return String.Empty;
        }

        protected static ElaValue Default()
        {
            return new ElaValue(ElaObject.ElaInvalidObject.Instance);
        }

        internal void AddFunction(ElaBuiltinKind builtin, ElaFunction fun)
        {
            switch (builtin)
            {
                case ElaBuiltinKind.BitwiseAnd:
                    and = fun;
                    break;
                case ElaBuiltinKind.BitwiseOr:
                    or = fun;
                    break;
                case ElaBuiltinKind.BitwiseXor:
                    xor = fun;
                    break;
                case ElaBuiltinKind.BitwiseNot:
                    not = fun;
                    break;
                case ElaBuiltinKind.ShiftLeft:
                    shl = fun;
                    break;
                case ElaBuiltinKind.ShiftRight:
                    shr = fun;
                    break;
                case ElaBuiltinKind.Concat:
                    cat = fun;
                    break;
                case ElaBuiltinKind.Equal:
                    eq = fun;
                    break;
                case ElaBuiltinKind.NotEqual:
                    neq = fun;
                    break;
                case ElaBuiltinKind.Length:
                    len = fun;
                    break;
                case ElaBuiltinKind.GetValue:
                    getval = fun;
                    break;
                case ElaBuiltinKind.GetField:
                    getfld = fun;
                    break;
                case ElaBuiltinKind.Add:
                    add = fun;
                    break;
                case ElaBuiltinKind.Subtract:
                    sub = fun;
                    break;
                case ElaBuiltinKind.Multiply:
                    mul = fun;
                    break;
                case ElaBuiltinKind.Divide:
                    div = fun;
                    break;
                case ElaBuiltinKind.Power:
                    pow = fun;
                    break;
                case ElaBuiltinKind.Remainder:
                    rem = fun;
                    break;
                case ElaBuiltinKind.Negate:
                    neg = fun;
                    break;
                case ElaBuiltinKind.Greater:
                    gt = fun;
                    break;
                case ElaBuiltinKind.Lesser:
                    lt = fun;
                    break;
                case ElaBuiltinKind.LesserEqual:
                    lteq = fun;
                    break;
                case ElaBuiltinKind.GreaterEqual:
                    gteq = fun;
                    break;
                case ElaBuiltinKind.Head:
                    head = fun;
                    break;
                case ElaBuiltinKind.Tail:
                    tail = fun;
                    break;
                case ElaBuiltinKind.IsNil:
                    isnil = fun;
                    break;
                case ElaBuiltinKind.Show:
                    show = fun;
                    break;
                case ElaBuiltinKind.Modulus:
                    mod = fun;
                    break;
                case ElaBuiltinKind.HasField:
                    hasfld = fun;
                    break;
            }            
        }
    }
}
