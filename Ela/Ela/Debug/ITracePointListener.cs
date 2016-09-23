using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ela.Debug
{
    public interface ITracePointListener
    {
        void Trace(string tracePoint, LineSym line, IEnumerable<TraceVar> vars);
    }
}
