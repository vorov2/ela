using System;
using System.Collections.Generic;
using System.Xml;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using System.IO;
using Ela.Runtime;
using System.Text;
using System.Web.Script.Serialization;

namespace Ela.Library.General
{
    public sealed class JsonModule : ForeignModule
    {
        public override void Initialize()
        {
            Add<String,ElaRecord>("fromString", FromString);
        }

        public ElaRecord FromString(string jsonSource)
        {
            var serializer = new JavaScriptSerializer();
            var jsonObject = serializer.DeserializeObject(jsonSource);
            return BuildRecord(jsonObject as Dictionary<String,Object>);
        }

        private ElaRecord BuildRecord(Dictionary<String,Object> dict)
        {
            var fields = new List<ElaRecordField>();

            foreach (var kv in dict)
            {
                if (kv.Value is object[])
                    fields.Add(new ElaRecordField(kv.Key, new ElaValue(BuildList((object[])kv.Value))));
                else if (kv.Value is Dictionary<String,Object>)
                    fields.Add(new ElaRecordField(kv.Key, new ElaValue(BuildRecord((Dictionary<String,Object>)kv.Value))));
                else
                    fields.Add(new ElaRecordField(kv.Key, ElaValue.FromObject(kv.Value)));
            }

            return new ElaRecord(fields.ToArray());
        }

        private ElaList BuildList(object[] array)
        {
            var vals = new List<ElaValue>();

            foreach (var o in array)
            {
                if (o is Dictionary<String,Object>)
                    vals.Add(new ElaValue(BuildRecord((Dictionary<String,Object>)o)));
                else
                    vals.Add(ElaValue.FromObject(o));
            }

            return ElaList.FromEnumerable(vals);
        }
    }
}
