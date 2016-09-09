using Ela.Runtime.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.Runtime.ObjectModel
{
    public sealed class ElaFailure : ElaLazy
    {
        public ElaFailure(string reason) : this(new ElaValue(reason))
        {

        }

        internal ElaFailure(ElaValue reason) : base(null)
        {
            base.Value = reason;
        }

        internal override ElaValue Force()
        {
            throw new Exception(Value.ToString());
        }

        internal override ElaValue Force(ElaValue @this, ExecutionContext ctx)
        {
            if (ctx == ElaObject.DummyContext)
                throw new Exception(Value.ToString());

            ctx.Fail(Value.ToString());
            return new ElaValue(ElaUnit.Instance);
        }
    }
}
