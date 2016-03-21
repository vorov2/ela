using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    using RND = Ela.Library.Wrapper<Random>;

    public sealed class CoreModule : ForeignModule
    {
        public CoreModule()
        {

        }
        
        public override void Initialize()
        {
            
            Add<Int32,Int32,Int32,Int32>("rnd", Rnd);
            Add<Int32,RND>("createRnd", CreateRnd);
            Add<RND,Int32,Int32,Int32>("getRnd", GetRnd);
            Add<ElaValue,Boolean>("evaled", IsEvaled);
        }

        public bool IsEvaled(ElaValue obj)
        {
            return !obj.Is<ElaLazy>() || obj.As<ElaLazy>().Evaled;
        }
                
        public int Rnd(int seed, int min, int max)
        {
            var rnd = new Random(seed);
            return rnd.Next(min, max);
        }

        public RND CreateRnd(int seed)
        {
            return new RND(new Random(seed));
        }

        public int GetRnd(RND rnd, int min, int max)
        {
            return rnd.Value.Next(min, max);
        }
    }
}
