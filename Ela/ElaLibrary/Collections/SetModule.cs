using System;
using System.Collections.Generic;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class SetModule : ForeignModule
    {
        #region Construction
        public SetModule()
        {

        }
        #endregion


        #region Methods
        public override void Initialize()
        {
            Add("empty", ElaSet.Empty);
            Add<IEnumerable<ElaValue>,ElaSet>("set", Create);
            Add<ElaValue,ElaSet,ElaSet>("add", Add);
            Add<ElaValue,ElaSet,ElaSet>("remove", Remove);
            Add<ElaValue,ElaSet,Boolean>("contains", Contains);
            Add<ElaSet,ElaList>("toList", ToList);
        }


        public ElaSet Create(IEnumerable<ElaValue> seq)
        {
			return ElaSet.FromEnumerable(seq);
        }


        public ElaSet Add(ElaValue value, ElaSet set)
        {
			return set.Add(value);
        }


        public ElaSet Remove(ElaValue value, ElaSet set)
        {
			return set.Remove(value);
        }


        public bool Contains(ElaValue value, ElaSet set)
        {
			return set.Contains(value);
        }


        public ElaList ToList(ElaSet set)
        {
            return ElaList.FromEnumerable(set);
        }
        #endregion
    }
}