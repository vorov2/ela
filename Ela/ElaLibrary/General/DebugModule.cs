using System;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using Ela.Runtime;
using System.Diagnostics;
using System.Threading;

namespace Ela.Library.General
{
	public sealed class DebugModule : ForeignModule
	{
		#region Construction
		public DebugModule()
		{

		}
		#endregion

        sealed class Fun2 : ElaFunction
        {
            public Fun2() : base() { }
            public override ElaValue Call(params ElaValue[] args)
            {
                return new ElaValue(ElaUnit.Instance);
            }
        }
		
		#region Methods
		public override void Initialize()
		{
			Add<ElaObject>("startClock", StartClock);
            Add<Wrapper<Stopwatch>,Double>("stopClock", StopClock);
			Add<Int32,ElaUnit>("sleep", Sleep);
            Add<ElaUnit,ElaUnit>("fun1", Fun1);
            Add("fun2",new Fun2());
            Add<Int32,Int32,Int32,ElaList>("enumFromTo2", EnumFromTo);
        }

        public ElaUnit Fun1(ElaUnit unit)
        {
            return ElaUnit.Instance;
        }
        
		public ElaObject StartClock()
		{
            var t = new Stopwatch();
            t.Start();
            return new Wrapper<Stopwatch>(t);
		}


		public double StopClock(Wrapper<Stopwatch> val)
		{
            val.Value.Stop();
            return val.Value.Elapsed.TotalMilliseconds;
		}


		public ElaUnit Sleep(int ms)
		{
			Thread.Sleep(ms);
			return ElaUnit.Instance;
		}


        public ElaList EnumFromTo(int max, int fst, int snd)
        {
            var sw = new Stopwatch();
            sw.Start();

            var e = max;
            var xs = ElaList.Empty;
            snd = snd - fst;

            for (;;)
            {
                if (e < fst)
                    break;

                xs = new ElaList(xs, new ElaValue(e));
                e = e-snd;
            }

            var ret = xs;//.Reverse();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);
            return ret;
        }
		#endregion
	}
}
