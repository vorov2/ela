using System;
using System.Collections.Generic;

namespace Ela.Runtime.ObjectModel
{
    internal sealed class ElaConstant : ElaObject
    {
        private Dictionary<Int32,ElaValue> consts;
        private readonly string name;

        internal ElaConstant(string name)
        {
            consts = new Dictionary<Int32,ElaValue>();
            this.name = name;
        }

        internal ElaValue GetConstantValue(ElaMachine vm, ExecutionContext ctx, int d)
        {
            if (d == 0)
            {
                ctx.NoContext(name);
                return Default();
            }

            ElaValue val;

            if (!consts.TryGetValue(d, out val))
            {
                ctx.NoOverload(vm.Assembly.Types[d].TypeName, name);
                return Default();
            }

            return val;
        }

        internal void AddConstant(int type, ElaValue right)
        {
            consts.Remove(type);
            consts.Add(type, right);
        }
    }
}
