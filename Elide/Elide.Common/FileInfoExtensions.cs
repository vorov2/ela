using System;
using System.IO;

namespace Elide
{
    public static class FileInfoExtensions
    {
        public static bool HasExtension(this FileInfo fi, string ext)
        {
            if (String.IsNullOrEmpty(ext) && String.IsNullOrEmpty(fi.Extension))
                return true;
            else if (String.IsNullOrEmpty(ext))
                return false;

            ext = ext[0] != '.' ? "." + ext : ext;
            return StringComparer.OrdinalIgnoreCase.Equals(ext, fi.Extension);
        }

        public static string ShortName(this FileInfo fi)
        {
            return fi.Name.Replace(fi.Extension, String.Empty);
        }
    }
}
