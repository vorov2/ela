using System;
using Ela.CodeModel;
using Ela.Compilation;

namespace Ela
{
    internal static class Builtins
    {
        internal static ElaBuiltinKind Kind(string func)
        {
            switch (func)
            {
                case "not": return ElaBuiltinKind.Not;
                case "force": return ElaBuiltinKind.Force;
                case "length": return ElaBuiltinKind.Length;
                case "head": return ElaBuiltinKind.Head;
                case "tail": return ElaBuiltinKind.Tail;
                case "isnil": return ElaBuiltinKind.IsNil;

                case "show": return ElaBuiltinKind.Show;
                case "fpipe": return ElaBuiltinKind.ForwardPipe;
                case "bpipe": return ElaBuiltinKind.BackwardPipe;
                case "equal": return ElaBuiltinKind.Equal;
                case "notequal": return ElaBuiltinKind.NotEqual;
                case "greaterequal": return ElaBuiltinKind.GreaterEqual;
                case "lesserequal": return ElaBuiltinKind.LesserEqual;
                case "greater": return ElaBuiltinKind.Greater;
                case "lesser": return ElaBuiltinKind.Lesser;
                case "add": return ElaBuiltinKind.Add;
                case "subtract": return ElaBuiltinKind.Subtract;
                case "multiply": return ElaBuiltinKind.Multiply;
                case "divide": return ElaBuiltinKind.Divide;
                case "quot": return ElaBuiltinKind.Quot;
                case "remainder": return ElaBuiltinKind.Remainder;
                case "modulus": return ElaBuiltinKind.Modulus;
                case "power": return ElaBuiltinKind.Power;
                case "negate": return ElaBuiltinKind.Negate;
                case "bitwiseand": return ElaBuiltinKind.BitwiseAnd;
                case "bitwiseor": return ElaBuiltinKind.BitwiseOr;
                case "bitwisexor": return ElaBuiltinKind.BitwiseXor;
                case "shiftright": return ElaBuiltinKind.ShiftRight;
                case "shiftleft": return ElaBuiltinKind.ShiftLeft;
                case "bitwisenot": return ElaBuiltinKind.BitwiseNot;
                case "concat": return ElaBuiltinKind.Concat;
                case "cons": return ElaBuiltinKind.Cons;

                case "getvalue": return ElaBuiltinKind.GetValue;
                case "getfield": return ElaBuiltinKind.GetValue;
                case "isfield": return ElaBuiltinKind.HasField;
                case "getvaluer": return ElaBuiltinKind.GetValueR;
                case "and": return ElaBuiltinKind.LogicalAnd;
                case "or": return ElaBuiltinKind.LogicalOr;
                case "seq": return ElaBuiltinKind.Seq;

                /* infrastructure */
                case "genmaxbound": return ElaBuiltinKind.GenMaxBound;
                case "genminbound": return ElaBuiltinKind.GenMinBound;
                case "gendefault": return ElaBuiltinKind.GenDefault;

                case "api1": return ElaBuiltinKind.Api1;
                case "api2": return ElaBuiltinKind.Api2;
                case "api3": return ElaBuiltinKind.Api3;
                case "api4": return ElaBuiltinKind.Api4;
                case "api5": return ElaBuiltinKind.Api5;
                case "api6": return ElaBuiltinKind.Api6;
                case "api7": return ElaBuiltinKind.Api7;
                case "api8": return ElaBuiltinKind.Api8;
                case "api9": return ElaBuiltinKind.Api9;
                case "api10": return ElaBuiltinKind.Api10;
                case "api11": return ElaBuiltinKind.Api11;
                case "api12": return ElaBuiltinKind.Api12;
                case "api13": return ElaBuiltinKind.Api13;

                case "api14": return ElaBuiltinKind.Api14;
                case "api15": return ElaBuiltinKind.Api15;
                case "api16": return ElaBuiltinKind.Api16;

                case "api101": return ElaBuiltinKind.Api101;
                case "api102": return ElaBuiltinKind.Api102;
                case "api103": return ElaBuiltinKind.Api103;
                case "api104": return ElaBuiltinKind.Api104;
                case "api105": return ElaBuiltinKind.Api105;
                case "api106": return ElaBuiltinKind.Api106;
                case "api107": return ElaBuiltinKind.Api107;
                default: return ElaBuiltinKind.None;
            }
        }
    }
}
