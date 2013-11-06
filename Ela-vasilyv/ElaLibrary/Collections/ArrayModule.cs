using System;
using System.Collections.Generic;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.Collections
{
    public sealed class ArrayModule : ForeignModule
    {
        #region Construction
        public ArrayModule()
        {

        }
        #endregion


        #region Methods
        public override void Initialize()
        {
			Add<ElaArray>("empty", CreateEmptyArray);
			Add<IEnumerable<ElaValue>,ElaArray>("array", CreateArray);
            Add<ElaValue,ElaArray,ElaUnit>("add", Add);
            Add<Int32,ElaArray,Boolean>("removeAt", RemoveAt);
            Add<Int32,ElaValue,ElaArray,Boolean>("insert", Insert);
            Add<ElaArray,ElaUnit>("clear", Clear);
            Add<Int32,ElaArray,ElaVariant>("get", Get);
        }


		public ElaArray CreateEmptyArray()
		{
			return new ElaArray();
		}


        public ElaArray CreateArray(IEnumerable<ElaValue> seq)
        {
            var lst = new List<ElaValue>();

            foreach (var v in seq)
                lst.Add(v);

            return new ElaArray(lst.ToArray());
        }


        public ElaUnit Add(ElaValue value, ElaArray arr)
        {
            //arr.Add(value);
            return ElaUnit.Instance;
        }


        public bool RemoveAt(int index, ElaArray arr)
        {
            return arr.Remove(index);
        }


        public bool Insert(int index, ElaValue value, ElaArray arr)
        {
            return arr.Insert(index, value);
        }


        public ElaUnit Clear(ElaArray arr)
        {
            arr.Clear();
            return ElaUnit.Instance;
        }


        public ElaVariant Get(int index, ElaArray arr)
        {
            if (index < 0 || index >= arr.Length)
                return ElaVariant.None();

            return ElaVariant.Some(arr.FastGet(index));
        }
        #endregion
    }
}