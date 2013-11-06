using System;
using System.Collections.Generic;

namespace Ela.Linking
{
    internal sealed class TypeData
    {
        public TypeData(ElaTypeCode typeCode) : this((Int32)typeCode, TCF.GetShortForm(typeCode))
        {
            
        }

        public TypeData(int typeCode, string typeName)
        {
            TypeName = typeName;
            TypeCode = typeCode;
            Constructors = new List<Int32>();
        }

        public List<Int32> Constructors { get; private set; }

        public int TypeCode { get; private set; }

        public string TypeName { get; private set; }
    }
}
