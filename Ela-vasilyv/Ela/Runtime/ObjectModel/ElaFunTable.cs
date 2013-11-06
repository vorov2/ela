using System;
using System.Collections.Generic;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
    public sealed class ElaFunTable : ElaFunction
    {
        private Dictionary<Int32,ElaFunction> funs;
        private readonly string name;
        private readonly int mask;
        private readonly int curType;

        internal ElaFunTable(string name, int mask, int pars, int curType, ElaMachine vm) : base(pars)
        {
            this.name = name;
            this.mask = mask;
            this.curType = curType;
            base.table = true;
            base.Machine = vm;
            funs = new Dictionary<Int32,ElaFunction>();
        }

        internal ElaFunction GetFunction(ElaValue val, ExecutionContext ctx, int contextPar)
        {
            var m = 1 << AppliedParameters;
            var bit = (mask & m) == m;
            var ct = curType;
            
            if (bit && val.TypeId == ElaMachine.LAZ)
            {
                val = val.Ref.Force(val, ctx);

                if (ctx.Failed)
                    return null;
            }

            if (ct == 0)
            {
                if (bit)
                    ct = val.TypeId;
            }
            else
            {
                if (bit && val.TypeId != ct)
                {
                    if (funs.ContainsKey(ct))
                        ctx.NoOverload(ToStringWithType(ct, null, AppliedParameters), ToStringWithType(ct, val.TypeId, AppliedParameters), name);
                    else
                        ctx.NoOverload(val.GetTypeName(), name);
                    return null;
                }
            }

            if (AppliedParameters == Parameters.Length)
            {
                var ret = default(ElaFunction);

                if ((mask & (1 << Parameters.Length + 1)) == (1 << Parameters.Length + 1))
                {
                    if (ct == 0)
                    {
                        ct = contextPar;

                        if (ct == 0)
                            ctx.NoContext(name);
                    }
                    else if (ct != contextPar)
                    {
                        var dc = contextPar;

                        if (dc != 0)
                        {
                            if (funs.ContainsKey(ct))
                                ctx.NoOverload(ToStringWithType(ct, null, AppliedParameters), ToStringWithType(ct, dc, AppliedParameters + 1), name);
                            else
                                ctx.NoOverload(Machine.Assembly.Types[ct].TypeName, name);
                        }
                        else
                            ctx.NoContext(name);
                    }
                }

                if (!funs.TryGetValue(ct, out ret))
                {
                    if (funs.ContainsKey(ct))
                        ctx.NoOverload(ToStringWithType(ct, null, AppliedParameters), ToStringWithType(ct, val.TypeId, AppliedParameters), name);
                    else if (ct == 0)
                        ctx.NoOverload(val.GetTypeName(), name);
                    else
                        ctx.NoOverload(Machine.Assembly.Types[ct].TypeName, name);

                    return null;
                }

                ret = ret.CloneFast();
                ret.Flip = Flip;
                ret.AppliedParameters = AppliedParameters;
                
                if (ret.Parameters.Length != Parameters.Length)
                {
                    for (var i = 0; i < AppliedParameters; i++)
                        ret.Parameters[i] = Parameters[i];
                }
                else
                    ret.Parameters = Parameters;

                return ret;
            }

            var f = new ElaFunTable(name, mask, Parameters.Length + 1, ct, Machine);
            f.funs = funs;
            return base.CloneFast(f);
        }

        public override ElaValue Call(params ElaValue[] args)
        {
            if (args == null || args.Length == 0)
                throw new ElaException("Unable to call an overloaded function without arguments.");

            var ctx = new ExecutionContext();
            var fn = this;
            
            for (var i = 0; i < args.Length; i++)
            {
                var f = fn.GetFunction(args[i], ctx, 0);
                var nfn = f as ElaFunTable;

                if (nfn == null && i < args.Length - 1)
                    throw new ElaException("Incorrect argument number.");
                else if (nfn != null)
                {
                    nfn.AppliedParameters = fn.AppliedParameters + 1;
                    fn = nfn;
                }
                else
                {
                    if (ctx.Failed)
                        throw new ElaRuntimeException(ctx.Error);
                    
                    f.AppliedParameters = 0;
                    return Machine.Call(f, args);
                }

                if (ctx.Failed)
                    throw new ElaException(ctx.Error.Message);
            }

            return base.Default();
        }

        internal void AddFunction(int typeId, ElaFunction fun)
        {
            funs.Remove(typeId);
            funs.Add(typeId, fun);
        }

        public override ElaFunction Clone()
        {
            return this;
        }

        public override string ToString(string format, IFormatProvider provider)
        {
            var sb = new StringBuilder();

            for (var i = 0; i < Parameters.Length + 1; i++)
            {
                if (i > 0)
                    sb.Append("->");

                var m = (1 << i);

                if ((mask & m) == m)
                    sb.Append('a');
                else
                    sb.Append('*');
            }

            var mm = 1 << Parameters.Length + 1;

            if ((mask & mm) == mm)
                sb.Append("->a");
            else
                sb.Append("->*");

            return GetFunctionName() + ":" + sb.ToString();
        }

        private string ToStringWithType(int type, int? cur, int arg)
        {
            var sb = new StringBuilder();
            var tn = base.Machine.Assembly.Types[type].TypeName;
            var curn = cur != null ? base.Machine.Assembly.Types[cur.Value].TypeName : null;
            var len = cur == null ? Parameters.Length + 2 : 
                (AppliedParameters != Parameters.Length ? AppliedParameters + 1 : AppliedParameters + 2);

            for (var i = 0; i < len; i++)
            {
                if (i > 0)
                    sb.Append("->");

                var m = (1 << i);

                if (i == arg && curn != null)
                    sb.Append(curn);
                else if ((mask & m) == m)
                    sb.Append(tn);
                else
                    sb.Append('*');
            }

            if (cur != null && AppliedParameters != Parameters.Length)
                sb.Append("->...");

            return sb.ToString();
        }


        public override string GetFunctionName()
        {
            return name;
        }
    }
}
