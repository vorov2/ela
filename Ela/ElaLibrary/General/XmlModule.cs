using System;
using System.Collections.Generic;
using System.Xml;
using Ela.Linking;
using Ela.Runtime.ObjectModel;
using System.IO;
using Ela.Runtime;
using System.Text;

namespace Ela.Library.General
{
    public sealed class XmlModule : ForeignModule
    {
        public override void Initialize()
        {
            Add<String,ElaRecord>("fromString", FromString);
        }


        public ElaRecord FromString(string xmlSource)
        {
            return ReadTree(xmlSource);
        }

        private ElaRecord ReadNode(XmlReader xr)
        {
            var list = new List<ElaRecordField>();
            var sxr = xr.ReadSubtree();
            sxr.Read();

            if (sxr.AttributeCount > 0)
            {
                for (var i = 0; i < sxr.AttributeCount; i++)
                {
                    sxr.MoveToAttribute(i);
                    list.Add(new ElaRecordField(sxr.Name, new ElaValue(sxr.Value)));
                }
            }

            var clist = new List<ElaRecord>();
            var sb = new StringBuilder();

            while (sxr.Read())
            {
                if (sxr.NodeType == XmlNodeType.Element)
                    clist.Add(new ElaRecord(new ElaRecordField(sxr.Name, ReadNode(sxr))));
                if (sxr.NodeType == XmlNodeType.Text || sxr.NodeType == XmlNodeType.CDATA)
                    sb.Append(sxr.ReadString());
            }

            if (clist.Count > 0)
                list.Add(new ElaRecordField("'children", ElaList.FromEnumerable(clist)));

            if (sb.Length > 0)
                list.Add(new ElaRecordField("'value", sb.ToString()));

            return new ElaRecord(list.ToArray());
        }

        private ElaRecord ReadTree(string xmlSource)
        {
            var sr = new StringReader(xmlSource);
            var xr = new XmlTextReader(sr);
            var list = new List<ElaRecordField>();

            while (xr.Read())
            {
                if (xr.IsStartElement())
                {
                    var name = xr.Name;
                    var rec = ReadNode(xr);
                    list.Add(new ElaRecordField(name, new ElaValue(rec)));
                }
            }

            return new ElaRecord(list.ToArray());
        }
    }
}
