using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Eladoc
{
    public class DocItem
    {
        public DocItem()
        {
            Lines = new List<DocLine>();
        }

        public string Name
        {
            get
            {
                if (String.IsNullOrEmpty(Header) || Header.Trim(' ').Length == 0)
                    return Header;

                var idx = Header.IndexOf(' ');

                if (idx == -1)
                    return Header;

                return Header.Substring(0, Header.IndexOf(' '));
            }
        }

        public string[] GetArguments()
        {
            if (Header == null)
                return null;

            return Header.Replace(Name, String.Empty).Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        public string Header { get; set; }

        public List<DocLine> Lines { get; private set; }
    }
}
