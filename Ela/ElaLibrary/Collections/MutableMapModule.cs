using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class MutableMapModule : ForeignModule
    {
        #region Construction
        public MutableMapModule()
        {

        }
        #endregion


        #region Methods
        public override void Initialize()
        {
            Add<ElaMutableMap>("empty", CreateEmptyMap);
            Add<ElaRecord,ElaMutableMap>("map", CreateMap);
            Add<ElaValue,ElaMutableMap,Boolean>("contains", Contains);
            Add<ElaValue,ElaMutableMap,ElaVariant>("get", Get);
            Add<ElaMutableMap,ElaList>("keys", GetKeys);
            Add<ElaMutableMap,ElaList>("values", GetValues);
        }


        public ElaMutableMap CreateEmptyMap()
        {
            return new ElaMutableMap();
        }


        public ElaMutableMap CreateMap(ElaRecord rec)
        {
            var map = new ElaMutableMap();

            foreach (var k in rec.GetKeys())
                map.Map.Add(new ElaValue(k), rec[k]);

            return map;
        }


        public bool Contains(ElaValue key, ElaMutableMap map)
        {
            return map.Map.ContainsKey(key);
        }


        public ElaVariant Get(ElaValue key, ElaMutableMap map)
        {
            var val = default(ElaValue);

            if (!map.Map.TryGetValue(key, out val))
                return ElaVariant.None();

            return ElaVariant.Some(val);
        }


        public ElaList GetKeys(ElaMutableMap map)
        {
            return ElaList.FromEnumerable(map.Map.Keys);
        }


        public ElaList GetValues(ElaMutableMap map)
        {
            return ElaList.FromEnumerable(map.Map.Values);
        }
        #endregion
    }
}