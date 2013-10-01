using System;
using System.Collections.Generic;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class ElaObjectFile
    {
        internal ElaObjectFile(Header header, IEnumerable<Reference> refs, IEnumerable<Global> globals, IEnumerable<LateBound> lateBounds, 
            IEnumerable<Layout> layouts, IEnumerable<String> strings, IEnumerable<OpCode> opCodes, IEnumerable<String> types, IEnumerable<Class> classes, 
            IEnumerable<Instance> instances, IEnumerable<String> constructors)
        {
            Header = header;
            References = refs;
            Globals = globals;
            LateBounds = lateBounds;
            Layouts = layouts;
            Strings = strings;
            OpCodes = opCodes;
            Types = types;
            Classes = classes;
            Instances = instances;
            Constructors = constructors;
        }

        public Header Header { get; private set; }

        public IEnumerable<Reference> References { get; private set; }

        public IEnumerable<Global> Globals { get; private set; }

        public IEnumerable<LateBound> LateBounds { get; private set; }

        public IEnumerable<Layout> Layouts { get; private set; }

        public IEnumerable<String> Strings { get; private set; }

        public IEnumerable<OpCode> OpCodes { get; private set; }

        public IEnumerable<String> Types { get; private set; }

        public IEnumerable<Class> Classes { get; private set; }

        public IEnumerable<Instance> Instances { get; private set; }

        public IEnumerable<String> Constructors { get; private set; }
    }
}
