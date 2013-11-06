using System;
using Ela.Runtime.ObjectModel;
using Ela.Runtime;
using System.Text;
using System.Collections.Generic;
using Ela.CodeModel;
using Ela.Parsing;

namespace Ela.Linking
{
    internal sealed class LangModule : ForeignModule
    {
        public override void Initialize()
        {
            Add<ElaValue,ElaValue>("asInt", AsInt);
            Add<ElaValue,ElaValue>("asLong", AsLong);
            Add<ElaValue,ElaValue>("asSingle", AsSingle);
            Add<ElaValue,ElaValue>("asDouble", AsDouble);
            Add<ElaValue,ElaValue>("asBool", AsBool);
            Add<ElaValue,ElaValue>("asChar", AsChar);
            Add<ElaValue,ElaValue>("asString", AsString);
            Add<ElaValue,ElaTuple>("asTuple", AsTuple);
            Add<Int32,ElaRecord,ElaValue>("showRecordKey", ShowRecordKey);
            Add<String,ElaValue,ElaValue>("toString", ToString);
            Add<String,ElaValue>("readLiteral", Read);
        }

        public ElaValue ToString(string format, ElaValue val)
        {
            return new ElaValue(val.ToString(format, Culture.NumberFormat));
        }

        public ElaValue ShowRecordKey(int field, ElaRecord rec)
        {
            var fl = rec.keys[field];

            if (fl.IndexOf(' ') != -1 || Format.IsSymbolic(fl))
                return new ElaValue("\"" + fl + "\"");

            return new ElaValue(fl);
        }

        public ElaValue AsString(ElaValue val)
        {
            if (val.TypeId > SysConst.MAX_TYP)
            {
                var tid = val.TypeId;
                var sb = new StringBuilder();
                var usb = val.Ref as ElaUserType2;
                var xs = new List<String>();

                while (usb != null)
                {
                    xs.Add(usb.Value2.DirectGetString());
                    var td = usb.Value1.Ref;

                    if (td.TypeId == tid)
                        usb = td as ElaUserType2;
                    else
                    {
                        usb = null;
                        xs.Add(td.ToString());
                    }
                }

                for (var i = xs.Count - 1; i > -1; i--)
                    sb.Append(xs[i]);

                return new ElaValue(sb.ToString());
            }

            return new ElaValue(val.AsString());
        }

        public ElaValue AsInt(ElaValue val)
        {
            return new ElaValue(val.GetInt());
        }

        public ElaValue AsLong(ElaValue val)
        {
            return new ElaValue(val.GetLong());
        }
        
        public ElaValue AsSingle(ElaValue val)
        {
            return new ElaValue(val.GetSingle());
        }

        public ElaValue AsDouble(ElaValue val)
        {
            return new ElaValue(val.GetDouble());
        }
        
        public ElaValue AsChar(ElaValue val)
        {
            return new ElaValue(val.GetChar());
        }
        
        public ElaValue AsBool(ElaValue val)
        {
            return new ElaValue(val.GetBool());
        }

        public ElaTuple AsTuple(ElaValue val)
        {
            if (val.TypeId == ElaMachine.LST)
            {
                var lst = (ElaList)val.Ref;
                var vals = new List<ElaValue>();

                foreach (var v in lst)
                    vals.Add(v);

                return new ElaTuple(vals.ToArray());
            }
            else
            {
                var rec = (ElaRecord)val.Ref;
                return new ElaTuple(rec.values);
            }
        }

        private ElaValue Read(string str)
        {
            var p = new ElaParser();
            var res = p.Parse(str);

            if (!res.Success)
                throw Fail(str);

            var prog = res.Program;

            if (prog.Instances != null
                || prog.Includes.Count != 0
                || prog.Classes != null
                || prog.Types != null
                || prog.TopLevel.Equations.Count != 1)
                throw Fail(str);

            var eq = prog.TopLevel.Equations[0];

            if (eq.Right != null)
                throw Fail(str);

            return Read(eq.Left, str);
        }

        private ElaValue Read(ElaExpression exp, string str)
        {
            switch (exp.Type)
            {
                case ElaNodeType.ListLiteral:
                    {
                        var n = (ElaListLiteral)exp;
                        var arr = new ElaValue[n.Values.Count];

                        for (var i = 0; i < arr.Length; i++)
                            arr[i] = Read(n.Values[i], str);

                        return new ElaValue(ElaList.FromEnumerable(arr));
                    }
                case ElaNodeType.TupleLiteral:
                    {
                        var n = (ElaTupleLiteral)exp;
                        var arr = new ElaValue[n.Parameters.Count];

                        for (var i = 0; i < arr.Length; i++)
                            arr[i] = Read(n.Parameters[i], str);

                        return new ElaValue(new ElaTuple(arr));
                    }
                case ElaNodeType.RecordLiteral:
                    {
                        var n = (ElaRecordLiteral)exp;
                        var arr = new ElaRecordField[n.Fields.Count];

                        for (var i = 0; i < arr.Length; i++)
                        {
                            var f = n.Fields[i];
                            arr[i] = new ElaRecordField(f.FieldName, Read(f.FieldValue, str));
                        }

                        return new ElaValue(new ElaRecord(arr));
                    }
                case ElaNodeType.Primitive:
                    {
                        var n = (ElaPrimitive)exp;

                        switch (n.Value.LiteralType)
                        {
                            case ElaTypeCode.Integer:
                                return new ElaValue(n.Value.AsInteger());
                            case ElaTypeCode.Single:
                                return new ElaValue(n.Value.AsReal());
                            case ElaTypeCode.Double:
                                return new ElaValue(n.Value.AsDouble());
                            case ElaTypeCode.Long:
                                return new ElaValue(n.Value.AsLong());
                            case ElaTypeCode.Char:
                                return new ElaValue(n.Value.AsChar());
                            case ElaTypeCode.Boolean:
                                return new ElaValue(n.Value.AsBoolean());
                            case ElaTypeCode.String:
                                return new ElaValue(n.Value.AsString());
                            default:
                                throw Fail(str);
                        }
                    }
                case ElaNodeType.UnitLiteral:
                    return new ElaValue(ElaUnit.Instance);
                default:
                    throw Fail(str);
            }
        }

        private Exception Fail(string str)
        {
            return new ElaException(
                String.Format("Unable to read a literal from a string \"{0}\".", str));
        }
    }
}
