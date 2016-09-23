using Ela.Debug;
using Elide.Core;
using Elide.ElaCode.Views;
using Elide.Environment.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.ElaCode
{
    internal sealed class ElaTraceListener : ITracePointListener
    {
        private readonly IApp app;

        internal ElaTraceListener(IApp app)
        {
            this.app = app;
        }

        public void Trace(string tracePoint, LineSym line, IEnumerable<TraceVar> vars)
        {
            var grid = (DebugView)app.GetService<IViewService>().GetView("Debug");
            grid.AddItems(vars.Select(v => new TraceItem(tracePoint, line, v)));
        }
    }
}
