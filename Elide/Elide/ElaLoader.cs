using System;
using System.IO;
using System.Reflection;
using Elide.Core;

namespace Elide
{
    public static class ElaLoader
    {
        public static void AttachLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += AssemblyResolve;
        }

        private static Assembly AssemblyResolve(object sender, ResolveEventArgs args)
        {
            if (args.Name.ToLower().StartsWith("ela,"))
            {
                var loc = new FileInfo(typeof(ElaLoader).Assembly.Location).Directory;

                if (loc.Parent == null)
                    throw Fail();

                var elaDir = new DirectoryInfo(Path.Combine(loc.Parent.FullName, "Ela"));

                if (!elaDir.Exists)
                    throw Fail();

                var elaPath = new FileInfo(Path.Combine(elaDir.FullName, "ela.dll"));

                if (!elaPath.Exists)
                    throw Fail();

                var asm = Assembly.LoadFrom(elaPath.FullName);
                return asm;
            }
            else
                return null;
        }

        private static Exception Fail()
        {
            return new ElideException("Unable to load Ela.");
        }
    }
}
