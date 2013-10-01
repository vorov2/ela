using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Elide.Core
{
    public static class TypeCreator
    {
        public static object New(Type type)
        {
            return New<Object>(type);
        }

        public static T New<T>(Type type) where T : class
        {
            var ret = default(Object);

            try
            {
                ret = Activator.CreateInstance(type);
            }
            catch (Exception ex)
            {
                throw new ElideException(ex, "Unable to create an instance of type '{0}'.", type);
            }

            var typ = ret as T;

            if (typ == null)
                throw new ElideException("Instance of type '{0}' should be derived from '{1}'.", ret.GetType(), typeof(T));

            return typ;
        }
    }
}
