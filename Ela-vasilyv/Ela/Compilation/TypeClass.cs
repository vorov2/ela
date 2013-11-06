using System;

namespace Ela.Compilation
{
    //This enumerate contains a list of built-in type classes
    //(that have internal implementation). It is used to give built-in
    //classes unique IDs (which in their turn are by compiler to recognize
    //these classes).
    internal enum TypeClass
    {
        None = -1,
        Eq = 1,
        Ord = 2,
        Additive = 3,
        Ring = 4,
        Field = 5,
        Modulo = 6,
        Bit = 7,
        Seq = 8,
        Ix = 9,
        Len = 10,
        Name = 11,
        Cat = 12,
        Show = 13
    }

    //Convert a string to TypeClass enum.
    internal static class TypeClassHelper
    {
        public static TypeClass GetTypeClass(string val)
        {
            switch (val)
            {
                case "Eq": return TypeClass.Eq;
                case "Ord": return TypeClass.Ord;
                case "Additive": return TypeClass.Additive;
                case "Ring": return TypeClass.Ring;
                case "Field": return TypeClass.Field;
                case "Modulo": return TypeClass.Modulo;
                case "Bit": return TypeClass.Bit;
                case "Seq": return TypeClass.Seq;
                case "Ix": return TypeClass.Ix;
                case "Len": return TypeClass.Len;
                case "Name": return TypeClass.Name;
                case "Cat": return TypeClass.Cat;
                case "Show": return TypeClass.Show;
                default: return TypeClass.None;
            }
        }
    }
}
