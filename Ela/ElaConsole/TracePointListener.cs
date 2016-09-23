using Ela.Debug;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ElaConsole
{
    public sealed class TracePointListener : ITracePointListener
    {
        public void Trace(string tracePoint, LineSym line, IEnumerable<TraceVar> vars)
        {
            var name = String.Format("Trace point: {0} (Line: {1}; Column: {2}; Offset: {3})", tracePoint,
                line.Line, line.Column, line.Offset);
            var sep = new String('*', name.Length);

            var local = false;
            Console.WriteLine(sep);
            Console.WriteLine(name);
            Console.WriteLine(sep);

            foreach (var c in vars)
            {
                if (c.Local && !local)
                {
                    Console.WriteLine("Locals:");
                    local = true;
                }
                else if (!c.Local && local)
                {
                    Console.WriteLine();
                    Console.WriteLine("Captured:");
                    local = false;
                }

                Console.WriteLine("{0} = {1}", c.Name, ObtainValue(c.Value));
            }
            
            Console.WriteLine(sep);
        }

        private string ObtainValue(ElaValue value)
        {
            try
            {
                var result = String.Empty;
                var hdl = new AutoResetEvent(false);
                var th = new Thread(() =>
                    {
                        var obj = value.AsObject();

                        if (obj is ElaLazy)
                            result = ((ElaLazy)obj).Force().AsObject().ToString();
                        else if (obj is ElaLazyList)
                        {
                            var lalist = (ElaLazyList)obj;
                            result = ElaList.FromEnumerable(lalist.Take(20)).ToString() + " (lazy)";
                        }
                        else
                            result = value.ToString();

                        hdl.Set();
                    });
                th.Start();

                if (!hdl.WaitOne(500))
                {
                    th.Abort();
                    result = "<evaluation timeout>";
                }

                if (result == null)
                    return "_|_";
                else if (result.Trim().Length == 0)
                    result = "[" + value.GetTypeName() + "]";

                return result;
            }
            catch (Exception)
            {
                return "<evaluation error>";
            }
        }
    }
}
