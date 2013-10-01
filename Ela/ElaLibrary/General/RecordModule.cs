using System;
using System.Collections.Generic;
using Ela.Linking;
using Ela.Runtime;
using Ela.Runtime.ObjectModel;

namespace Ela.Library.General
{
    public sealed class RecordModule : ForeignModule
    {
        public RecordModule()
        {

        }

        public override void Initialize()
        {
            Add<ElaRecord,ElaRecord,ElaRecord>("addFields", AddFields);
            Add<String,ElaValue,ElaRecord,ElaRecord>("addField", AddField);
            Add<IEnumerable<String>,ElaRecord,ElaRecord>("removeFields", RemoveFields);
            Add<String,ElaRecord,ElaRecord>("removeField", RemoveField);
            Add<String,ElaValue,ElaRecord,ElaRecord>("changeField", ChangeField);
            Add<ElaRecord,ElaList>("fields", GetFields);
        }

        public ElaRecord AddField(string field, ElaValue value, ElaRecord rec)
        {
            var fieldList = new List<ElaRecordField>();
            fieldList.AddRange(rec);
            fieldList.Add(new ElaRecordField(field, value));
            return new ElaRecord(fieldList.ToArray());
        }

        public ElaRecord AddFields(ElaRecord fields, ElaRecord rec)
        {
            var fieldList = new List<ElaRecordField>();
            fieldList.AddRange(rec);
            fieldList.AddRange(fields);
            return new ElaRecord(fieldList.ToArray());
        }

        public ElaRecord RemoveFields(IEnumerable<String> fields, ElaRecord rec)
        {
            var fieldList = new List<ElaRecordField>();
            var fieldArr = new List<String>(fields);

            foreach (var f in rec)
                if (fieldArr.IndexOf(f.Field) == -1)
                    fieldList.Add(f);

            return new ElaRecord(fieldList.ToArray());
        }

        public ElaRecord RemoveField(string field, ElaRecord rec)
        {
            var fieldList = new List<ElaRecordField>();
            
            foreach (var f in rec)
                if (f.Field != field)
                    fieldList.Add(f);

            return new ElaRecord(fieldList.ToArray());
        }

        public ElaRecord ChangeField(string field, ElaValue value, ElaRecord rec)
        {
            var fieldList = new List<ElaRecordField>();

            foreach (var f in rec)
                if (f.Field == field)
                    fieldList.Add(new ElaRecordField(f.Field, value));
                else
                    fieldList.Add(f);

            return new ElaRecord(fieldList.ToArray());
        }

        public ElaList GetFields(ElaRecord rec)
        {
            return ElaList.FromEnumerable(rec.GetKeys());
        }
    }
}
