using Ela.Debug;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.ElaCode.Views
{
    public sealed class TraceItem
    {
        public TraceItem(string name, LineSym lineInfo, TraceVar var)
        {
            TracePointName = name;
            LineInfo = lineInfo;
            Var = var;
        }

        public string TracePointName { get; private set; }

        public LineSym LineInfo { get; private set; }

        public TraceVar Var { get; private set; }
    }
}
