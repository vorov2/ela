using System;
using Ela.CodeModel;
using System.Collections.Generic;

namespace Ela.Compilation
{
    public sealed class ClassData
    {
        internal ClassData(ElaClassMember[] members)
        {
            Members = members;
            Code = -1;
        }

        public IEnumerable<ElaClassMember> GetMembers()
        {
            return Members;
        }

        internal ElaClassMember[] Members { get; private set; }

        internal int Code { get; set; }
    }
}
