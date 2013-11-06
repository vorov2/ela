using System;

namespace Ela
{
    internal enum Api
    {
        None = 0,
        ConsName = 1,
        ConsParamNumber = 2,
        ConsIndex = 3,

        TypeName = 4,
        TypeCode = 5,
        TypeConsNumber = 6,
        ConsInfix = 7,
        IsAlgebraic = 8,
        ConsCode = 9,
        LazyList = 10,
        ListLength = 11,
        ReverseList = 12,
        ListToString = 13,
        ConsDefault = 14,
        ConsCreate = 15,

        CurrentContext = 16,
        Classes = 17,

        ConsParamIndex = 101,
        ConsParamValue = 102,
        ConsParamName = 103,
        ConsCodeByIndex = 104,
        ConsParamExist = 105,
        RecordField = 106,
        ConsNameIndex = 107,
    }
}
