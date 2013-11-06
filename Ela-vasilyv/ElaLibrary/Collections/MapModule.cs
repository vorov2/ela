using System;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
	public sealed class MapModule : ForeignModule
	{
		#region Construction
		public MapModule()
		{

		}
		#endregion


		#region Methods
		public override void Initialize()
		{
			Add("empty", ElaMap.Empty);
			Add<ElaRecord,ElaMap>("map", CreateMap);
			Add<ElaValue,ElaValue,ElaMap,ElaMap>("add", Add);
			Add<ElaValue,ElaMap,ElaMap>("remove", Remove);
			Add<ElaValue,ElaMap,Boolean>("contains", Contains);
			Add<ElaValue,ElaMap,ElaVariant>("get", GetValue);
		}


		public ElaMap CreateMap(ElaRecord rec)
		{
			var map = ElaMap.Empty;

			foreach (var k in rec.GetKeys())
				map = new ElaMap(map.Tree.Add(new ElaValue(k), rec[k]));

			return map;
		}


		public ElaMap Add(ElaValue key, ElaValue value, ElaMap map)
		{
			return map.Add(key, value);
		}


		public ElaMap Remove(ElaValue key, ElaMap map)
		{
			return map.Remove(key);
		}


		public bool Contains(ElaValue key, ElaMap map)
		{
			return map.Contains(key);
		}


		public ElaVariant GetValue(ElaValue key, ElaMap map)
		{
			if (map.Tree.IsEmpty)
				return ElaVariant.None();

			var res = map.Tree.Search(key);
			return res.IsEmpty ? ElaVariant.None() : ElaVariant.Some(res.Value);
		}
		#endregion
	}
}