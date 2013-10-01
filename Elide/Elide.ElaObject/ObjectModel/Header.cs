using System;

namespace Elide.ElaObject.ObjectModel
{
    public sealed class Header
    {
        internal Header(int version, Version elaVersion, DateTime date)
        {
            Version = version;
            ElaVersion = elaVersion;
            Date = date;
        }

        public int Version { get; private set; }

        public Version ElaVersion { get; private set; }

        public DateTime Date { get; private set; }
    }
}
