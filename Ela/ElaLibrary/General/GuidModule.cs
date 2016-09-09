using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    public sealed class GuidModule : ForeignModule
    {
        public GuidModule()
        {

        }

        public override void Initialize()
        {
            Add("empty", new Wrapper<Guid>(Guid.Empty));
            Add<ElaUnit,Wrapper<Guid>>("new", NewGuid);
            Add<String,ElaObject>("parse", Parse);
            Add<Wrapper<Guid>,String>("toString", ToString);
            Add<Wrapper<Guid>,Wrapper<Guid>,Int32>("compare", Compare);
        }

        public Wrapper<Guid> NewGuid(ElaUnit _)
        {
            return new Wrapper<Guid>(Guid.NewGuid());
        }

        public ElaObject Parse(string str)
        {
            var g = Guid.Empty;

            if (Guid.TryParse(str, out g))
                return new Wrapper<Guid>(g);
            else
                return ElaUnit.Instance;
        }

        public string ToString(Wrapper<Guid> guid)
        {
            return guid.Value.ToString();
        }

        public int Compare(Wrapper<Guid> lho, Wrapper<Guid> rho)
        {
            return lho.Value.CompareTo(rho.Value);
        }
    }
}
