using System;

namespace Ela
{
	public static class TCF
	{
		internal const string ERR = "?";
		internal const string CHAR = "Char";
		internal const string INT = "Int";
		internal const string LONG = "Long";
		internal const string SINGLE = "Single";
		internal const string DOUBLE = "Double";
		internal const string STRING = "String";
		internal const string BOOL = "Bool";
		internal const string RECORD = "Record";
		internal const string TUPLE = "Tuple";
		internal const string LIST = "List";
		internal const string FUN = "Fun";
		internal const string UNIT = "Unit";
		internal const string MOD = "Module";
		internal const string OBJ = "Object";
		internal const string LAZ = "Thunk";
		
        public static ElaTypeCode GetTypeCode(string type)
        {
            switch (type)
            {
                case CHAR: return ElaTypeCode.Char;
                case INT: return ElaTypeCode.Integer;
                case LONG: return ElaTypeCode.Long;
                case SINGLE: return ElaTypeCode.Single;
                case DOUBLE: return ElaTypeCode.Double;
                case BOOL: return ElaTypeCode.Boolean;
                case STRING: return ElaTypeCode.String;
                case LIST: return ElaTypeCode.List;
                case TUPLE: return ElaTypeCode.Tuple;
                case RECORD: return ElaTypeCode.Record;
                case FUN: return ElaTypeCode.Function;
                case UNIT: return ElaTypeCode.Unit;
                case MOD: return ElaTypeCode.Module;
                case OBJ: return ElaTypeCode.Object;
                case LAZ: return ElaTypeCode.Lazy;
                default: return ElaTypeCode.None;
            }
        }

		public static string GetShortForm(ElaTypeCode @this)
		{
			switch (@this)
			{
				case ElaTypeCode.Char: return CHAR;
				case ElaTypeCode.Integer: return INT;
				case ElaTypeCode.Long: return LONG;
				case ElaTypeCode.Single: return SINGLE;
				case ElaTypeCode.Double: return DOUBLE;
				case ElaTypeCode.Boolean: return BOOL;
				case ElaTypeCode.String: return STRING;
				case ElaTypeCode.List: return LIST;
				case ElaTypeCode.Tuple: return TUPLE;
				case ElaTypeCode.Record: return RECORD;
				case ElaTypeCode.Function: return FUN;
				case ElaTypeCode.Unit: return UNIT;
				case ElaTypeCode.Module: return MOD;
				case ElaTypeCode.Object: return OBJ;
				case ElaTypeCode.Lazy: return LAZ;
                default: return ERR;
			}
		}
	}
}
