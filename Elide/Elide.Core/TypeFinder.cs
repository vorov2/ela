using System;

namespace Elide.Core
{
    public static class TypeFinder
    {
        public static Type Get(string typeStr)
        {
            if (typeStr == null)
                return null;

            var type = Type.GetType(typeStr);

            if (type == null)
                throw new ElideException("Unable to find type '{0}'.", typeStr);

            return type;
        }
    }
}
