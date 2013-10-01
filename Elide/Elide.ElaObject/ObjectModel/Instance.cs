using System;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Instance
    {
        internal Instance(string type, string @class, int typeId, int classId, int line, int col)
        {
            Type = type;
            Class = @class;
            TypeModuleId = typeId;
            ClassModuleId = classId;
            Line = line;
            Column = col;
        }

        public string Type { get; private set; }

        public string Class { get; private set; }

        public int TypeModuleId { get; private set; }

        public int ClassModuleId { get; private set; }

        public int Line { get; private set; }

        public int Column { get; private set; }
    }
}
