using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    public sealed class GuidModule : ForeignModule
    {
        #region Construction
        private static readonly ElaGuid emptyGuid = new ElaGuid(Guid.Empty);

        public GuidModule()
        {

        }
        #endregion


        #region Methods
        public override void Initialize()
        {
            Add("empty", emptyGuid);
            Add<ElaGuid>("guid", NewGuid);
            Add<String,ElaVariant>("parse", Parse);
        }


        public ElaGuid NewGuid()
        {
            return new ElaGuid(Guid.NewGuid());
        }


        public ElaVariant Parse(string str)
        {
            var g = Guid.Empty;

            try
            {
                g = new Guid(str);
                return ElaVariant.Some(g);
            }
            catch (Exception)
            {
                return ElaVariant.None();
            }
        }
        #endregion
    }
}
