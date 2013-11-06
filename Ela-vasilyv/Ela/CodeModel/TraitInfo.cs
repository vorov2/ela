using System;

namespace Ela.CodeModel
{
    public sealed class TraitInfo
    {
        public readonly string Prefix;
        public readonly string Name;

        public TraitInfo(string prefix, string name)
        {
            Prefix = prefix;
            Name = name;
        }

        public override string ToString()
        {
            return Prefix != null ? Prefix + "." + Name : Name;
        }
    }
}
