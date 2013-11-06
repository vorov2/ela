using System;

namespace Ela
{
    public static class ElaVersionInfo
    {
        public static Version Version
        {
            get { return new Version(Const.Version); }
        }
    }
}
