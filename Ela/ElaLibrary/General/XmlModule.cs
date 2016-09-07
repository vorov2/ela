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
        public sealed class ElaXmlDocument : ElaObject
        {
            
        }

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
                var alist = new List<ElaRecordField>();

                for (var i = 0; i < sxr.AttributeCount; i++)
                {
                    sxr.MoveToAttribute(i);
                    //alist.Add(new ElaRecordField(sxr.Name, new ElaValue(sxr.Value)));
                    list.Add(new ElaRecordField(sxr.Name, new ElaValue(sxr.Value)));
                }

                //list.Add(new ElaRecordField("attr", new ElaValue(new ElaRecord(alist.ToArray()))));
            }

            var clist = new List<ElaRecordField>();
            var sb = new StringBuilder();

            while (sxr.Read())
            {
                if (sxr.NodeType == XmlNodeType.Element)
                    list.Add(new ElaRecordField(sxr.Name, new ElaValue(ReadNode(sxr))));
                if (sxr.NodeType == XmlNodeType.Text || sxr.NodeType == XmlNodeType.CDATA)
                    sb.Append(sxr.ReadString());
            }

            //if (clist.Count > 0)
            //    list.Add(new ElaRecordField("child", new ElaRecord(clist.ToArray())));

            if (sb.Length > 0)
                list.Add(new ElaRecordField("value'", sb.ToString()));

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
